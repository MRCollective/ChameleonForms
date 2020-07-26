using System;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ChameleonForms.Component;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Creates a ChameleonForms form field context, use within a ChameleonForm form section or form field context.
    /// </summary>
    public class FieldTagHelper : ModelPropertyTagHelper
    {
        /// <inheritdoc />
        public override async Task ProcessUsingModelPropertyAsync<TModel, TProperty>(TagHelperContext context, TagHelperOutput output,
            Expression<Func<TModel, TProperty>> modelProperty)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();

            if (helper.IsInChameleonFormsField())
            {
                var ff = helper.GetChameleonFormsField();
                output.TagMode = TagMode.StartTagAndEndTag;
                output.TagName = null;
                output.Content.SetHtmlContent(ff.FieldFor(modelProperty, context.GetFieldConfiguration()));
            }
            else if (helper.IsInChameleonFormsSection())
            {
                var s = helper.GetChameleonFormsSection();
                if (output.TagMode == TagMode.SelfClosing)
                {
                    output.TagMode = TagMode.StartTagAndEndTag;
                    output.TagName = null;
                    output.Content.SetHtmlContent(s.FieldFor(modelProperty, context.GetFieldConfiguration()));
                }
                else
                {
                    using (s.BeginFieldFor(modelProperty, context.GetFieldConfiguration()))
                    {
                        var childContent = await output.GetChildContentAsync();
                        childContent.WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);
                    }

                    output.Content.SetContent("");
                    output.TagName = null;
                }
            }
            else
            {
                throw new NotSupportedException("Attempt to specify a <field> outside of a <form-section>.");
            }
        }
    }
}
