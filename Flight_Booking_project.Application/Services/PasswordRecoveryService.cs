using EStore.Application.Services;
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.Services
{
    public class PasswordRecoveryService : IPasswordRecoveryService
    {
        private readonly IEmailRepository _emailService;
        private readonly IUserService _userService;
        private readonly InMemoryTokenStore _inMemoryTokenStore;
        private readonly IUserRepository _userRepository;
        public PasswordRecoveryService(IEmailRepository emailService, IUserService userService, InMemoryTokenStore inMemoryTokenStore, IUserRepository userRepository)
        {
            _emailService = emailService;
            _userService = userService;
            _inMemoryTokenStore = inMemoryTokenStore;
            _userRepository = userRepository;
        }

        // Mapping method to convert RegisterDto to UserDto
        private UserDto MapToUserDto(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return null;
            }

            return new UserDto
            {
                Email = registerDto.Email,
                Password = registerDto.Password // Ensure password handling is secure
            };
        }

        private User MapToUser(RegisterDto registerDto)
        {
            if (registerDto == null) return null;

            return new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Password = registerDto.Password, // Make sure to hash this before saving
                Address = registerDto.Address,
                PhoneNumber = registerDto.PhoneNumber,
                Gender = registerDto.Gender,
                AlternativeContactNumber = registerDto.AlternativeContactNumber
            };
        }

        // Updated PasswordRecoveryService methods
        public async Task<bool> SendResetLinkAsync(string email)
        {
            // Create a RegisterDto instance with the email
            var registerDto = new RegisterDto { Email = email };

            // Fetch the user details as RegisterDto
            var userDto = await _userService.GetUserByEmail(registerDto);
            if (userDto == null) { return false; }

            // Convert RegisterDto to UserDto
            var user = MapToUserDto(userDto);

            // Generate the password reset token
            var token = await _userService.GeneratePasswordResetToken(user); // Await the token generation

            // Store the token
            _inMemoryTokenStore.StoreToken(email, token, TimeSpan.FromHours(1));

            // Create the reset link
            var resetLink = $"http://localhost:3000/passwordReset?token={token}&email={email}";

            // Create the email message
            var message = $"<p>To reset your password, click the link below:</p><p><a href='{resetLink}'>Reset Password</a></p>";

            // Send the email
            _emailService.SendMailNotification(email, "Password Reset", message);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string password)
        {
            // Create a RegisterDto instance with the email
            var registerDto = new RegisterDto { Email = email };

            var isValidToken = await ValidateTokenAsync(email, token);
            if (!isValidToken)
            {
                return false;
            }

            // Get the user as RegisterDto
            var userDto = await _userService.GetUserByEmail(registerDto);
            if (userDto == null)
            {
                return false;
            }

            // Map RegisterDto to User
            var user = MapToUser(userDto);
            if (user == null)
            {
                return false; // Handle the mapping failure if needed
            }

            // Update the password
            user.Password = password; // Make sure to hash this password before saving

            // Update the user password in the repository
            await _userRepository.UpdateUserPassword(user);

            // Invalidate the token
            _inMemoryTokenStore.InvalidateToken(email);
            return true;
        }

        private async Task<bool> ValidateTokenAsync(string email, string token)
        {
            var storedTokenInfo = _inMemoryTokenStore.GetToken(email);
            if (storedTokenInfo == null)
            {
                return false; // No token found for this email
            }

            // Check if the provided token matches the stored token
            if (storedTokenInfo.Value.Token != token)
            {
                return false; // Tokens do not match
            }

            // Check if the token is expired
            if (DateTime.UtcNow > storedTokenInfo.Value.Expiration)
            {
                return false; // Token has expired
            }

            return true; // Token is valid
        }
    }
}