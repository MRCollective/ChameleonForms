using System;
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
        /// Creates the HTML for a submit &lt;button&gt;.
        /// </summary>
        /// <param name="text">The text to display in the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public HtmlAttributes Submit(string text)
        {
            return Submit(text.ToHtml());
        }

        /// <summary>
        /// Creates the HTML for a submit &lt;button&gt;.
        /// </summary>
        /// <param name="content">The content to display in the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public HtmlAttributes Submit(IHtmlString content)
        {
            if (content == null)
                throw new ArgumentNullException("content", "Content must be specified");

            return new LazyHtmlAttributes(h => Form.Template.Button(content, "submit", null, null, h));
        }

        /// <summary>
        /// Creates the HTML for a submit button that submits a value in the form post when clicked.
        /// </summary>
        /// <param name="name">The name of the element</param>
        /// <param name="value">The value to submit with the form</param>
        /// <param name="content">The text the user sees (leave as null if you want the user to see the value instead)</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public HtmlAttributes Submit(string name, string value, IHtmlString content = null)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Expected value to be specified");

            return new LazyHtmlAttributes(h => Form.Template.Button(content, "submit", name, value, h));
        }

        /// <summary>
        /// Creates the HTML for a &lt;button&gt;.
        /// </summary>
        /// <param name="text">The text to display in the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public HtmlAttributes Button(string text)
        {
            return Button(text.ToHtml());
        }

        /// <summary>
        /// Creates the HTML for a &lt;button&gt;.
        /// </summary>
        /// <param name="content">The content to display in the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public HtmlAttributes Button(IHtmlString content)
        {
            if (content == null)
                throw new ArgumentNullException("content", "Content must be specified");

            return new LazyHtmlAttributes(h => Form.Template.Button(content, null, null, null, h));
        }

        /// <summary>
        /// Creates the HTML for a reset &lt;button&gt;.
        /// </summary>
        /// <param name="text">The text to display for the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public HtmlAttributes Reset(string text)
        {
            return Reset(text.ToHtml());
        }

        /// <summary>
        /// Creates the HTML for a reset &lt;button&gt;.
        /// </summary>
        /// <param name="content">The content to display for the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public HtmlAttributes Reset(IHtmlString content)
        {
            if (content == null)
                throw new ArgumentNullException("content", "Content must be specified");

            return new LazyHtmlAttributes(h => Form.Template.Button(content, "reset", null, null, h));
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
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <param name="form">The form the navigation is being created in</param>
        /// <returns>The form navigation</returns>
        public static Navigation<TModel, TTemplate> BeginNavigation<TModel, TTemplate>(this IForm<TModel, TTemplate> form) where TTemplate : IFormTemplate
        {
            return new Navigation<TModel, TTemplate>(form);
        }
    }
}
