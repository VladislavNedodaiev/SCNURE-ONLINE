using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCNURE_BACKEND.Data.Entities;

namespace SCNURE_BACKEND.Services.Users
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetById(int id);
        Task<User> Create(User user, string password);
    }
}
