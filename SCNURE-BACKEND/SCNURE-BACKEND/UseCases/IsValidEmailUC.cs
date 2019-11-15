using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.UseCases
{
    public static class IsValidEmailUC
    {
        public static bool IsValidEmail(string email)
        {
            email = email.ToLower();
            if (email.Length < 3)
            {
                return false;
            }
            if (!IsLetterOrLowDigit(email[0]) || !IsLetterOrLowDigit(email[email.Length - 1]))
            {
                return false;
            }
            var atPosition = email.IndexOf('@');
            if (atPosition == -1 || email.LastIndexOf('@') != atPosition)
            {
                return false;
            }
            return true;
        }
        private static bool IsLetterOrLowDigit(char symbol)
        {
            return ((symbol >= '0' && symbol <= '9') || (symbol >= 'a' && symbol <= 'z'));
        }
    }
}
