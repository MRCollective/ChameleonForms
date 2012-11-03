using System;
using System.Web;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    /// <summary>
    /// Chameleon Forms component - holds a reference to a form.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
    public interface IFormComponent<TModel, out TTemplate> where TTemplate : IFormTemplate
    {
        /// <summary>
        /// The form that the component is attached to.
        /// </summary>
        IForm<TModel, TTemplate> Form { get; }
    }

    /// <summary>
    /// Chameleon Forms base component class; provides an ability to easily write HTML to the page in a self-closing or nested manner.
    /// Ensure you call Initialise() at the end of the constructor when extending this class.
    /// </summary>
    public abstract class FormComponent<TModel, TTemplate> : IFormComponent<TModel, TTemplate>, IHtmlString, IDisposable where TTemplate : IFormTemplate
    {
        protected readonly bool IsSelfClosing;
        public IForm<TModel, TTemplate> Form { get; private set; }

        protected FormComponent(IForm<TModel, TTemplate> form, bool isSelfClosing)
        {
            Form = form;
            IsSelfClosing = isSelfClosing;
        }

        /// <summary>
        /// Initialises the form component; should be called at the end of the constructor of any derived classes.
        /// Writes HTML directly to the page is the component isn't self-closing
        /// </summary>
        public void Initialise()
        {
            if (!IsSelfClosing)
                Form.Write(Begin());
        }

        /// <summary>
        /// Returns the HTML representation of the beginning of the form component.
        /// </summary>
        /// <returns>The beginning HTML for the form component</returns>
        public abstract IHtmlString Begin();

        /// <summary>
        /// Returns the HTML representation of the end of the form component.
        /// </summary>
        /// <returns>The ending HTML for the form component</returns>
        public abstract IHtmlString End();

        public string ToHtmlString()
        {
            if (!IsSelfClosing)
                return null;

            return string.Format("{0}{1}", Begin().ToHtmlString(), End().ToHtmlString());
        }

        public void Dispose()
        {
            if (!IsSelfClosing)
                Form.Write(End());
        }
    }
}
