using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using JetBrains.Annotations;

namespace ChameleonForms.Component
{
    /// <summary>
    /// Extension methods to create form sections.
    /// </summary>
    public static class SectionExtensions
    {
        /// <summary>
        /// Creates a top-level form section.
        /// </summary>
        /// <example>
        /// @using (var s = f.BeginSection("Section heading")) {
        ///     @s.FieldFor(m => m.FirstName)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <param name="form">The form the section is being created in</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        /// <returns>The form section</returns>
        public static Section<TModel> BeginSection<TModel>(this IForm<TModel> form, string heading = null, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(form, heading.ToIHtml(), false, leadingHtml.ToIHtml(), htmlAttributes);
        }

        /// <summary>
        /// Creates a nested form section.
        /// </summary>
        /// <example>
        /// @using (var s = f.BeginSection("Section heading")) {
        ///     using (var ss = s.BeginSection("Nested section heading")) {
        ///         @ss.FieldFor(m => m.FirstName)
        ///     }
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <param name="section">The section the section is being created under</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        /// <returns>The nested form section</returns>
        public static Section<TModel> BeginSection<TModel>(this Section<TModel> section, string heading = null, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(section.Form, heading.ToIHtml(), true, leadingHtml.ToIHtml(), htmlAttributes);
        }

        /// <summary>
        /// Renders the given partial in the context of the parent model.
        /// </summary>
        /// <typeparam name="TModel">The form model type</typeparam>
        /// <param name="section">The current section</param>
        /// <param name="partialViewName">The name of the partial view to render</param>
        /// <returns>The HTML for the rendered partial</returns>
        public static IHtmlString Partial<TModel>(this ISection<TModel> section, [AspMvcPartialView] string partialViewName)
        {
            return PartialFor(section, m => m, partialViewName);
        }

        /// <summary>
        /// Renders the given partial in the context of the given property.
        /// Use PartialFor(m => m) to render a partial for the model itself rather than a child property.
        /// </summary>
        /// <typeparam name="TModel">The form model type</typeparam>
        /// <typeparam name="TPartialModel">The type of the model property to use for the partial model</typeparam>
        /// <param name="section">The current section</param>
        /// <param name="partialModelProperty">The property to use for the partial model</param>
        /// <param name="partialViewName">The name of the partial view to render</param>
        /// <returns>The HTML for the rendered partial</returns>
        public static IHtmlString PartialFor<TModel, TPartialModel>(this ISection<TModel> section, Expression<Func<TModel, TPartialModel>> partialModelProperty, [AspMvcPartialView] string partialViewName)
        {
            return section.Form.View.Partial(section, partialModelProperty, partialViewName).ToIHtmlString();
        }
    }
}