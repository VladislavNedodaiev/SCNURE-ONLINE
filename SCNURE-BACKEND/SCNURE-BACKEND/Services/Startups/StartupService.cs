using Microsoft.Extensions.Options;
using SCNURE_BACKEND.Data;
using SCNURE_BACKEND.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Services.Users
{
    public interface IStartupService
    {
        Task<Data.Entities.Startup> GetStartupById(int startupId);
        Task<List<Data.Entities.Startup>> GetAllStartups();
        Task<Data.Entities.Startup> AddStartup(string title, string description, Data.Entities.User creator);
    }

    public class StartupServiceImpl : IStartupService
    {
        private readonly SCContext _dbcontext;

        public StartupServiceImpl(SCContext sCContext)
        {
            _dbcontext = sCContext;
        }

        public Task<Data.Entities.Startup> GetStartupById(int startupId)
        {
            return _dbcontext.Startups.FindAsync(startupId);
        }

        public async Task<List<Data.Entities.Startup>> GetAllStartups()
        {
            return _dbcontext.Startups.AsQueryable().ToList();
        }

        public async Task<Data.Entities.Startup> AddStartup(string title, string description, Data.Entities.User creator)
        {
            _dbcontext.Users.Update(creator);
            var dbStartup = new Data.Entities.Startup()
            {
                Title = title,
                Description = description
            };
            var addedStartup = _dbcontext.Startups.Add(dbStartup).Entity;
            var dbTeamMember = new Data.Entities.TeamMember()
            {
                StartupId = addedStartup.StartupId,
                UserId = creator.UserId,
                Role = "",
                EditAccess = 1
            };
            _dbcontext.TeamMembers.Add(dbTeamMember);
            _dbcontext.SaveChanges();
            return _dbcontext.Startups.Find(addedStartup.StartupId);
        }
    }
}
