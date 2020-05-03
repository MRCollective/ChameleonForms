using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace ChameleonForms.Example
{
    public interface IFieldConfigurableTagHelper
    {
        public string AddClass { get; set; }
        public string Label { get; set; }

        public IDictionary<string, string> Attrs { get; set; }
    }

    public static class FieldConfigurableTagHelperExtensions
    {
        public static IFieldConfiguration Configure(this IFieldConfiguration fc, IFieldConfigurableTagHelper th)
        {
            if (th.Label != null)
                fc.Label(th.Label);

            if (th.AddClass != null)
                fc.AddClass(th.AddClass);

            foreach (var attrKey in th.Attrs.Keys)
            {
                fc.Attr(attrKey, th.Attrs[attrKey]);
            }

            return fc;
        }
    }

    //[HtmlTargetElement(TagStructure = TagStructure.WithoutEndTag)]
    public class FieldTagHelper : TagHelper, IFieldConfigurableTagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        public ModelExpression For { get; set; }
        public IFieldConfiguration Configuration { get; set; }
        
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
            var ff = ViewContext.ViewData["ChameleonFormField"] as Field<TModel>;

            if (s != null)
            {
                if (output.TagMode == TagMode.SelfClosing)
                {
                    output.TagMode = TagMode.StartTagAndEndTag;
                    output.TagName = null;
                    output.Content.SetHtmlContent(s.FieldFor(@for).Configure(this));
                }
                else
                {
                    using (var field = s.BeginFieldFor(@for, Field.Configure().Configure(this)))
                    {
                        ViewContext.ViewData["ChameleonFormField"] = field;
                        ViewContext.Writer.WriteLine(output.GetChildContentAsync().GetAwaiter().GetResult());
                        ViewContext.ViewData.Remove("ChameleonFormField");
                    }
                }
            }
            else
            {
                output.TagMode = TagMode.StartTagAndEndTag;
                output.TagName = null;
                output.Content.SetHtmlContent(ff.FieldFor(@for).Configure(this));
            }
        }

        static Expression GetPropertySelector(Type modelType, string propertyName)
        {
            var arg = Expression.Parameter(modelType, "x");
            var property = Expression.Property(arg, propertyName);
            var exp = Expression.Lambda(property, arg);
            return exp;
        }

        public string AddClass { get; set; }
        public string Label { get; set; }
        [HtmlAttributeName("attrs", DictionaryAttributePrefix = "attr-")]
        public IDictionary<string, string> Attrs { get; set; } = new Dictionary<string, string>();
    }
}