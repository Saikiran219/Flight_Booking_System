// AuthController.cs
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.Services;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserLoginController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPasswordRecoveryService _passwordRecoveryService;
  //  private readonly UserManger 
    public UserLoginController(IUserService userService, IPasswordRecoveryService passwordRecoveryService)
    {
        _userService = userService;
        _passwordRecoveryService = passwordRecoveryService;
    }

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

    [HttpPost("reset-password")]
    //[HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordReq request)
    {
        var result = await _passwordRecoveryService.ResetPasswordAsync(request.Email, request.Token, request.Password);
        if (result)
        {
            return Ok(new { message = "Password has been reset successfully." });
        }

        return BadRequest(new { message = "Invalid token or email." });
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

