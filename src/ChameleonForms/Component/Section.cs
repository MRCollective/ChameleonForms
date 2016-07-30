using ChameleonForms.Component.Config;

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
        private readonly IHtml _heading;
        private readonly bool _nested;
        private readonly IHtml _leadingHtml;
        private readonly HtmlAttributes _htmlAttributes;

        /// <summary>
        /// Creates a form section
        /// </summary>
        /// <param name="form">The form the message is being created in</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="nested">Whether the section is nested within another section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        public Section(IForm<TModel> form, IHtml heading, bool nested, IHtml leadingHtml = null, HtmlAttributes htmlAttributes = null) : base(form, false)
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
        public IFieldConfiguration Field(IHtml labelHtml, IHtml elementHtml, IHtml validationHtml = null, IFieldMetadata metadata = null, bool isValid = true)
        {
            var fc = new FieldConfiguration();
            fc.SetField(() => Form.Template.Field(labelHtml, elementHtml, validationHtml, metadata, fc, isValid));
            return fc;
        }

        /// <inheritdoc />
        public override IHtml Begin()
        {
            return _nested ? Form.Template.BeginNestedSection(_heading, _leadingHtml, _htmlAttributes) : Form.Template.BeginSection(_heading, _leadingHtml, _htmlAttributes);
        }

        /// <inheritdoc />
        public override IHtml End()
        {
            return _nested ? Form.Template.EndNestedSection() : Form.Template.EndSection();
        }

        /// <inheritdoc />
        public ISection<TPartialModel> CreatePartialSection<TPartialModel>(IForm<TPartialModel> partialModelForm)
        {
            return new PartialViewSection<TPartialModel>(partialModelForm);
        }
    }
}
