using ChameleonForms.Templates;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;

namespace ChameleonForms.Config
{
    /// <summary>
    /// Provides a fluent builder syntax to configure Chameleon Forms.
    /// </summary>
    /// <typeparam name="TFormTemplate">The template type to use</typeparam>
    public class ChameleonFormsConfigBuilder<TFormTemplate> where TFormTemplate : IFormTemplate
    {
        private readonly ChameleonFormsConfig<TFormTemplate> _config = new ChameleonFormsConfig<TFormTemplate>();

        /// <summary>
        /// Turn off humanized labels.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutHumanizedLabels()
        {
            _config.HumanizeLabels = false;
            return this;
        }

        /// <summary>
        /// Humanize labels with the given transformer. Use <see cref="To"/> to access the default Humanizer ones.
        /// </summary>
        /// <example>
        /// builder.WithHumanizedLabelTransformer(To.TitleCase)
        /// </example>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithHumanizedLabelTransformer(IStringTransformer transformer)
        {
            _config.HumanizedLabelsTransformer = transformer;
            return this;
        }

        /// <summary>
        /// Don't register the template type with the <see cref="ServiceCollection"/>.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutTemplateTypeRegistration()
        {
            _config.RegisterTemplateType = false;
            return this;
        }

        /// <summary>
        /// Turn off model binding of flag enums.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutFlagsEnumBinding()
        {
            _config.RegisterFlagsEnumBinding = false;
            return this;
        }

        /// <summary>
        /// Turn off validation of [Required] on flag enums.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutFlagsEnumRequiredValidation()
        {
            _config.RegisterFlagsEnumRequiredValidation = false;
            return this;
        }

        /// <summary>
        /// Turn off model binding of <see cref="System.DateTime"/>s.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutDateTimeBinding()
        {
            _config.RegisterDateTimeBinding = false;
            return this;
        }

        /// <summary>
        /// Turn off model binding of enum lists.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutEnumListBinding()
        {
            _config.RegisterEnumListBinding = false;
            return this;
        }

        /// <summary>
        /// Turn off model binding of <see cref="System.Uri"/>s.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutUriBinding()
        {
            _config.RegisterUriBinding = false;
            return this;
        }

        /// <summary>
        /// Turn off client model validation of integral numerics.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutIntegralClientModelValidation()
        {
            _config.RegisterIntegralClientModelValidator = false;
            return this;
        }

        /// <summary>
        /// Turn off client model validation of <see cref="System.DateTime"/>s.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutDateTimeClientModelValidation()
        {
            _config.RegisterDateTimeClientModelValidator = false;
            return this;
        }

        /// <summary>
        /// Returns a built <see cref="ChameleonFormsConfig{TFormTemplate}"/>.
        /// </summary>
        /// <returns>The built config</returns>
        internal ChameleonFormsConfig<TFormTemplate> Build()
        {
            return _config;
        }
    }

    internal class ChameleonFormsConfig<TFormTemplate> where TFormTemplate : IFormTemplate
    {
        public bool HumanizeLabels { get; set; } = true;
        public IStringTransformer HumanizedLabelsTransformer { get; set; } = null;
        public bool RegisterTemplateType { get; set; } = true;
        public bool RegisterFlagsEnumBinding { get; set; } = true;
        public bool RegisterFlagsEnumRequiredValidation { get; set; } = true;
        public bool RegisterDateTimeBinding { get; set; } = true;
        public bool RegisterEnumListBinding { get; set; } = true;
        public bool RegisterUriBinding { get; set; } = true;
        public bool RegisterIntegralClientModelValidator { get; set; } = true;
        public bool RegisterDateTimeClientModelValidator { get; set; } = true;
    }
}
