using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ChameleonForms.Enums;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Tag helper to create a ChameleonForms form context.
    /// </summary>
    public class ChameleonFormTagHelper : ModelAwareTagHelper
    {
        /// <summary>
        /// Form method for the tag helper.
        /// POST is the default (for when the method isn't specified).
        /// </summary>
        /// <remarks>
        /// Nullable enums don't allow you to omit the enum type in the views hence using
        /// this instead of <see cref="Microsoft.AspNetCore.Mvc.Rendering.FormMethod"/>
        /// (where GET is the default).
        /// </remarks>
        public enum FormMethod
        {
            /// <summary>
            /// HTTP POST.
            /// </summary>
            Post,
            /// <summary>
            /// HTTP GET.
            /// </summary>
            Get
        }

        /// <summary>
        /// The encoding type to use for the form submission.
        /// </summary>
        // ReSharper disable once IdentifierTypo
        public EncType Enctype { get; set; }

        /// <summary>
        /// the HTTP method to use for the form submission.
        /// </summary>
        public FormMethod Method { get; set; }

        /// <summary>
        /// The action URL to post to for the form submission.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// HTML attributes to apply to the form. You can either pass them in as a dictionary (attrs="@dictionary"), or
        /// you can pass them in as individual attributes via attr-attribute-name="attributevalue" ...
        /// </summary>
        [HtmlAttributeName("attrs", DictionaryAttributePrefix = "attr-")]
        public IDictionary<string, string> Attrs { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc />
        public override async Task ProcessWhileAwareOfModelTypeAsync<TModel>(TagHelperContext context, TagHelperOutput output)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();
            var method = Method == FormMethod.Get
                ? Microsoft.AspNetCore.Mvc.Rendering.FormMethod.Get
                : Microsoft.AspNetCore.Mvc.Rendering.FormMethod.Post;
            using (helper.BeginChameleonForm(Action ?? "", method, new HtmlAttributes(Attrs), Enctype))
            {
                var childContent = await output.GetChildContentAsync();
                childContent.WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);
            }
            output.SuppressOutput();
        }
    }
}
