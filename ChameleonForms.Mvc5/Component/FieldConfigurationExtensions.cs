using System.Web;
using ChameleonForms.Component.Config;

namespace ChameleonForms.Component
{
    public static class FieldConfigurationExtensions
    {
        public static IFieldConfiguration InlineLabel(this IFieldConfiguration fieldConfiguration, IHtmlString labelHtml)
        {
            return fieldConfiguration.InlineLabel(labelHtml.ToIHtml());
        }

        public static IFieldConfiguration Label(this IFieldConfiguration fieldConfiguration, IHtmlString labelHtml)
        {
            return fieldConfiguration.Label(labelHtml.ToIHtml());
        }

        public static IFieldConfiguration WithHint(this IFieldConfiguration fieldConfiguration, IHtmlString hint)
        {
            return fieldConfiguration.WithHint(hint.ToIHtml());
        }

        public static IFieldConfiguration Prepend(this IFieldConfiguration fieldConfiguration, IHtmlString html)
        {
            return fieldConfiguration.Prepend(html.ToIHtml());
        }

        public static IFieldConfiguration Append(this IFieldConfiguration fieldConfiguration, IHtmlString html)
        {
            return fieldConfiguration.Append(html.ToIHtml());
        }

        public static IFieldConfiguration OverrideFieldHtml(this IFieldConfiguration fieldConfiguration, IHtmlString html)
        {
            return fieldConfiguration.OverrideFieldHtml(html.ToIHtml());
        }
    }
}
