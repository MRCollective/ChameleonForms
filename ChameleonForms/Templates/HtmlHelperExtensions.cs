using System.Collections.Generic;
using System.Net.Http;
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
    }
}
