// AuthController.cs
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Domain.EntitiesDto;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserLoginController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPasswordRecoveryService _passwordRecoveryService;
   
    public UserLoginController(IUserService userService, IPasswordRecoveryService passwordRecoveryService)
    {
        _userService = userService;
        _passwordRecoveryService = passwordRecoveryService;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            await _userService.RegisterAsync(registerDto);
            return Ok("User registered successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<LoginResultDto>> Login([FromBody] UserDto userDto)
    {
        if (userDto == null)
        {
            return BadRequest("UserDto cannot be null.");
        }

        var loginResult = await _userService.LoginAsync(userDto);

        if (loginResult != null)
        {
            return Ok(loginResult); // Return LoginResultDto in the response
        }

        return Unauthorized("Invalid username or password.");
    }

    [Authorize(Roles ="Admin,User")]
    [HttpGet("UserByEmail")]
    public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email is required."); // Return 400 Bad Request if the email is not provided
        }

        var userDto = new RegisterDto { Email = email }; // Create UserDto with the email
        var user = await _userService.GetUserByEmail(userDto);

        if (user == null)
        {
            return NotFound(); // Return 404 Not Found if the user is not found
        }
        return Ok(user); // Return 200 OK with the user data
    }


    [Authorize(Roles ="Admin")]
    [HttpPost("create-role")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        await _userService.CreateRoleAsync(roleName);
        return Ok(new { message = "Role created successfully" });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRoleToUser(string userEmail, string roleName)
    {
        await _userService.AssignRoleToUserAsync(userEmail, roleName);
        return Ok(new { message = "Role assigned successfully" });
    }


    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordReq request)
    {
        var (result,message) = await _passwordRecoveryService.ResetPasswordAsync(request.Email, request.Token, request.Password);
        if (result)
        {
            return Ok(new { message });
        }

        return BadRequest(new { message });
    }


    [HttpPost("send-reset-link")]
    public async Task<IActionResult> SendResetLink([FromBody] string email)
    {
        var result = await _passwordRecoveryService.SendResetLinkAsync(email);
        if (result)
        {
            return Ok(new { message = "Password reset link sent successfully." });
        }

        return BadRequest(new { message = "User not found or error sending email." });
    }
}

