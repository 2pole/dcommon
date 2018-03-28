using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DCommon.Security
{
    public static class PasswordAdvisor
    {
        public static int VeryWeakPasswordLength = 4;
        public static int StrongPasswordLength = 10;
        public static string DigitRegexPattern = @"[0-9]+(\.[0-9][0-9]?)?";
        public static string BothLowerAndUpperCaseRegexPattern = @"^(?=.*[a-z])(?=.*[A-Z]).+$";
        public static string SpecialCharacterRegexPattern = @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]";

        public static PasswordScore CheckStrength(string password)
        {
            if (string.IsNullOrEmpty(password))
                return PasswordScore.Blank;

            if (password.Length < VeryWeakPasswordLength)
                return PasswordScore.VeryWeak;

            int score = 1;
            if (password.Length >= StrongPasswordLength)
                score++;
            if (Regex.IsMatch(password, DigitRegexPattern, RegexOptions.Compiled))
                //number only //"^\d+$" if you need to match more than one digit.
                score++;
            if (Regex.IsMatch(password, BothLowerAndUpperCaseRegexPattern, RegexOptions.Compiled))
                //both, lower and upper case
                score++;
            if (Regex.IsMatch(password, SpecialCharacterRegexPattern, RegexOptions.Compiled)) //^[A-Z]+$
                score++;

            return (PasswordScore)score;
        }
    }
}
