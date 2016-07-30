using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Enums;
using JetBrains.Annotations;

namespace ChameleonForms
{
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
        public static IForm<TModel> BeginChameleonForm<TModel>(this HtmlHelper<TModel> helper, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
        {
            return new Form<TModel>(new MvcViewWithModel<TModel>(helper), FormTemplate.Default, action, method.ToFormSubmitMethod(), htmlAttributes, enctype);
        }

        /// <summary>
        /// Renders the given partial in the context of the parent model.
        /// </summary>
        /// <typeparam name="TModel">The form model type</typeparam>
        /// <param name="form">The form</param>
        /// <param name="partialViewName">The name of the partial view to render</param>
        /// <returns>The HTML for the rendered partial</returns>
        public static IHtmlString Partial<TModel>(this IForm<TModel> form, [AspMvcPartialView] string partialViewName)
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
        public static IHtmlString PartialFor<TModel, TPartialModel>(this IForm<TModel> form, Expression<Func<TModel, TPartialModel>> partialModelProperty, [AspMvcPartialView] string partialViewName)
        {
            return form.View.Partial(form, partialModelProperty, partialViewName).ToIHtmlString();
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
        public static IForm<TChildModel> BeginChameleonFormFor<TParentModel, TChildModel>(this HtmlHelper<TParentModel> helper, Expression<Func<TParentModel, TChildModel>> formFor, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
        {
            var childHelper = helper.For(formFor, bindFieldsToParent: false);
            return new Form<TChildModel>(new MvcViewWithModel<TChildModel>(childHelper), FormTemplate.Default, action, method.ToFormSubmitMethod(), htmlAttributes, enctype);
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
        public static IForm<TNewModel> BeginChameleonFormFor<TOriginalModel, TNewModel>(this HtmlHelper<TOriginalModel> helper, TNewModel model, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
        {
            var childHelper = helper.For(model);
            return new Form<TNewModel>(new MvcViewWithModel<TNewModel>(childHelper), FormTemplate.Default, action, method.ToFormSubmitMethod(), htmlAttributes, enctype);
        }
    }
}