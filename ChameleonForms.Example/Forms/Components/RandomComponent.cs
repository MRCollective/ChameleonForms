using System.IO;
using System.Text.Encodings.Web;
using ChameleonForms.Component;
using ChameleonForms.Enums;
using ChameleonForms.Example.Forms.Templates;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ChameleonForms.Example.Forms.Components
{
    /// <summary>
    /// Illustrates using a custom template class and wrapping an unencoded string output.
    /// </summary>
    public class RandomComponent<TModel> : IFormComponent<TModel>
    {
        public IForm<TModel> Form { get; private set; }

        public RandomComponent(IForm<TModel> form)
        {
            Form = form;
        }

        public override string ToString()
        {
            return ((RandomFormTemplate)Form.Template).RandomComponent();
        }
    }

    /// <summary>
    /// Illustrates using a custom template class and wrapping an encoded HTML string output.
    /// </summary>
    public class RandomComponent2<TModel> : IFormComponent<TModel>, IHtmlContent
    {
        public IForm<TModel> Form { get; private set; }

        public RandomComponent2(IForm<TModel> form)
        {
            Form = form;
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            ((RandomFormTemplate)Form.Template).RandomComponent2().WriteTo(writer, encoder);
        }
    }

    public static class RandomComponentExtensions
    {
        public static Form<TModel> BeginRandomForm<TModel>(this IHtmlHelper<TModel> helper, string action, FormMethod method, object htmlAttributes = null, EncType? enctype = null)
        {
            return new Form<TModel>(helper, new RandomFormTemplate(), action, method, htmlAttributes.ToHtmlAttributes(), enctype);
        }

        public static RandomComponent<TModel> RandomComponent<TModel>(this Form<TModel> form)
        {
            return new RandomComponent<TModel>(form);
        }

        public static RandomComponent2<TModel> RandomComponent2<TModel>(this Form<TModel> form)
        {
            return new RandomComponent2<TModel>(form);
        }
    }
}