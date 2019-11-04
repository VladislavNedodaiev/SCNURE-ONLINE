using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data.Entities
{
    public partial class TeamMember
    {
        public int StartupId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
        public bool EditAccess { get; set; }

        public virtual Startup Startup { get; set; }
        public virtual User User { get; set; }
    }
}
