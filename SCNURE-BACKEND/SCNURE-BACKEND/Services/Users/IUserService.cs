using System.Threading.Tasks;
using SCNURE_BACKEND.Data.Dtos;
using SCNURE_BACKEND.Data.Dtos.TeamMembers;
using SCNURE_BACKEND.Data.Dtos.Users;
using SCNURE_BACKEND.Data.Entities;

namespace SCNURE_BACKEND.Services.Users
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string loginOrEmail, string password);
        Task<User> GetByIdAsync(int id);
		Task<User> RegisterAsync(RegisterRequest userDto);
		Task ConfirmUserEmailAsync(string token);
        Task<User> GetUserByToken(string token);
		Task<UserProfileResponse> GetUserProfile(int userId);
		Task<UserDataResponse> GetAccountData(int userId);
		Task UpdateUser(EditUserDataRequest user);
		Task<TeamMember> AddTeamMember(AddTeamMemberRequest addTeamMemberRequest);
		Task<bool> HasEditAccess(int userId, int startupId);
    }
}
