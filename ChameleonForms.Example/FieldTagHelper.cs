using System;
using System.Linq.Expressions;
using ChameleonForms.Component;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace ChameleonForms.Example
{
    //[HtmlTargetElement(TagStructure = TagStructure.WithoutEndTag)]
    public class FieldTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        public ModelExpression For { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            GetType().GetMethod(nameof(ProcessInternal)).MakeGenericMethod(ViewContext.ViewData.ModelMetadata.ModelType, For.Metadata.ModelType)
                .Invoke(this, new object[] {context, output, GetPropertySelector(ViewContext.ViewData.ModelMetadata.ModelType, For.Name)});
        }

        public void ProcessInternal<TModel, TField>(TagHelperContext context, TagHelperOutput output, Expression<Func<TModel, TField>> @for)
        {
            var helper = ViewContext.HttpContext.RequestServices.GetRequiredService<IHtmlHelper<TModel>>();
            (helper as HtmlHelper<TModel>)?.Contextualize(ViewContext);
            var s = ViewContext.ViewData["ChameleonFormSection"] as Section<TModel>;

            ViewContext.Writer.WriteLine(s.FieldFor(@for));
            output.SuppressOutput();
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