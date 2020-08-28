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
        /// The encoding type to use for the form submission. URL encoded by default.
        /// </summary>
        // ReSharper disable once IdentifierTypo
        public EncType Enctype { get; set; }

        /// <summary>
        /// The HTTP method to use for the form submission. POST by default.
        /// </summary>
        public FormMethod Method { get; set; }

        /// <summary>
        /// The action URL to post to for the form submission. Leave as default value for a self-submitting form.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Whether or not to output an antiforgery token in the form; defaults to null which will output a token if the method isn't GET.
        /// </summary>
        public bool? OutputAntiforgeryToken { get; set; }

        /// <inheritdoc />
        public override async Task ProcessWhileAwareOfModelTypeAsync<TModel>(TagHelperContext context, TagHelperOutput output)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();
            var method = Method == FormMethod.Get
                ? Microsoft.AspNetCore.Mvc.Rendering.FormMethod.Get
                : Microsoft.AspNetCore.Mvc.Rendering.FormMethod.Post;
            using (helper.BeginChameleonForm(Action ?? "", method, context.GetHtmlAttributes(), Enctype, OutputAntiforgeryToken))
            {
                var childContent = await output.GetChildContentAsync();
                childContent.WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);
            }
            output.SuppressOutput();
        }
    }
}
