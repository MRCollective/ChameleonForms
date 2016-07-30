using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component;
using ChameleonForms.Enums;
using ChameleonForms.Example.Forms.Templates;

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
    public class RandomComponent2<TModel> : IFormComponent<TModel>, IHtmlString
    {
        public IForm<TModel> Form { get; private set; }

        public RandomComponent2(IForm<TModel> form)
        {
            Form = form;
        }

        public string ToHtmlString()
        {
            return ((RandomFormTemplate)Form.Template).RandomComponent2().ToHtmlString();
        }
    }

    public static class RandomComponentExtensions
    {
        public static Form<TModel> BeginRandomForm<TModel>(this HtmlHelper<TModel> helper, string action, FormMethod method, object htmlAttributes = null, EncType? enctype = null)
        {
            return new Form<TModel>(new MvcViewWithModel<TModel>(helper), new RandomFormTemplate(), action, method.ToFormSubmitMethod(), htmlAttributes.ToHtmlAttributes(), enctype);
        }

        public static RandomComponent<TModel> RandomComponent<TModel>(this Form<TModel>  form)
        {
            return new RandomComponent<TModel>(form);
        }

        public static RandomComponent2<TModel> RandomComponent2<TModel>(this Form<TModel> form)
        {
            return new RandomComponent2<TModel>(form);
        }
    }
}