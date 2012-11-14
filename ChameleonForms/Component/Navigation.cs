using System.Web;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    /// <summary>
    /// Wraps the output of the navigation area of a form.
    /// For example the area with submit buttons.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
    public class Navigation<TModel, TTemplate> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        /// <summary>
        /// Creates a form navigation area.
        /// </summary>
        /// <param name="form">The form the message is being created in</param>
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

        /// <summary>
        /// Creates the HTML for a submit button.
        /// </summary>
        /// <param name="value">The text to display for the button</param>
        /// <param name="id">The id/name of the button</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the button</param>
        /// <returns>The HTML for the submit button</returns>
        public IHtmlString Submit(string value, string id = null, HtmlAttributes htmlAttributes = null)
        {
            return HtmlCreator.BuildSubmitButton(value, "submit", id, htmlAttributes);
        }

        /// <summary>
        /// Creates the HTML for a reset button.
        /// </summary>
        /// <param name="value">The text to display for the button</param>
        /// <param name="id">The id/name of the button</param>
        /// <param name="htmlAttributes">Any HTML attributes that should be applied to the button</param>
        /// <returns>The HTML for the reset button</returns>
        public IHtmlString Reset(string value, string id = null, HtmlAttributes htmlAttributes = null)
        {
            return HtmlCreator.BuildSubmitButton(value, "reset", id, htmlAttributes);
        }
    }

    /// <summary>
    /// Extension methods for the creation of navigation sections.
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        /// Creates a navigation section.
        /// </summary>
        /// <example>
        /// @using (var n = f.BeginNavigation()) {
        ///     @n.Submit("Previous", "previous")
        ///     @n.Submit("Save", "save")
        ///     @n.Submit("Next", "next")
        /// }
        /// </example>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TTemplate"></typeparam>
        /// <param name="form"></param>
        /// <returns></returns>
        public static Navigation<TModel, TTemplate> BeginNavigation<TModel, TTemplate>(this IForm<TModel, TTemplate> form) where TTemplate : IFormTemplate
        {
            return new Navigation<TModel, TTemplate>(form);
        }
    }
}
