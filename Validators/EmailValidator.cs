using System;
using System.Text.RegularExpressions;

namespace Backend_Dis_App.Validators
{
    public class EmailValidator : IEmailValidator
    {
        private const string ValidEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                         + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                         + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        private static readonly Regex EmailRule = new Regex(ValidEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromSeconds(5));

        public bool CheckRule(object parameter)
        {
            bool result;

            try
            {
                result = EmailRule.IsMatch((string)parameter ?? string.Empty);
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}
