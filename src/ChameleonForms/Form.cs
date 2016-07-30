using System;
using System.Linq.Expressions;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Templates;

namespace ChameleonForms
{
    /// <summary>
    /// Interface for a modeless cast of a Chameleon Form.
    /// </summary>
    public interface IForm
    {
        /// <summary>
        /// Returns a wrapped <see cref="PartialViewForm{TModel,TPartialModel}"/> for the given partial view information.
        /// </summary>
        /// <typeparam name="TPartialModel">The model type of the partial view</typeparam>
        /// <param name="partialModelExpression">The expression that identifies the partial model</param>
        /// <param name="partialView">The partial view</param>
        /// <returns>The PartialViewForm wrapping the original form</returns>
        IForm<TPartialModel> CreatePartialForm<TPartialModel>(object partialModelExpression, IViewWithModel<TPartialModel> partialView);
    }

    /// <summary>
    /// Interface for a Chameleon Form.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>    
    public interface IForm<TModel> : IForm, IDisposable
    {
        /// <summary>
        /// The HTML helper for the current view.
        /// </summary>
        IViewWithModel<TModel> View { get; }
        /// <summary>
        /// The template renderer for the current view.
        /// </summary>
        IFormTemplate Template { get; }
        /// <summary>
        /// Writes a HTML String directly to the view's output.
        /// </summary>
        /// <param name="htmlString">The HTML to write to the view's output</param>
        void Write(IHtml htmlString);

        /// <summary>
        /// The field generator for the given field.
        /// </summary>
        /// <param name="property">The property to return the field generator for</param>
        IFieldGenerator GetFieldGenerator<T>(Expression<Func<TModel, T>> property);
    }

    /// <summary>
    /// Default Chameleon Form implementation.
    /// </summary>
    public class Form<TModel> : IForm<TModel>
    {
        /// <inheritdoc />
        public IViewWithModel<TModel> View { get; private set; }
        /// <inheritdoc />
        public IFormTemplate Template { get; private set; }

        /// <summary>
        /// Construct a Chameleon Form.
        /// Note: Contains a call to the virtual method Write.
        /// </summary>
        /// <param name="view">The current view and its model</param>
        /// <param name="template">A template renderer instance to use to render the form</param>
        /// <param name="action">The action the form should submit to</param>
        /// <param name="method">The HTTP method the form submission should use</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use expressed as an anonymous object</param>
        /// <param name="enctype">The encoding type the form submission should use</param>
        public Form(IViewWithModel<TModel> view, IFormTemplate template, string action, FormSubmitMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            View = view;
            Template = template;
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            // Write method is virtual to allow it to be mocked for testing
            Write(Template.BeginForm(action, method, htmlAttributes, enctype));
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <inheritdoc />
        public virtual void Write(IHtml htmlString)
        {
            View.Write(htmlString);
        }

        /// <inheritdoc />
        public virtual IFieldGenerator GetFieldGenerator<T>(Expression<Func<TModel, T>> property)
        {
            return new DefaultFieldGenerator<TModel, T>(View, property, Template);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Write(Template.EndForm());
        }

        /// <inheritdoc />
        public IForm<TPartialModel> CreatePartialForm<TPartialModel>(object partialModelExpression, IViewWithModel<TPartialModel> partialView)
        {
            var partialModelAsExpression = partialModelExpression as Expression<Func<TModel, TPartialModel>>;
            return new PartialViewForm<TModel, TPartialModel>(this, partialView, partialModelAsExpression);
        }
    }
}