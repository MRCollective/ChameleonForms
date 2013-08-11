using System;
using System.Linq.Expressions;
using System.Web;
using ChameleonForms.Component.Config;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    /// <summary>
    /// Helper for field configuration.
    /// </summary>
    public static class Field
    {
        /// <summary>
        /// Returns a field configuration object.
        /// </summary>
        /// <returns>A field configuration</returns>
        public static FieldConfiguration Configure()
        {
            return new FieldConfiguration();
        }
    }

    /// <summary>
    /// Wraps the output of a single form field.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
    public class Field<TModel, TTemplate> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        private readonly IFieldGenerator _fieldGenerator;
        private readonly IFieldConfiguration _config;
        private bool IsParent { get { return !IsSelfClosing; } }
        
        /// <summary>
        /// Creates a form field.
        /// </summary>
        /// <param name="form">The form the field is being created in</param>
        /// <param name="isParent">Whether or not the field has other fields nested within it</param>
        /// <param name="fieldGenerator">A field HTML generator class</param>
        /// <param name="config"> </param>
        public Field(IForm<TModel, TTemplate> form, bool isParent, IFieldGenerator fieldGenerator, IFieldConfiguration config)
            : base(form, !isParent)
        {
            _fieldGenerator = fieldGenerator;
            _config = config;
            if (_config != null)
                _config.SetField(this);
            Initialise();
        }
        
        public override IHtmlString Begin()
        {
            var isValid = Form.HtmlHelper.ViewData.ModelState.IsValidField(_fieldGenerator.GetFieldId());
            var readonlyConfig = _fieldGenerator.PrepareFieldConfiguration(_config);
            return !IsParent
                ? Form.Template.Field(_fieldGenerator.GetLabelHtml(readonlyConfig), _fieldGenerator.GetFieldHtml(readonlyConfig), _fieldGenerator.GetValidationHtml(readonlyConfig), _fieldGenerator.Metadata, _config, isValid)
                : Form.Template.BeginField(_fieldGenerator.GetLabelHtml(readonlyConfig), _fieldGenerator.GetFieldHtml(readonlyConfig), _fieldGenerator.GetValidationHtml(readonlyConfig), _fieldGenerator.Metadata, _config, isValid);
        }

        public override IHtmlString End()
        {
            return !IsParent
                ? new HtmlString(string.Empty)
                : Form.Template.EndField();
        }
    }

    /// <summary>
    /// Extension methods for the creation of form fields, labels and validation messages.
    /// </summary>
    public static class FieldExtensions
    {
        /// <summary>
        /// Creates a single form field as a child of a form section.
        /// </summary>
        /// <example>
        /// @s.FieldFor(m => m.FirstName)
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="section">The section the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <returns>A field configuration object that allows you to configure the field</returns>
        public static IFieldConfiguration FieldFor<TModel, TTemplate, T>(this Section<TModel, TTemplate> section, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            var fc = new FieldConfiguration();
            // ReSharper disable ObjectCreationAsStatement
            new Field<TModel, TTemplate>(section.Form, false, section.Form.GetFieldGenerator(property), fc);
            // ReSharper restore ObjectCreationAsStatement
            return fc;
        }

        /// <summary>
        /// Creates a single form field as a child of a form section that can have other form fields nested within it.
        /// </summary>
        /// <example>
        /// @using (var f = s.BeginFieldFor(m => m.Company)) {
        ///     @f.FieldFor(m => m.PositionTitle)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="section">The section the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <param name="config">Any configuration information for the field</param>
        /// <returns></returns>
        public static Field<TModel, TTemplate> BeginFieldFor<TModel, TTemplate, T>(this Section<TModel, TTemplate> section, Expression<Func<TModel, T>> property, IFieldConfiguration config = null) where TTemplate : IFormTemplate
        {
            return new Field<TModel, TTemplate>(section.Form, true, section.Form.GetFieldGenerator(property), config);
        }

        /// <summary>
        /// Creates a single form field as a child of another form field.
        /// </summary>
        /// <example>
        /// @using (var f = s.BeginFieldFor(m => m.Company)) {
        ///     @f.FieldFor(m => m.PositionTitle)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="field">The parent field the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <returns>A field configuration object that allows you to configure the field</returns>
        public static IFieldConfiguration FieldFor<TModel, TTemplate, T>(this Field<TModel, TTemplate> field, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            var config = new FieldConfiguration();
            // ReSharper disable ObjectCreationAsStatement
            new Field<TModel, TTemplate>(field.Form, false, field.Form.GetFieldGenerator(property), config);
            // ReSharper restore ObjectCreationAsStatement
            return config;
        }

        /// <summary>
        /// Creates a standalone form field to be output in a form.
        /// </summary>
        /// <example>
        /// @using (var f = s.BeginChameleonForm()) {
        ///     <p>@f.LabelFor(m => m.PositionTitle) @f.FieldFor(m => m.PositionTitle)</p>
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="form">The form the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <returns>A field configuration object that allows you to configure the field</returns>
        public static IFieldConfiguration FieldFor<TModel, TTemplate, T>(this IForm<TModel, TTemplate> form, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            var config = new FieldConfiguration();
            config.SetField(() => form.GetFieldGenerator(property).GetFieldHtml(config));
            return config;
        }

        /// <summary>
        /// Creates a standalone label to be output in a form for a field.
        /// </summary>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="form">The form the label is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the label for</param>
        /// <returns>The HTML for the label</returns>
        public static IFieldConfiguration LabelFor<TModel, TTemplate, T>(this IForm<TModel, TTemplate> form, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            var config = new FieldConfiguration();
            config.SetField(() => form.GetFieldGenerator(property).GetLabelHtml(config));
            return config;
        }

        /// <summary>
        /// Creates a standalone validation message to be output in a form for a field.
        /// </summary>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="form">The form the label is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the validation message for</param>
        /// <returns>The HTML for the validation message</returns>
        public static IFieldConfiguration ValidationMessageFor<TModel, TTemplate, T>(this IForm<TModel, TTemplate> form, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            var config = new FieldConfiguration();
            config.SetField(() => form.GetFieldGenerator(property).GetValidationHtml(config));
            return config;
        }
    }
}