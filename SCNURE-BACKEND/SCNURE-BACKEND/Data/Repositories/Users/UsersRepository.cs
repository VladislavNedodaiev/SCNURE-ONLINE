using Microsoft.EntityFrameworkCore;
using SCNURE_BACKEND.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Repositories.Users
{
    public class UsersRepository : GenericAsyncRepository<User>, IUsersRepository
    {
        public UsersRepository(SCContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByLoginOrEmailAsync(string loginOrEmail)
        {
            var userByLogin = await _dbSet.SingleOrDefaultAsync(u => u.Login == loginOrEmail);
            if (userByLogin == null)
                return await _dbSet.SingleOrDefaultAsync(u => u.Email == loginOrEmail);
            return userByLogin;
            
        }

        public async Task<bool> IsLoginTaken(string login)
        {
            return await _dbSet.AnyAsync(u => u.Login == login);
        }
    }
}
