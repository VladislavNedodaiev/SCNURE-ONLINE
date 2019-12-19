using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Comments
{
    public class UserInfo
    {
        public UserInfo() { }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Login { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
    }
}
