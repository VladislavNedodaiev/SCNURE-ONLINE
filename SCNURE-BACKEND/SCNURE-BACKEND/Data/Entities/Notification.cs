using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data.Entities
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime ViewDate { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }

        public virtual User User { get; set; }
    }
}
