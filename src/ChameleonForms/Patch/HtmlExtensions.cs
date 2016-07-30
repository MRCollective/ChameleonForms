#if !NETSTANDARD
using System.Net;
#endif

namespace ChameleonForms.Patch
{
    internal static class HtmlExtensions
    {
        public static string HtmlEncode(this string stringToEncode)
        {
#if NETSTANDARD
            return System.Text.Encodings.Web.HtmlEncoder.Default.Encode(stringToEncode);
#else
            return WebUtility.HtmlEncode(stringToEncode);
#endif
        }
    }
}
