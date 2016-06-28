using System.Web;

namespace ChameleonForms.Component
{
    public static class NavigationExtensions
    {
        public static ButtonHtmlAttributes Submit<T>(this Navigation<T> navigation, IHtmlString content)
        {
            return navigation.Submit(content.ToIHtml());
        }

        public static ButtonHtmlAttributes Submit<T>(this Navigation<T> navigation, string name, string value, IHtmlString content)
        {
            return navigation.Submit(name, value, content.ToIHtml());
        }

        public static ButtonHtmlAttributes Reset<T>(this Navigation<T> navigation, IHtmlString content)
        {
            return navigation.Reset(content.ToIHtml());
        }

        public static ButtonHtmlAttributes Button<T>(this Navigation<T> navigation, IHtmlString content)
        {
            return navigation.Button(content.ToIHtml());
        }
    }
}
