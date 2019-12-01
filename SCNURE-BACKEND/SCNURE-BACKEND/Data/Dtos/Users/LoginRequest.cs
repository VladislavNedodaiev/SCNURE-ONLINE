using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Users
{
    public class LoginRequest
    {
        [Required]
        public string LoginOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
