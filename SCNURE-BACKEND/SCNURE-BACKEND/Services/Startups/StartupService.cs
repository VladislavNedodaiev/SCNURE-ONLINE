using Microsoft.Extensions.Options;
using SCNURE_BACKEND.Data;
using SCNURE_BACKEND.Helpers;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Services.Users
{
    public interface IStartupService
    {
        Task<Data.Entities.Startup> GetStartupById(int startupId);
    }

    public class StartupServiceImpl : IStartupService
    {
        private readonly SCContext _dbcontext;
        private readonly JwtSettings _jwtSettings; //Tokens

        public StartupServiceImpl(SCContext sCContext, IOptions<JwtSettings> jwtSettings)
        {
            _dbcontext = sCContext;
            _jwtSettings = jwtSettings.Value;
        }

        public Task<Data.Entities.Startup> GetStartupById(int startupId)
        {
            return _dbcontext.Startups.FindAsync(startupId);
        }
    }
}
