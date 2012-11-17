using System.Web;
using System.Web.Mvc;
using ChameleonForms.Enums;
using Humanizer;

namespace ChameleonForms.Templates
{
    /// <summary>
    /// Helper class that creates HTML for a view.
    /// </summary>
    public static class HtmlCreator
    {
        /// <summary>
        /// Creates the HTML for a form tag.
        /// </summary>
        /// <param name="action">The URL the form submits to</param>
        /// <param name="method">The HTTP method the form submits with</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the form; specified as an anonymous object</param>
        /// <param name="encType">The encoding type the form uses</param>
        /// <returns>The HTML for the form</returns>
        public static IHtmlString BuildFormTag(string action, FormMethod method, HtmlAttributes htmlAttributes = null, EncType? encType = null)
        {
            var tagBuilder = new TagBuilder("form");
            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes.Attributes);
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
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the button</param>
        /// <returns>The HTML for the submit button</returns>
        public static IHtmlString BuildSubmitButton(string value, string type = "submit", string id = null, HtmlAttributes htmlAttributes = null)
        {
            var t = new TagBuilder("input");
            t.Attributes.Add("value", value);
            t.Attributes.Add("type", type);
            if (id != null)
            {
                t.Attributes.Add("id", id);
                t.Attributes.Add("name", id);
            }
            if (htmlAttributes != null)
                t.MergeAttributes(htmlAttributes.Attributes, true);

            return new HtmlString(t.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Creates the HTML for a single checkbox.
        /// </summary>
        /// <param name="name">The name/id for the checkbox</param>
        /// <param name="isChecked">Whether or not the checkbox is currently checked</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the checkbox</param>
        /// <returns>The HTML for the checkbox</returns>
        public static IHtmlString BuildSingleCheckbox(string name, bool isChecked, HtmlAttributes htmlAttributes)
        {
            var t = new TagBuilder("input");
            t.Attributes.Add("value", "true");
            t.Attributes.Add("type", "checkbox");
            t.GenerateId(name);
            t.Attributes.Add("name", name);
            if (isChecked)
                t.Attributes.Add("checked", "checked");
            if (htmlAttributes != null)
                t.MergeAttributes(htmlAttributes.Attributes, false);

            return new HtmlString(t.ToString(TagRenderMode.SelfClosing));
        }
    }
}
