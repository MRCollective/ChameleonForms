using System;
using System.Linq.Expressions;
using ChameleonForms.Component.Config;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace ChameleonForms.Component
{
    /// <summary>
    /// Interface for a modeless cast of a ChameleonForms Section.
    /// </summary>
    public interface ISection
    {
        /// <summary>
        /// Returns a section with the same characteristics as the current section, but using the given partial form.
        /// </summary>
        /// <typeparam name="TPartialModel">The model type of the partial view</typeparam>
        /// <returns>A section with the same characteristics as the current section, but using the given partial form</returns>
        ISection<TPartialModel> CreatePartialSection<TPartialModel>(IForm<TPartialModel> partialModelForm);
    }

    /// <summary>
    /// Tagging interface for a ChameleonForms Section with a model type.
    /// </summary>
    public interface ISection<TModel> : IFormComponent<TModel> {}

    /// <summary>
    /// Wraps the output of a form section.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    public class Section<TModel> : FormComponent<TModel>, ISection, ISection<TModel>
    {
        private readonly IHtmlContent _heading;
        private readonly bool _nested;
        private readonly IHtmlContent _leadingHtml;
        private readonly HtmlAttributes _htmlAttributes;

        /// <summary>
        /// Creates a form section
        /// </summary>
        /// <param name="form">The form the message is being created in</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="nested">Whether the section is nested within another section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        public Section(IForm<TModel> form, IHtmlContent heading, bool nested, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null) : base(form, false)
        {
            _heading = heading;
            _nested = nested;
            _leadingHtml = leadingHtml;
            _htmlAttributes = htmlAttributes;
            Initialise();
        }

        /// <summary>
        /// Outputs a field with passed in HTML.
        /// </summary>
        /// <param name="labelHtml">The HTML for the label part of the field</param>
        /// <param name="elementHtml">The HTML for the field element part of the field</param>
        /// <param name="validationHtml">The HTML for the validation markup part of the field</param>
        /// <param name="metadata">Any field metadata</param>
        /// <param name="isValid">Whether or not the field is valid</param>
        /// <returns>A field configuration that can be used to output the field as well as configure it fluently</returns>
        public IFieldConfiguration Field(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml = null, ModelMetadata metadata = null, bool isValid = true)
        {
            var fc = new FieldConfiguration();
            fc.SetField(() => Form.Template.Field(labelHtml, elementHtml, validationHtml, metadata, fc, isValid));
            return fc;
        }

        /// <inheritdoc />
        public override IHtmlContent Begin()
        {
            return _nested ? Form.Template.BeginNestedSection(_heading, _leadingHtml, _htmlAttributes) : Form.Template.BeginSection(_heading, _leadingHtml, _htmlAttributes);
        }

        /// <inheritdoc />
        public override IHtmlContent End()
        {
            return _nested ? Form.Template.EndNestedSection() : Form.Template.EndSection();
        }

        /// <inheritdoc />
        public ISection<TPartialModel> CreatePartialSection<TPartialModel>(IForm<TPartialModel> partialModelForm)
        {
            return new PartialViewSection<TPartialModel>(partialModelForm);
        }
    }

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
        public static Section<TModel> BeginSection<TModel>(this IForm<TModel> form, string heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(form, heading.ToHtml(), false, leadingHtml, htmlAttributes);
        }

        public static Section<TModel> BeginSection<TModel>(this IForm<TModel> form, IHtmlContent heading, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(form, heading ?? new HtmlString(""), false, leadingHtml, htmlAttributes);
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
        public static Section<TModel> BeginSection<TModel>(this Section<TModel> section, string heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(section.Form, heading.ToHtml(), true, leadingHtml, htmlAttributes);
        }

        public static Section<TModel> BeginSection<TModel>(this Section<TModel> section, IHtmlContent heading, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(section.Form, heading, true, leadingHtml, htmlAttributes);
        }

        /// <summary>
        /// Renders the given partial in the context of the parent model.
        /// </summary>
        /// <typeparam name="TModel">The form model type</typeparam>
        /// <param name="section">The current section</param>
        /// <param name="partialViewName">The name of the partial view to render</param>
        /// <returns>The HTML for the rendered partial</returns>
        public static IHtmlContent Partial<TModel>(this ISection<TModel> section, [AspMvcPartialView] string partialViewName)
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
        public static IHtmlContent PartialFor<TModel, TPartialModel>(this ISection<TModel> section, Expression<Func<TModel, TPartialModel>> partialModelProperty, [AspMvcPartialView] string partialViewName)
        {
            var formModel = (TModel)section.Form.HtmlHelper.ViewData.Model;
            var expressionText = ExpressionHelper.GetExpressionText(partialModelProperty);

            var viewData = new ViewDataDictionary(section.Form.HtmlHelper.ViewData);
            viewData[WebViewPageExtensions.PartialViewModelExpressionViewDataKey] = partialModelProperty;
            viewData[WebViewPageExtensions.CurrentFormViewDataKey] = section.Form;
            viewData[WebViewPageExtensions.CurrentFormSectionViewDataKey] = section;
            viewData.TemplateInfo.HtmlFieldPrefix = section.Form.HtmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
            return section.Form.HtmlHelper.Partial(partialViewName, partialModelProperty.Compile().Invoke(formModel), viewData);
        }
    }
}
