using AutoMapper;
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UserService : IUserService
{
   
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;

    public UserService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper, UserManager<User> userManager,
        SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
    {
      
        _configuration = configuration;
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }
    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto?.Email??"");
        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }

        var user = new User
        {
            UserName = registerDto?.Name??"",
            Email = registerDto?.Email ?? "",
            PhoneNumber = registerDto?.PhoneNumber??"",
            Address = registerDto?.Address??"",
            AlternativeContactNumber = registerDto?.AlternativeContactNumber
        };
        var result = await _userManager.CreateAsync(user, registerDto?.Password??"");
        if (!result.Succeeded)
        {
            throw new Exception("User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var roleExists = await _roleManager.RoleExistsAsync("User");
        if (!roleExists)
        {
            // If the role doesn't exist, create it
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }

        // Assign the "User" role to the newly registered user
        await _userManager.AddToRoleAsync(user, "User");

        return _mapper.Map<UserDto>(user);
    }
    private async Task<string> GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

        var claims = new[]
        {
    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
  }.Concat(roleClaims).ToArray(); ;

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<LoginResultDto> LoginAsync(UserDto userDto)
    {
        var user = await _userManager.FindByEmailAsync(userDto?.Email??"");
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var result = await _signInManager.PasswordSignInAsync(user, userDto?.Password, false, false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }
        var token = await GenerateToken(user);

        return new LoginResultDto
        {
            Token = token,
            UserId = user.Id // Return UserId from custom User class
        };

      
    }
    public async Task<RegisterDto> GetUserByEmail(RegisterDto registerDto)
    {
        if (string.IsNullOrEmpty(registerDto.Email))
        {
            return null; // Or handle as appropriate for invalid email
        }

        var user = await _userManager.FindByEmailAsync(registerDto.Email);

        if (user == null)
        {
            return null; // Or handle as appropriate for not found
        }

        // Map User entity to UserDto
        return new RegisterDto
        {
            Name = user.UserName,
            Email = user.Email,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Gender = user.Gender,
            AlternativeContactNumber = user.AlternativeContactNumber
        };
    }
    public async Task<string> GeneratePasswordResetToken(UserDto userDto)
    {
        var user = await _userManager.FindByEmailAsync(userDto?.Email??"");
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        // Generate password reset token
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        return resetToken; // Return token for password reset
    }
    public async Task CreateRoleAsync(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var role = new IdentityRole(roleName);
            await _roleManager.CreateAsync(role);
        }
        else
        {
            throw new Exception("Role Already Exists");
        }
    }
    public async Task AssignRoleToUserAsync(string userEmail, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            throw new Exception("Role does not exist");
        }
        // Get all the roles the user is currently assigned to
        var currentRoles = await _userManager.GetRolesAsync(user);

        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeRolesResult.Succeeded)
        {
            throw new Exception("Failed to remove user from previous roles");
        }

        // Assign the user to the new role
        var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);
        if (!addRoleResult.Succeeded)
        {
            throw new Exception("Failed to assign the new role");
        }
    }
}
 