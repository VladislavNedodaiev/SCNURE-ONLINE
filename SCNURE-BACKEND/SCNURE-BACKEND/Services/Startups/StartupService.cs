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
    }
}
