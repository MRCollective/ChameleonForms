using System.Web;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    public class Navigation<TModel, TTemplate> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        public Navigation(IForm<TModel, TTemplate> form) : base(form, false)
        {
            Initialise();
        }

        public override IHtmlString Begin()
        {
            return Form.Template.BeginNavigation();
        }

        public override IHtmlString End()
        {
            return Form.Template.EndNavigation();
        }

        public IHtmlString Submit(string value, string id = null, object htmlAttributes = null)
        {
            return Html.BuildSubmitButton(value, "submit", id, htmlAttributes);
        }

        public IHtmlString Reset(string value, string id = null, object htmlAttributes = null)
        {
            return Html.BuildSubmitButton(value, "reset", id, htmlAttributes);
        }
    }

    public static class NavigationExtensions
    {
        public static Navigation<TModel, TTemplate> BeginNavigation<TModel, TTemplate>(this IForm<TModel, TTemplate> form) where TTemplate : IFormTemplate
        {
            return new Navigation<TModel, TTemplate>(form);
        }
    }
}
