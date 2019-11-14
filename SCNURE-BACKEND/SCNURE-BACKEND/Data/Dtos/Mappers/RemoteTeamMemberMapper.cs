using SCNURE_BACKEND.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Mappers
{
    public class RemoteTeamMemberMapper
    {
        public RemoteTeamMemberMapper()
        {

        }

        public TeamMemberResponseDto MapRemoteTeamMember(TeamMember remoteTeamMember)
        {
            var result = new TeamMemberResponseDto()
            {
                StartupId = remoteTeamMember.StartupId,
                UserId = remoteTeamMember.UserId,
                Role = remoteTeamMember.Role,
                EditAccess = remoteTeamMember.EditAccess
            };
            return result;
        }

        public List<TeamMemberResponseDto> MapRemoteTeamMembers(IEnumerable<TeamMember> remoteTeamMembers)
        {
            return remoteTeamMembers.Select(x => MapRemoteTeamMember(x)).ToList();
        }
    }
}
