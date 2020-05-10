using System;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using ChameleonForms.Component;
using ChameleonForms.Utils;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ChameleonForms.Example
{
    //[HtmlTargetElement(TagStructure = TagStructure.WithoutEndTag)]
    public class PartialFormTagHelper : TagHelper
    {
        [AspMvcPartialView]
        public string Name { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        public ModelExpression For { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            GetType().GetMethod(nameof(ProcessInternal)).MakeGenericMethod(ViewContext.ViewData.ModelMetadata.ModelType, For.Metadata.ModelType)
                .Invoke(this, new object[] {context, output, GetPropertySelector(ViewContext.ViewData.ModelMetadata.ModelType, For.Name)});
        }

        public void ProcessInternal<TModel, TPartialModel>(TagHelperContext context, TagHelperOutput output, Expression<Func<TModel, TPartialModel>> @for)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();
            if (helper.IsInChameleonFormsSection())
            {
                var s = helper.GetChameleonFormsSection();
                output.TagMode = TagMode.StartTagAndEndTag;
                output.TagName = null;
                output.Content.SetHtmlContent(s.PartialFor(@for, Name));
            }
            else
            {
                var f = helper.GetChameleonForm();
                output.TagMode = TagMode.StartTagAndEndTag;
                output.TagName = null;
                output.Content.SetHtmlContent(f.PartialFor(@for, Name));
            }
        }

        static Expression GetPropertySelector(Type modelType, string propertyName)
        {
            var arg = Expression.Parameter(modelType, "x");
            var property = Expression.Property(arg, propertyName);
            var exp = Expression.Lambda(property, arg);
            return exp;
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
