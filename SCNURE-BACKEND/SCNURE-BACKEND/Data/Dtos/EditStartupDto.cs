using System.ComponentModel.DataAnnotations;

namespace SCNURE_BACKEND.Data.Dtos
{
    public class EditStartupDto
    {
        [Required]
        public int StartupId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
