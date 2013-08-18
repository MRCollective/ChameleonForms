using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Templates;

namespace ChameleonForms
{
    /// <summary>
    /// Interface for a Chameleon Form.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    /// <typeparam name="TTemplate">The type of HTML template render the form is using</typeparam>
    public interface IForm<TModel, out TTemplate> : IDisposable where TTemplate : IFormTemplate
    {
        /// <summary>
        /// The HTML helper for the current view.
        /// </summary>
        HtmlHelper<TModel> HtmlHelper { get; }
        /// <summary>
        /// The template renderer for the current view.
        /// </summary>
        TTemplate Template { get; }
        /// <summary>
        /// Writes a HTML String directly to the view's output.
        /// </summary>
        /// <param name="htmlString">The HTML to write to the view's output</param>
        void Write(IHtmlString htmlString);

        /// <summary>
        /// The field generator for the given field.
        /// </summary>
        /// <param name="property">The property to return the field generator for</param>
        IFieldGenerator GetFieldGenerator<T>(Expression<Func<TModel, T>> property);
    }

    /// <summary>
    /// Default Chameleon Form implementation.
    /// </summary>
    public class Form<TModel, TTemplate> : IForm<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        public HtmlHelper<TModel> HtmlHelper { get; private set; }
        public TTemplate Template { get; private set; }

        /// <summary>
        /// Construct a Chameleon Form.
        /// Note: Contains a call to the virtual method Write.
        /// </summary>
        /// <param name="helper">The HTML Helper for the current view</param>
        /// <param name="template">A template renderer instance to use to render the form</param>
        /// <param name="action">The action the form should submit to</param>
        /// <param name="method">The HTTP method the form submission should use</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use expressed as an anonymous object</param>
        /// <param name="enctype">The encoding type the form submission should use</param>
        public Form(HtmlHelper<TModel> helper, TTemplate template, string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            HtmlHelper = helper;
            Template = template;
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            // Write method is virtual to allow it to be mocked for testing
            Write(Template.BeginForm(action, method, htmlAttributes, enctype));
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        public virtual void Write(IHtmlString htmlString)
        {
            HtmlHelper.ViewContext.Writer.Write(htmlString);
        }

        public virtual IFieldGenerator GetFieldGenerator<T>(Expression<Func<TModel, T>> property)
        {
            return new DefaultFieldGenerator<TModel, T>(HtmlHelper, property, Template);
        }

        public void Dispose()
        {
            Write(Template.EndForm());
        }
    }

    /// <summary>
    /// Default extension methods for <see cref="Form{TModel,TTemplate}"/>.
    /// </summary>
    public static class ChameleonFormExtensions
    {
        /// <summary>
        /// Constructs a <see cref="Form{TModel,TTemplate}"/> object with the default Chameleon form template renderer.
        /// </summary>
        /// <example>
        /// @using (var f = Html.BeginChameleonForm(...)) {
        ///     ...
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <param name="helper">The HTML Helper for the current view</param>
        /// <param name="action">The action the form should submit to</param>
        /// <param name="method">The HTTP method the form submission should use</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use</param>
        /// <param name="enctype">The encoding type the form submission should use</param>
        /// <returns>A <see cref="Form{TModel,TTemplate}"/> object with an instance of the default form template renderer.</returns>
        public static IForm<TModel, IFormTemplate> BeginChameleonForm<TModel>(this HtmlHelper<TModel> helper, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
        {
            return new Form<TModel, IFormTemplate>(helper, Config.FormTemplate, action, method, htmlAttributes, enctype);
        }

//        public static IForm<TModel, IFormTemplate> WithTemplate<TModel>(this IForm<TModel, IFormTemplate> form, IFormTemplate template)
//        {
//            form.Template = template;
//        }
    }
}