using System.Text.RegularExpressions;

namespace PromisePayDotNet.Internals
{
    internal class Email
    {
        private static Regex emailRegex = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        public static bool IsCorrect(string email)
        {
            try
            {
                return emailRegex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

    }
}
