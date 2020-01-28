using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Niles.PrintWeb.Shared
{
    public class ValidationUtilities
    {
        ///<summary>Rule check value is not empty string.</summary>
        public static bool NotEmptyRule(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        ///<summary>Rule check value string contains only letters, digits or underscorces</summary>
        public static bool OnlyLettersNumbersAndUnderscorcesRule(string value)
        {
            return Regex.IsMatch(value, @"^[[a-zA-Z0-9а-яА-я]");
        }

        ///<summary>Rule check value string length more than min value</summary>
        public static bool MoreThanValueLengthRule(string value, int minValue)
        {
            return value.Length >= minValue;
        }

        ///<summary>Rule check value string is valid email format</summary>
        public static bool CheckEmailFormat(string email)
        {
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
                return false;
            }

            try
            {
                if (!Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                    return false;
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            return true;
        }
    }
}