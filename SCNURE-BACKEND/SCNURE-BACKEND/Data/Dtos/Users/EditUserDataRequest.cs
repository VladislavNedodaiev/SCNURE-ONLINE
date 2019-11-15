using SCNURE_BACKEND.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Users
{
	public class EditUserDataRequest
	{
		[Required]
		public string Login { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string SecondName { get; set; }
		[Required]
		public UserGender? Gender { get; set; }
		[Required]
		public string Photo { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public string Phone { get; set; }
		[Required]
		public bool ShowPhone { get; set; }
		[Required]
		public DateTime? Birthday { get; set; }
		[Required]
		public bool ShowBirthday { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public bool ShowEmail { get; set; }
	}
}
