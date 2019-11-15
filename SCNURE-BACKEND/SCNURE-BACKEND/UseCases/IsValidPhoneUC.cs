namespace SCNURE_BACKEND.UseCases
{
    public static class IsValidPhoneUC
    {
        public static bool IsValidPhone(string phone)
        {
            if (phone.Length < 8 || phone.Length > 15)
            {
                return false;
            }
            if (phone[0] != '+' && !IsDigit(phone[0]))
            {
                return false;
            }
            for (int i = 1; i < phone.Length; ++i)
            {
                if (!IsDigit(phone[i]))
                {
                    return false;
                }
            }
            return true;
        }
        private static bool IsDigit(char symbol)
        {
            return (symbol >= '0' && symbol <= '9');
        }
    }
}
