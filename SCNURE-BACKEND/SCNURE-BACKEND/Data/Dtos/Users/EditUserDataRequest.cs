using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Users
{
	public class EditUserDataRequest
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public string FirstName { get; set; }
		public string SecondName { get; set; }
		public string Photo { get; set; }
		public string Description { get; set; }
		public string Phone { get; set; }
		public bool ShowPhone { get; set; }
		public DateTime? Birthday { get; set; }
		public bool ShowBirthday { get; set; }
		public DateTime RegisterDate { get; set; }
		public string Email { get; set; }
		public bool ShowEmail { get; set; }
	}
}
