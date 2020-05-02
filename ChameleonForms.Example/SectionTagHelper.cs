using System;
using System.Text.Encodings.Web;
using ChameleonForms.Component;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace ChameleonForms.Example
{
    //[HtmlTargetElement(TagStructure = TagStructure.WithoutEndTag)]
    public class SectionTagHelper : TagHelper
    {
        public string Heading { get; set; }
        public Func<dynamic, IHtmlContent> LeadingHtml { get; set; }

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
            var f = ViewContext.ViewData["ChameleonForm"] as Form<TModel>;
            using (var s = f.BeginSection(heading: Heading, leadingHtml: LeadingHtml))
            {
                helper.ViewData["ChameleonFormSection"] = s;
                output.GetChildContentAsync().GetAwaiter().GetResult()
                    .WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);
            }
            output.SuppressOutput();
        }
    }
}