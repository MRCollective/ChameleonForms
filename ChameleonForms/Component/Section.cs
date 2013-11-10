using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component.Config;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    /// <summary>
    /// Wraps the output of a form section.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
    public class Section<TModel, TTemplate> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        private readonly IHtmlString _heading;
        private readonly bool _nested;
        private readonly IHtmlString _leadingHtml;
        private readonly HtmlAttributes _htmlAttributes;

        /// <summary>
        /// Creates a form section
        /// </summary>
        /// <param name="form">The form the message is being created in</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="nested">Whether the section is nested within another section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        public Section(IForm<TModel, TTemplate> form, IHtmlString heading, bool nested, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null) : base(form, false)
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
        public IFieldConfiguration Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml = null, ModelMetadata metadata = null, bool isValid = true)
        {
            var fc = new FieldConfiguration();
            fc.SetField(() => Form.Template.Field(labelHtml, elementHtml, validationHtml, metadata, new ReadonlyFieldConfiguration(fc), isValid));
            return fc;
        }

        public override IHtmlString Begin()
        {
            return _nested ? Form.Template.BeginNestedSection(_heading, _leadingHtml, _htmlAttributes) : Form.Template.BeginSection(_heading, _leadingHtml, _htmlAttributes);
        }

        public override IHtmlString End()
        {
            return _nested ? Form.Template.EndNestedSection() : Form.Template.EndSection();
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
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <param name="form">The form the section is being created in</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        /// <returns>The form section</returns>
        public static Section<TModel, TTemplate> BeginSection<TModel, TTemplate>(this IForm<TModel, TTemplate> form, string heading = null, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null) where TTemplate : IFormTemplate
        {
            return new Section<TModel, TTemplate>(form, heading.ToHtml(), false, leadingHtml, htmlAttributes);
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
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <param name="section">The section the section is being created under</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        /// <returns>The nested form section</returns>
        public static Section<TModel, TTemplate> BeginSection<TModel, TTemplate>(this Section<TModel, TTemplate> section, string heading = null, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null) where TTemplate : IFormTemplate
        {
            return new Section<TModel, TTemplate>(section.Form, heading.ToHtml(), true, leadingHtml, htmlAttributes);
        }
    }
}
