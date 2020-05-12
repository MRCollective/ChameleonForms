using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Utils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ChameleonForms.TagHelpers
{
    public class FieldTagHelper : ModelPropertyTagHelper
    {

        public IFieldConfiguration Configuration { get; set; }

        public string AddClass { get; set; }
        public string Label { get; set; }

        [HtmlAttributeName("attrs", DictionaryAttributePrefix = "attr-")]
        public IDictionary<string, string> Attrs { get; set; } = new Dictionary<string, string>();

        public override async Task ProcessUsingModelPropertyAsync<TModel, TProperty>(TagHelperContext context, TagHelperOutput output,
            Expression<Func<TModel, TProperty>> modelProperty)
        {
            var helper = ViewContext.GetHtmlHelper<TModel>();

            if (helper.IsInChameleonFormsSection())
            {
                var s = helper.GetChameleonFormsSection();
                if (output.TagMode == TagMode.SelfClosing)
                {
                    output.TagMode = TagMode.StartTagAndEndTag;
                    output.TagName = null;
                    output.Content.SetHtmlContent(s.FieldFor(modelProperty).Configure(this));
                }
                else
                {
                    using (var field = s.BeginFieldFor(modelProperty, Field.Configure().Configure(this)))
                    {
                        ViewContext.ViewData["ChameleonFormField"] = field;
                        var childContent = await output.GetChildContentAsync();
                        childContent.WriteTo(helper.ViewContext.Writer, HtmlEncoder.Default);
                        ViewContext.ViewData.Remove("ChameleonFormField");
                    }
                }
            }
            else
            {
                var ff = ViewContext.ViewData["ChameleonFormField"] as Field<TModel>;
                output.TagMode = TagMode.StartTagAndEndTag;
                output.TagName = null;
                output.Content.SetHtmlContent(ff.FieldFor(modelProperty).Configure(this));
            }
        }
    }

    internal static class FieldConfigurationExtensions
    {
        public static IFieldConfiguration Configure(this IFieldConfiguration fc, FieldTagHelper th)
        {
            if (th.Label != null)
                fc.Label(th.Label);

            if (th.AddClass != null)
                fc.AddClass(th.AddClass);

            fc.Attrs(th.Attrs);

            return fc;
        }
    }
}
