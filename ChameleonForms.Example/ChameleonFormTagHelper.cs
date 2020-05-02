using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ChameleonForms.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace ChameleonForms.Example
{
    //[HtmlTargetElement(TagStructure = TagStructure.WithoutEndTag)]
    public class ChameleonFormTagHelper : TagHelper
    {
        public EncType? Enctype { get; set; }
        public FormMethod Method { get; set; }
        public string Action { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        //public ModelExpression For { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            GetType().GetMethod(nameof(ProcessInternal)).MakeGenericMethod(ViewContext.ViewData.ModelMetadata.ModelType)
                .Invoke(this, new object[] {context, output});
        }

        public void ProcessInternal<TModel>(TagHelperContext context, TagHelperOutput output)
        {
            var helper = ViewContext.HttpContext.RequestServices.GetRequiredService<IHtmlHelper<TModel>>();
            (helper as HtmlHelper<TModel>)?.Contextualize(ViewContext);
            using (var f = helper.BeginChameleonForm(Action, Method, new HtmlAttributes(output.Attributes), Enctype))
            {
                helper.ViewData["ChameleonForm"] = f;
                output.GetChildContentAsync().GetAwaiter().GetResult()
                    .WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);
            }
            output.SuppressOutput();
        }
    }
}