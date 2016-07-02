using System.IO;
using System.Web.WebPages;

namespace ChameleonForms.Templates
{
    public class ChameleonFormsHelperPage : HelperPage
    {
        public new static void WriteTo(TextWriter writer, object value)
        {
            if (value is IHtml)
                value = ((IHtml) value).ToIHtmlString();

            HelperPage.WriteTo(writer, value);
        }
    }
}
