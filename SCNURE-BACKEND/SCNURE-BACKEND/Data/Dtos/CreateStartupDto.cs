using System.ComponentModel.DataAnnotations;

namespace SCNURE_BACKEND.Data.Dtos
{
    public class CreateStartupDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
