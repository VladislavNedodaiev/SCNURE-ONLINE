using SCNURE_BACKEND.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Repositories.Users
{
    public interface IUsersRepository : IAsyncRepository<User>
    {
        Task<User> GetByLoginOrEmailAsync(string loginOrEmail);
		Task<User> GetByLogin(string login);
        Task<bool> IsLoginTaken(string username);
    }
}
