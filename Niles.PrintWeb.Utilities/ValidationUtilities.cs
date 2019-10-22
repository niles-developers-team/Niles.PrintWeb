using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Niles.PrintWeb.Utilities
{
    public class ValidationUtilities
    {
        public static string ValidateUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return "User name should not be empty.";

            Regex regex = new Regex(@"^\w+$");
            if (!regex.IsMatch(userName))
            {
                string message = "User name must contains only letters, numbers and underscores.";
                return message;
            }

            if (userName.Length < 5)
            {
                string message = "User name is to short.";
                return message;
            }

            return string.Empty;
        }

        public static string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return "Email should not be empty.";

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch
            {
                return "Email is not validated.";
            }

            try
            {
                if (!Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                    return "Email is not validated.";
            }
            catch (RegexMatchTimeoutException)
            {
                return "Email is not validated.";
            }

            return string.Empty;
        }
    }
}