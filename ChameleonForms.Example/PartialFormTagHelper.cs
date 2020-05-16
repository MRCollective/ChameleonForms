using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChameleonForms.TagHelpers;
using ChameleonForms.Utils;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.Example
{
    public class FormPartialTagHelper : ModelPropertyTagHelper
    {
        /// <summary>
        /// The partial view name.
        /// </summary>
        [AspMvcPartialView]
        public string Name { get; set; }

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
namespace JetBrains.Annotations
{
    /// <summary>
    /// ASP.NET MVC attribute. If applied to a parameter, indicates that the parameter is an MVC
    /// partial view. If applied to a method, the MVC partial view name is calculated implicitly
    /// from the context. Use this attribute for custom wrappers similar to
    /// <c>System.Web.Mvc.Html.RenderPartialExtensions.RenderPartial(HtmlHelper, String)</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class AspMvcPartialViewAttribute : Attribute
    {
    }
}
