using System.Text;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Enums;
using Humanizer;

namespace ChameleonForms.Templates
{
    /// <summary>
    /// Helper class that outputs HTML for a view.
    /// </summary>
    public static class HtmlWriter
    {
        /// <summary>
        /// Creates the HTML for a form tag.
        /// </summary>
        /// <param name="action">The URL the form submits to</param>
        /// <param name="method">The HTTP method the form submits with</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the form; specified as an anonymous object</param>
        /// <param name="encType">The encoding type the form uses</param>
        /// <returns>The HTML for the form</returns>
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

        /// <summary>
        /// Creates the HTML for a submit button.
        /// </summary>
        /// <param name="value">The text to display for the button</param>
        /// <param name="type">The type of submit button; submit (default) or reset</param>
        /// <param name="id">The id/name to use for the button</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the button; specified as an anonymous object</param>
        /// <returns>The HTML for the submit button</returns>
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

        /// <summary>
        /// Creates the HTML for a list of HTML attributes as specified by one or more anonymous objects representing attribute values.
        /// </summary>
        /// <remarks>
        /// Precedence is given to the earlier objects passed into the method.
        /// Classes are culmulative between attribute objects.
        /// </remarks>
        /// <param name="attributesList">The attribute specification objects</param>
        /// <returns>The HTML for the attributes</returns>
        public static IHtmlString OutputAttributes(params object[] attributesList)
        {
            if (attributesList == null)
                return new HtmlString(string.Empty);

            var t = new TagBuilder("p");
            foreach (var attrs in attributesList)
            {
                var attrDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(attrs);
                if (attrDictionary.ContainsKey("class"))
                {
                    t.AddCssClass(attrDictionary["class"].ToString());
                    attrDictionary.Remove("class");
                }
                t.MergeAttributes(attrDictionary);
            }
            var sb = new StringBuilder();
            foreach (var attr in t.Attributes)
            {
                sb.Append(string.Format(" {0}=\"{1}\"",
                    HttpUtility.HtmlEncode(attr.Key),
                    HttpUtility.HtmlEncode(attr.Value))
                );
            }
            return new HtmlString(sb.ToString());
        }
    }
}
