using System.Text.RegularExpressions;

namespace ChameleonForms.AcceptanceTests.IntegrationTests
{
    public static class StringExtensions
    {
        private static readonly Regex RequestVerificationToken = new Regex("<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\".+?\">", RegexOptions.IgnoreCase);

        public static string SanitiseRequestVerificationToken(this string html)
        {
            return RequestVerificationToken.Replace(html, "<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"...\">");
        }
    }
}
