using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChameleonForms.Component.Config;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    public interface ISection<TModel> : IFormComponent<TModel>, IDisposable
    {
        /// <summary>
        /// Returns a section with the same characteristics as the current section, but using the given partial form.
        /// </summary>
        /// <typeparam name="TPartialModel">The model type of the partial view</typeparam>
        /// <param name="partialModelForm">The <see cref="Form{TModel}"/> from the partial view</param>
        /// <returns>A section with the same characteristics as the current section, but using the given partial form</returns>
        ISection<TPartialModel> CreatePartialSection<TPartialModel>(IForm<TPartialModel> partialModelForm);

        /// <summary>
        /// Returns a section with the same characteristics as the current section, but using the given partial form.
        /// </summary>
        /// <param name="partialHelper">The HTML helper from the partial view</param>
        /// <returns>A section with the same characteristics as the current section, but using the given partial form</returns>
        ISection<TModel> CreatePartialSection(IHtmlHelper<TModel> partialHelper);
    }

    /// <summary>
    /// Wraps the output of a form section.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    public class Section<TModel> : FormComponent<TModel>, ISection, ISection<TModel>
    {
        private readonly IHtmlContent _heading;
        private readonly bool _nested;
        private readonly object _parentSection;
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
            if (nested)
                _parentSection = form.HtmlHelper.ViewData[Constants.ViewDataSectionKey];
            form.HtmlHelper.ViewData[Constants.ViewDataSectionKey] = this;
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

        /// <inheritdoc />
        public ISection<TModel> CreatePartialSection(IHtmlHelper<TModel> partialHelper)
        {
            return new PartialViewSection<TModel>(Form.CreatePartialForm(partialHelper));
        }

        /// <summary>
        /// Called when form section is created within a `using` block: writes the end tag(s) of the section.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (_nested && _parentSection != null)
                Form.HtmlHelper.ViewData[Constants.ViewDataSectionKey] = _parentSection;
            else
                Form.HtmlHelper.ViewData.Remove(Constants.ViewDataSectionKey);
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

        /// <summary>
        /// Creates a top-level form section.
        /// </summary>
        /// <example>
        /// @using (var s = f.BeginSection("Section heading", leadingHtml: @&lt;p&gt;Leading html...&lt;/p&gt;)) {
        ///     @s.FieldFor(m => m.FirstName)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <param name="form">The form the section is being created in</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section as a templated razor delegate</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        /// <returns>The form section</returns>
        public static Section<TModel> BeginSection<TModel>(this IForm<TModel> form, string heading, Func<dynamic, IHtmlContent> leadingHtml, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(form, heading.ToHtml(), false, leadingHtml(null), htmlAttributes);
        }

        /// <summary>
        /// Creates a top-level form section.
        /// </summary>
        /// <example>
        /// @using (var s = f.BeginSection(new HtmlString("&lt;strong&gt;Section heading&lt;/strong&gt;"))) {
        ///     @s.FieldFor(m => m.FirstName)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <param name="form">The form the section is being created in</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        /// <returns>The form section</returns>
        public static Section<TModel> BeginSection<TModel>(this IForm<TModel> form, IHtmlContent heading, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(form, heading ?? new HtmlString(""), false, leadingHtml, htmlAttributes);
        }

        /// <summary>
        /// Creates a top-level form section.
        /// </summary>
        /// <example>
        /// @using (var s = f.BeginSection(@&lt;strong&gt;Section heading&lt;/strong&gt;, leadingHtml: @&lt;p&gt;Leading html...&lt;/p&gt;)) {
        ///     @s.FieldFor(m => m.FirstName)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <param name="form">The form the section is being created in</param>
        /// <param name="heading">The heading for the section as a templated razor delegate</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section as a templated razor delegate</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        /// <returns>The form section</returns>
        public static Section<TModel> BeginSection<TModel>(this IForm<TModel> form, Func<dynamic, IHtmlContent> heading, Func<dynamic, IHtmlContent> leadingHtml, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(form, heading(null), false, leadingHtml(null), htmlAttributes);
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
        public static Section<TModel> BeginSection<TModel>(this ISection<TModel> section, string heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(section.Form, heading.ToHtml(), true, leadingHtml, htmlAttributes);
        }

        /// <summary>
        /// Creates a nested form section.
        /// </summary>
        /// <example>
        /// @using (var s = f.BeginSection("Section heading")) {
        ///     using (var ss = s.BeginSection("Nested section heading", leadingHtml: @&lt;p&gt;Leading html...&lt;/p&gt;)) {
        ///         @ss.FieldFor(m => m.FirstName)
        ///     }
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <param name="section">The section the section is being created under</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section as a templated razor delegate</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        /// <returns>The nested form section</returns>
        public static Section<TModel> BeginSection<TModel>(this ISection<TModel> section, string heading, Func<dynamic, IHtmlContent> leadingHtml, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(section.Form, heading.ToHtml(), true, leadingHtml(null), htmlAttributes);
        }

        /// <summary>
        /// Creates a nested form section.
        /// </summary>
        /// <example>
        /// @using (var s = f.BeginSection("Section heading")) {
        ///     using (var ss = s.BeginSection(new HtmlString("&lt;strong&gt;Nested section heading&lt;/strong&gt;"))) {
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
        public static Section<TModel> BeginSection<TModel>(this ISection<TModel> section, IHtmlContent heading, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(section.Form, heading, true, leadingHtml, htmlAttributes);
        }

        /// <summary>
        /// Creates a nested form section.
        /// </summary>
        /// <example>
        /// @using (var s = f.BeginSection("Section heading")) {
        ///     using (var ss = s.BeginSection(@&lt;strong&gt;Nested section heading&lt;/strong&gt;, leadingHtml: &lt;p&gt;Leading html...&lt;/p&gt;)) {
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
        public static Section<TModel> BeginSection<TModel>(this ISection<TModel> section, Func<dynamic, IHtmlContent> heading, Func<dynamic, IHtmlContent> leadingHtml, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(section.Form, heading(null), true, leadingHtml(null), htmlAttributes);
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
        public static Task<IHtmlContent> PartialForAsync<TModel, TPartialModel>(this ISection<TModel> section, Expression<Func<TModel, TPartialModel>> partialModelProperty, [AspMvcPartialView] string partialViewName)
        {
            var formModel = section.Form.HtmlHelper.ViewData.Model;

            using (var h = section.Form.HtmlHelper.For(partialModelProperty, bindFieldsToParent: true))
            {
                using (var f = section.Form.CreatePartialForm(partialModelProperty, h))
                {
                    using (section.CreatePartialSection(f))
                    {
                        return h.PartialAsync(partialViewName, partialModelProperty.Compile().Invoke(formModel), h.ViewData);
                    }
                }
            }
        }
    }
}
