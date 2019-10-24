using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCNURE_BACKEND.Data.Dtos;
using SCNURE_BACKEND.Data.Entities;

namespace SCNURE_BACKEND.Services.Users
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string loginOrEmail, string password);
        Task<User> GetByIdAsync(int id);
		Task<User> RegisterAsync(RegisterDto userDto);
		Task ConfirmUserEmailAsync(string token);
    }
}
