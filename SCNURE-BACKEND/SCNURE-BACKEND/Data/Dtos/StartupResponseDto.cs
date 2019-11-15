using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos
{
    public class StartupResponseDto
    {
        public StartupResponseDto()
        {
        }

        public int StartupId { get; set; }
        public string Title { get; set; }
        public short FoundationYear { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
