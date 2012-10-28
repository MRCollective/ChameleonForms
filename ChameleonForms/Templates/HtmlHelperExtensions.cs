using System.Text;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Enums;
using Humanizer;

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

        public static IHtmlString BuildFormTag(string action, FormMethod method, object htmlAttributes = null, EncType? encType = null)
        {
            var tagBuilder = new TagBuilder("form");
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tagBuilder.MergeAttribute("action", action);
            tagBuilder.MergeAttribute("method", HtmlHelper.GetFormMethodString(method), true);
            if (encType.HasValue)
            {
                tagBuilder.MergeAttribute("enctype", encType.Humanize());
            }
            return new HtmlString(tagBuilder.ToString(TagRenderMode.StartTag));
        }

        public static IHtmlString BuildSubmitButton(string value, string type = "submit", string id = null, object htmlAttributes = null)
        {
            var t = new TagBuilder("input");
            t.Attributes.Add("value", value);
            t.Attributes.Add("type", type);
            if (id != null)
            {
                t.Attributes.Add("id", id);
                t.Attributes.Add("name", id);
            }
            t.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);

            return new HtmlString(t.ToString(TagRenderMode.SelfClosing));
        }

        public static string OutputAttributes(params object[] attributesList)
        {
            if (attributesList == null)
                return string.Empty;

            var t = new TagBuilder("p");
            foreach (var attrs in attributesList)
            {
                t.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attrs));
            }
            var sb = new StringBuilder();
            foreach (var attr in t.Attributes)
            {
                sb.Append(string.Format(" {0}=\"{1}\"",
                    HttpUtility.HtmlEncode(attr.Key),
                    HttpUtility.HtmlEncode(attr.Value))
                );
            }
            return sb.ToString();
        }
    }
}
