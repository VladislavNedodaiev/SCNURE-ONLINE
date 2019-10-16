using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data.Entities
{
    public partial class Startup
    {
        public Startup()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            TeamMembers = new HashSet<TeamMember>();
        }

        public int StartupId { get; set; }
        public string Title { get; set; }
        public short FoundationYear { get; set; }
        public DateTime PublicationDate { get; set; }
        public int? Photo { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public virtual Canvase Canvases { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
