using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data
{
    public partial class Startups
    {
        public int StartupId { get; set; }
        public string Title { get; set; }
        public short FoundationYear { get; set; }
        public DateTime PublicationDate { get; set; }
        public int? Photo { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
