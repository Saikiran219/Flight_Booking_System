using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Flight_Booking_project.Application.Services
{
    public class PasswordRecoveryService : IPasswordRecoveryService
    {
        private readonly IEmailRepository _emailService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserManager<User> _userManager;
        public PasswordRecoveryService(IEmailRepository emailService, UserManager<User> userManager, IPasswordHasher<User> passwordHasher)
        {
            _emailService = emailService;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
             
        }
  

        // Updated PasswordRecoveryService methods
        public async Task<bool> SendResetLinkAsync(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false; 
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
       

            // Create the reset link
            var resetLink = $"http://localhost:3000/passwordReset?token={token}&email={email}";

            // Create the email message
            var message = $"<p>To reset your password, click the link below:</p><p><a href='{resetLink}'>Reset Password</a></p>";
            // Send the email
            await _emailService.SendMailNotification(email, "Password Reset", message);
            return true;
        }

        public async Task<(bool, string)> ResetPasswordAsync(string email, string token, string password)
        {
            // Validate the password
            var passwordValidationResult = ValidatePassword(password);
            if (!passwordValidationResult.isValid)
            {
                return (false, passwordValidationResult.errorMessage); // Return validation error
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (false, "User not found."); // User not found
            }

            // Optionally verify the token before resetting the password (useful for security)
            var isTokenValid = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);
            if (!isTokenValid)
            {
                return (false, "Invalid or expired token."); // Invalid or expired token
            }

            // Reset the user's password using the token
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (result.Succeeded)
            {
                return (true, "Password has been reset successfully.");
            }

            return (false, "Failed to reset the password."); // General error if something goes wrong
        }

        private (bool isValid, string errorMessage) ValidatePassword(string password)
        {
            // Check if password is at least 8 characters long
            if (password.Length < 8)
            {
                return (false, "Password must be at least 8 characters long.");
            }

            // Check for at least one uppercase letter
            if (!password.Any(char.IsUpper))
            {
                return (false, "Password must contain at least one uppercase letter.");
            }

            // Check for at least one special character
            var specialChars = "!@#$%^&*()_+[]{}|;:'\",.<>?/";
            if (!password.Any(ch => specialChars.Contains(ch)))
            {
                return (false, "Password must contain at least one special character.");
            }

            // If all checks pass, return valid
            return (true, string.Empty);
        }



    }
}