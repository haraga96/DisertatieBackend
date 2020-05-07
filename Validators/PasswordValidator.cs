using System;
using System.Text.RegularExpressions;

namespace Backend_Dis_App.Validators
{
    public class PasswordValidator : IPasswordValidator
    {
        private const string ValidPasswordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";

        private static readonly Regex PasswordRule = new Regex(ValidPasswordPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5));

        public bool CheckRule(object parameter)
        {
            bool result;

            try
            {
                result = PasswordRule.IsMatch((string)parameter ?? string.Empty);
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}
