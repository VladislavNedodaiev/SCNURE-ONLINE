using Newtonsoft.Json;
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
		
		[JsonIgnore]
        public virtual Startup Startup { get; set; }
		[JsonIgnore]
        public virtual User User { get; set; }
    }
}
