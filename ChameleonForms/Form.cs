using System;
using System.Linq.Expressions;
using System.Web;


using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Templates;
using ChameleonForms.Utils;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

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
        /// <param name="partialViewHelper">The HTML Helper from the partial view</param>
        /// <returns>The PartialViewForm wrapping the original form</returns>
        IForm<TPartialModel> CreatePartialForm<TPartialModel>(object partialModelExpression, HtmlHelper<TPartialModel> partialViewHelper);
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
        IHtmlHelper<TModel> HtmlHelper { get; }
        /// <summary>
        /// The template renderer for the current view.
        /// </summary>
        IFormTemplate Template { get; }
        /// <summary>
        /// Writes a HTML String directly to the view's output.
        /// </summary>
        /// <param name="IHtmlContent">The HTML to write to the view's output</param>
        void Write(IHtmlContent IHtmlContent);

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
        public IHtmlHelper<TModel> HtmlHelper { get; private set; }
        /// <inheritdoc />
        public IFormTemplate Template { get; private set; }

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
        public Form(IHtmlHelper<TModel> helper, IFormTemplate template, string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            HtmlHelper = helper;
            Template = template;
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            // Write method is virtual to allow it to be mocked for testing
            Write(Template.BeginForm(action, method, htmlAttributes, enctype));
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <inheritdoc />
        public virtual void Write(IHtmlContent IHtmlContent)
        {
            HtmlHelper.ViewContext.Writer.Write(IHtmlContent);
        }

        /// <inheritdoc />
        public virtual IFieldGenerator GetFieldGenerator<T>(Expression<Func<TModel, T>> property)
        {
            return new DefaultFieldGenerator<TModel, T>(HtmlHelper, property, Template);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Write(Template.EndForm());
        }

        /// <inheritdoc />
        public IForm<TPartialModel> CreatePartialForm<TPartialModel>(object partialModelExpression, HtmlHelper<TPartialModel> partialViewHelper)
        {
            var partialModelAsExpression = partialModelExpression as Expression<Func<TModel, TPartialModel>>;
            if (partialModelAsExpression == null
                && typeof(TPartialModel).IsAssignableFrom(typeof(TModel))
                && partialModelExpression is Expression<Func<TModel, TModel>>)
            {
                var partialModelAsUnboxedExpression = partialModelExpression as Expression<Func<TModel, TModel>>;
                partialModelAsExpression = partialModelAsUnboxedExpression.AddCast<TModel, TPartialModel>();
            }
            return new PartialViewForm<TModel, TPartialModel>(this, partialViewHelper, partialModelAsExpression);
        }
    }

    /// <summary>
    /// Default extension methods for <see cref="Form{TModel}"/>.
    /// </summary>
    public static class ChameleonFormExtensions
    {
        /// <summary>
        /// Constructs a <see cref="Form{TModel}"/> object with the default ChameleonForms template renderer.
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
        /// <returns>A <see cref="Form{TModel}"/> object with an instance of the default form template renderer.</returns>
        public static IForm<TModel> BeginChameleonForm<TModel>(this IHtmlHelper<TModel> helper, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
        {
            return new Form<TModel>(helper, FormTemplate.Default, action, method, htmlAttributes, enctype);
        }

        /// <summary>
        /// Renders the given partial in the context of the parent model.
        /// </summary>
        /// <typeparam name="TModel">The form model type</typeparam>
        /// <param name="form">The form</param>
        /// <param name="partialViewName">The name of the partial view to render</param>
        /// <returns>The HTML for the rendered partial</returns>
        public static IHtmlContent Partial<TModel>(this IForm<TModel> form, [AspMvcPartialView] string partialViewName)
        {
            return PartialFor(form, m => m, partialViewName);
        }

        /// <summary>
        /// Renders the given partial in the context of the given property.
        /// Use PartialFor(m => m, ...) pr Partial(...) to render a partial for the model itself rather than a child property.
        /// </summary>
        /// <typeparam name="TModel">The form model type</typeparam>
        /// <typeparam name="TPartialModel">The type of the model property to use for the partial model</typeparam>
        /// <param name="form">The form</param>
        /// <param name="partialModelProperty">The property to use for the partial model</param>
        /// <param name="partialViewName">The name of the partial view to render</param>
        /// <returns>The HTML for the rendered partial</returns>
        public static IHtmlContent PartialFor<TModel, TPartialModel>(this IForm<TModel> form, Expression<Func<TModel, TPartialModel>> partialModelProperty, [AspMvcPartialView] string partialViewName)
        {
            var formModel = (TModel) form.HtmlHelper.ViewData.Model;
            var expressionText = ExpressionHelper.GetExpressionText(partialModelProperty);
            var viewData = new ViewDataDictionary(form.HtmlHelper.ViewData);
            viewData[WebViewPageExtensions.PartialViewModelExpressionViewDataKey] = partialModelProperty;
            viewData[WebViewPageExtensions.CurrentFormViewDataKey] = form;
            viewData.TemplateInfo.HtmlFieldPrefix = form.HtmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
            return form.HtmlHelper.Partial(partialViewName, partialModelProperty.Compile().Invoke(formModel), viewData);
        }

        /// <summary>
        /// Constructs a <see cref="Form{TModel}"/> object with the default ChameleonForms template renderer using a sub-property of the current model as the model.
        /// Values will bind back to the model type of the sub-property as if that was the model all along.
        /// </summary>
        /// <example>
        /// @using (var f = Html.BeginChameleonFormFor(m => m.Subproperty, ...)) {
        ///     ...
        /// }
        /// </example>
        /// <typeparam name="TParentModel">The model type of the view</typeparam>
        /// <typeparam name="TChildModel">The model type of the sub-property to construct the form for</typeparam>
        /// <param name="helper">The HTML Helper for the current view</param>
        /// <param name="formFor">A lambda expression identifying the sub-property to construct the form for</param>
        /// <param name="action">The action the form should submit to</param>
        /// <param name="method">The HTTP method the form submission should use</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use</param>
        /// <param name="enctype">The encoding type the form submission should use</param>
        /// <returns>A <see cref="Form{TModel}"/> object with an instance of the default form template renderer.</returns>
        public static IForm<TChildModel> BeginChameleonFormFor<TParentModel, TChildModel>(this IHtmlHelper<TParentModel> helper, Expression<Func<TParentModel, TChildModel>> formFor, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
        {
            var childHelper = helper.For(formFor, bindFieldsToParent: false);
            return new Form<TChildModel>(childHelper, FormTemplate.Default, action, method, htmlAttributes, enctype);
        }

        /// <summary>
        /// Constructs a <see cref="Form{TModel}"/> object with the default ChameleonForms template renderer using the given model type and instance.
        /// Values will bind back to the model type specified as if that was the model all along.
        /// </summary>
        /// <example>
        /// @using (var f = Html.BeginChameleonFormFor(new AnotherModelType(), ...)) {
        ///     ...
        /// }
        /// @using (var f = Html.BeginChameleonFormFor(default(AnotherModelType), ...)) {
        ///     ...
        /// }
        /// </example>
        /// <remarks>
        /// This can also be done using the For() extension method and just a type:
        /// @using (var f = Html.For&lt;AnotherModelType&gt;().BeginChameleonForm(...)) {
        ///     ...
        /// }
        /// </remarks>
        /// <typeparam name="TOriginalModel">The model type of the view</typeparam>
        /// <typeparam name="TNewModel">The model type of the sub-property to construct the form for</typeparam>
        /// <param name="helper">The HTML Helper for the current view</param>
        /// <param name="model">The model to use for the form</param>
        /// <param name="action">The action the form should submit to</param>
        /// <param name="method">The HTTP method the form submission should use</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use</param>
        /// <param name="enctype">The encoding type the form submission should use</param>
        /// <returns>A <see cref="Form{TModel}"/> object with an instance of the default form template renderer.</returns>
        public static IForm<TNewModel> BeginChameleonFormFor<TOriginalModel, TNewModel>(this IHtmlHelper<TOriginalModel> helper, TNewModel model, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
        {
            var childHelper = helper.For(model);
            return new Form<TNewModel>(childHelper, FormTemplate.Default, action, method, htmlAttributes, enctype);
        }
    }
}