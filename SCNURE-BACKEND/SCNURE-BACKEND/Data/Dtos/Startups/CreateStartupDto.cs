using System.ComponentModel.DataAnnotations;

namespace SCNURE_BACKEND.Data.Dtos.Startups
{
    public class CreateStartupDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
