using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChameleonForms.Utils;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    /// <summary>
    /// Includes a partial view that is a different type to the parent form.
    /// </summary>
    [HtmlTargetElement("form-partial", TagStructure = TagStructure.WithoutEndTag)]
    public class FormPartialTagHelper : ModelPropertyTagHelper
    {
        /// <summary>
        /// The partial view name.
        /// </summary>
        [AspMvcPartialView]
        public string Name { get; set; }

        /// <inheritdoc />
        public override async Task ProcessUsingModelPropertyAsync<TModel, TProperty>(TagHelperContext context, TagHelperOutput output,
            Expression<Func<TModel, TProperty>> modelProperty)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();

            using (var h = helper.For((TProperty) For.Model, For.Name))
            {
                Func<Task> render = async () =>
                {
                    var content = await h.PartialAsync(Name, For.Model, h.ViewData);
                    output.TagMode = TagMode.StartTagAndEndTag;
                    output.TagName = null;
                    output.Content.SetHtmlContent(content);
                };

                using (var f = helper.GetChameleonForm().CreatePartialForm(modelProperty, h))
                {
                    if (helper.IsInChameleonFormsSection())
                    {
                        using (var s = helper.GetChameleonFormsSection().CreatePartialSection(f))
                        {
                            await render();
                        }
                    }
                    else
                    {
                        await render();
                    }
                }
            }
        }
    }
}
