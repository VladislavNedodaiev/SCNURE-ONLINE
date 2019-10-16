using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data.Entities
{
    public partial class Event
    {
        public Event()
        {
            Participants = new HashSet<Participant>();
        }

        public int EventId { get; set; }
        public int Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string Hash { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }
    }
}
