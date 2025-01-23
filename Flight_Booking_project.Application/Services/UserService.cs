using AutoMapper;
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _mapper = mapper;
    }
    public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new Exception("User already exists");
        }

        var user = _mapper.Map<User>(registerDto);
        user.Password = registerDto.Password; // Store the password directly

        // Save the user to the repository
        await _userRepository.RegisterUserAsync(user);

        // Verify the user was saved
        var savedUser = await _userRepository.GetUserByEmailAsync(user.Email);
        if (savedUser == null)
        {
            throw new Exception("User registration failed, user not found.");
        }

        return _mapper.Map<UserDto>(savedUser);
    }


    private async Task<User> AuthenticateUser(UserDto user)
    {
        User authenticatedUser = null;

        try
        {
            var userEntity = await _userRepository.GetUserByEmailAsync(user.Email);

            if (userEntity != null && user.Password == userEntity.Password) // Ensure to compare passwords securely
            {
                authenticatedUser = userEntity; // Return User entity
            }
        }
        catch (Exception ex)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }
        return authenticatedUser; // Return User entity
    }


    private async Task<string> GenerateToken(UserDto user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
  };

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public async Task<LoginResultDto> LoginAsync(UserDto user)
    {
        LoginResultDto loginResult = null;

        try
        {
            var authenticatedUser = await AuthenticateUser(user);
            if (authenticatedUser != null)
            {
                var token = await GenerateToken(new UserDto { Email = authenticatedUser.Email });
                loginResult = new LoginResultDto
                {
                    Token = token,
                    UserId = authenticatedUser.UserId // Get UserId from authenticatedUser
                };
            }
        }
        catch (Exception e)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        return loginResult; // Return LoginResultDto
    }


    public async Task<RegisterDto> GetUserByEmail(RegisterDto registerDto)
    {
        if (string.IsNullOrEmpty(registerDto.Email))
        {
            return null; // Or handle as appropriate for invalid email
        }

        var user = await _userRepository.GetUserByEmailAsync(registerDto.Email); // Fetch user from repository

        if (user == null)
        {
            return null; // Or handle as appropriate for not found
        }

        // Map User entity to UserDto
        return new RegisterDto
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Gender = user.Gender,
            AlternativeContactNumber = user.AlternativeContactNumber
        };
    }

    public Task<string> GeneratePasswordResetToken(UserDto user)
    {
        var token = GenerateToken(user);
        return token;
    }
}
 