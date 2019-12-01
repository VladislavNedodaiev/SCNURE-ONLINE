using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Users
{
	public class RegisterRequest
	{
		[Required]
		[MaxLength(24, ErrorMessage = "Login must be more than 4 symbols"), MinLength(4, ErrorMessage = "Login must be less than 24 symbols")]
		[RegularExpression(@"(?!^\d+$)^[A-Za-z\d]+$", ErrorMessage = "Login should consists of latin and digits symbols, but can't contain only digits")]
		public string Login { get; set; }
		[Required]
		[Compare(nameof(PasswordConfirmation), ErrorMessage = "Passwords don't match")]
		public string Password { get; set; }
		[Required]
		public string PasswordConfirmation { get; set; }
        [Required]
		[EmailAddress]
        public string Email { get; set; }
    }
}
