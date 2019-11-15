using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data.Entities
{
    public partial class Participant
    {
        public int ParticipantId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public int Visits { get; set; }

        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}
