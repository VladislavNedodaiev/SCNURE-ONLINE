using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.TeamMembers
{
	public class AddTeamMemberRequest
	{
		public int StartupId { get; set; }
		public string Login { get; set; }
		public string Role { get; set; }
		public bool HasEditAccess { get; set; }
	}
}
