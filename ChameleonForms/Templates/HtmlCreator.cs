using ChameleonForms.Enums;
using ChameleonForms.Utils;
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
        public static IHtml BuildFormTag(string action, FormSubmitMethod method, HtmlAttributes htmlAttributes = null, EncType? encType = null)
        {
            var tagBuilder = new TagBuilder("form");
            if (htmlAttributes != null)
                tagBuilder.MergeAttributes(htmlAttributes.Attributes);
            tagBuilder.MergeAttribute("action", action);
            tagBuilder.MergeAttribute("method", method.ToString().ToLower(), true);
            if (encType.HasValue)
            {
                tagBuilder.MergeAttribute("enctype", encType.Humanize());
            }
            return new Html(tagBuilder.ToString(TagRenderMode.StartTag));
        }

        /// <summary>
        /// Creates the HTML for a submit &lt;button&gt;.
        /// </summary>
        /// <param name="text">The text to display for the button</param>
        /// <param name="type">The type of submit button; submit (default) or reset</param>
        /// <param name="value">The value to submit with the button</param>
        /// <param name="id">The id/name to use for the button</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the button</param>
        /// <returns>The HTML for the submit button</returns>
        public static IHtml BuildButton(string text, string type = null, string id = null, string value = null, HtmlAttributes htmlAttributes = null)
        {
            return BuildButton(new Html(text), type, id, value, htmlAttributes);
        }

        /// <summary>
        /// Creates the HTML for a submit &lt;button&gt;.
        /// </summary>
        /// <param name="content">The content to display for the button</param>
        /// <param name="type">The type of submit button; submit (default) or reset</param>
        /// <param name="value">The value to submit with the button</param>
        /// <param name="id">The id/name to use for the button</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the button</param>
        /// <returns>The HTML for the submit button</returns>
        public static IHtml BuildButton(IHtml content, string type = null, string id = null, string value = null, HtmlAttributes htmlAttributes = null)
        {
            var t = new TagBuilder("button") {InnerHtml = content.ToHtmlString()};
            if (value != null)
                t.Attributes.Add("value", value);
            if (type != null)
                t.Attributes.Add("type", type);
            if (id != null)
            {
                t.Attributes.Add("id", id);
                t.Attributes.Add("name", id);
            }
            if (htmlAttributes != null)
                t.MergeAttributes(htmlAttributes.Attributes, true);

            return new Html(t.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Creates the HTML for a single checkbox.
        /// </summary>
        /// <param name="name">The name/id for the checkbox</param>
        /// <param name="isChecked">Whether or not the checkbox is currently checked</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the checkbox</param>
        /// <param name="value">The value to submit when the checkbox is ticked</param>
        /// <returns>The HTML for the checkbox</returns>
        // todo: fold this into IViewWithModel
        public static IHtml BuildSingleCheckbox(string name, bool isChecked, HtmlAttributes htmlAttributes, string value = "true")
        {
            var t = new TagBuilder("input");
            t.Attributes.Add("value", value);
            t.Attributes.Add("type", "checkbox");
            if (value == "true")
                t.GenerateId(name);
            t.Attributes.Add("name", name);
            if (isChecked)
                t.Attributes.Add("checked", "checked");
            if (htmlAttributes != null)
                t.MergeAttributes(htmlAttributes.Attributes, false);

            return new Html(t.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Creates the HTML for an input.
        /// </summary>
        /// <param name="name">The name/id of the input</param>
        /// <param name="value">The value of the input</param>
        /// <param name="type">The type of the input</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the button</param>
        /// <returns>The HTML for the input</returns>
        public static IHtml BuildInput(string name, string value, string type, HtmlAttributes htmlAttributes)
        {
            var t = new TagBuilder("input");
            if (name != null)
            {
                t.Attributes.Add("name", name);
                t.GenerateId(name);
            }
            t.Attributes.Add("value", value);
            t.Attributes.Add("type", type);
            if (htmlAttributes != null)
                t.MergeAttributes(htmlAttributes.Attributes, true);

            return new Html(t.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Creates the HTML for a label.
        /// </summary>
        /// <param name="for">The name/id for the checkbox</param>
        /// <param name="labelText">The text inside the label</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the checkbox</param>
        /// <returns>The HTML for the checkbox</returns>
        public static IHtml BuildLabel(string @for, IHtml labelText, HtmlAttributes htmlAttributes)
        {
            var t = new TagBuilder("label");
            if (@for != null)
                t.Attributes.Add("for", TagBuilder.CreateSanitizedId(@for));
            t.InnerHtml = labelText.ToHtmlString();

            if (htmlAttributes != null)
                t.MergeAttributes(htmlAttributes.Attributes, false);

            return new Html(t.ToString(TagRenderMode.Normal));
        }
    }
}
