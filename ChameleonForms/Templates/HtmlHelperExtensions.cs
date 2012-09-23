using System.Web;

namespace ChameleonForms.Templates
{
    public static class Html
    {
        public static IHtmlString Attribute(string name, string value)
        {
            if (value == null)
                return new HtmlString(string.Empty);

            //Todo: encode the values here
            return new HtmlString(string.Format(" {0}=\"{1}\"", name, value));
        }
    }
}
