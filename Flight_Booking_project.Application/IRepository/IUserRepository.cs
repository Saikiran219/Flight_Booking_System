using Flight_Booking_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<User> RegisterUserAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<User> UpdateUserPassword(User user);
    }
}
