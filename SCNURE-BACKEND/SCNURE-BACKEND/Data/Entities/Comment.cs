using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data.Entities
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int StartupId { get; set; }
        public DateTime PostDate { get; set; }
        public string Text { get; set; }

        public virtual Startup Startup { get; set; }
        public virtual User User { get; set; }
    }
}
