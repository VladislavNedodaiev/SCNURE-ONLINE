using System;
using System.Collections.Generic;

namespace SCNURE_BACKEND.Data
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string Verification { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public sbyte ShowPhone { get; set; }
        public DateTime? Birthday { get; set; }
        public sbyte ShowBirthday { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Email { get; set; }
        public sbyte ShowEmail { get; set; }
        public sbyte Admin { get; set; }
        public sbyte Membership { get; set; }
        public sbyte Ban { get; set; }
    }
}
