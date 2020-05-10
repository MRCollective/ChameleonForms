using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ChameleonForms.Component;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Creates a ChameleonForms form section context, use within a ChameleonForm form context.
    /// </summary>
    public class FormNavigationTagHelper : ModelAwareTagHelper
    {
        /// <inheritdoc />
        public override async Task ProcessWhileAwareOfModelTypeAsync<TModel>(TagHelperContext context, TagHelperOutput output)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();
            var f = helper.GetChameleonForm();

            using (f.BeginNavigation())
            {
                var childContent = await output.GetChildContentAsync();
                childContent.WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);
            }

            output.SuppressOutput();
        }
    }
}
