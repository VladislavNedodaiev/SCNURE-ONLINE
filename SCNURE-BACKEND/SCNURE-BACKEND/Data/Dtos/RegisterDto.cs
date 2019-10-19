using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos
{
	public class RegisterDto
	{
		[Required]
		public string Login { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string PasswordConfirmation { get; set; }
        [Required]
		[EmailAddress]
        public string Email { get; set; }
    }
}
