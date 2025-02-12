using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<LoginResultDto> LoginAsync(UserDto user);
        Task<RegisterDto> GetUserByEmail(RegisterDto registerDto);
        Task<string> GeneratePasswordResetToken(UserDto user);
         Task CreateRoleAsync(string roleName);
        Task AssignRoleToUserAsync(string userEmail, string roleName);
      
    }
}
