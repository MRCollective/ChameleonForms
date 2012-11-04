using System;
using System.Linq.Expressions;
using System.Web;
using ChameleonForms.FieldGenerator;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    /// <summary>
    /// Wraps the output of a single form field.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
    public class Field<TModel, TTemplate> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        private readonly IFieldGenerator _fieldGenerator;
        private bool IsParent { get { return !IsSelfClosing; } }

        /// <summary>
        /// Creates a form field.
        /// </summary>
        /// <param name="form">The form the field is being created in</param>
        /// <param name="isParent">Whether or not the field has other fields nested within it</param>
        /// <param name="fieldGenerator">A field HTML generator class</param>
        public Field(IForm<TModel, TTemplate> form, bool isParent, IFieldGenerator fieldGenerator)
            : base(form, !isParent)
        {
            _fieldGenerator = fieldGenerator;
            Initialise();
        }
        
        public override IHtmlString Begin()
        {
            return !IsParent
                ? Form.Template.Field(_fieldGenerator.GetLabelHtml(), _fieldGenerator.GetFieldHtml(), _fieldGenerator.GetValidationHtml(), _fieldGenerator.Metadata)
                : Form.Template.BeginField(_fieldGenerator.GetLabelHtml(), _fieldGenerator.GetFieldHtml(), _fieldGenerator.GetValidationHtml(), _fieldGenerator.Metadata);
        }

        public override IHtmlString End()
        {
            return !IsParent
                ? new HtmlString(string.Empty)
                : Form.Template.EndField();
        }
    }

    /// <summary>
    /// Extension methods for the creation of form fields.
    /// </summary>
    public static class FieldExtensions
    {
        /// <summary>
        /// Creates a single form field as a child of a form section.
        /// </summary>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="section">The section the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <returns>The form field</returns>
        public static Field<TModel, TTemplate> FieldFor<TModel, TTemplate, T>(this Section<TModel, TTemplate> section, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            return new Field<TModel, TTemplate>(section.Form, false, new DefaultFieldGenerator<TModel, T>(section.Form.HtmlHelper, property));
        }

        /// <summary>
        /// Creates a single form field as a child of a form section that can have other form fields nested within it.
        /// </summary>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="section">The section the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <returns></returns>
        public static Field<TModel, TTemplate> BeginFieldFor<TModel, TTemplate, T>(this Section<TModel, TTemplate> section, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            return new Field<TModel, TTemplate>(section.Form, true, new DefaultFieldGenerator<TModel, T>(section.Form.HtmlHelper, property));
        }

        /// <summary>
        /// Creates a single form field as a child of another form field.
        /// </summary>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="field">The parent field the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <returns></returns>
        public static Field<TModel, TTemplate> FieldFor<TModel, TTemplate, T>(this Field<TModel, TTemplate> field, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            return new Field<TModel, TTemplate>(field.Form, false, new DefaultFieldGenerator<TModel, T>(field.Form.HtmlHelper, property));
        }
    }
}