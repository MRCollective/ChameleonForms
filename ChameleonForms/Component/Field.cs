using System;
using System.Linq.Expressions;
using System.Web;
using ChameleonForms.FieldGenerator;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    public class Field<TModel, TTemplate> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        private readonly IFieldGenerator _fieldGenerator;
        private bool IsParent { get { return !IsSelfClosing; } }

        public Field(IForm<TModel, TTemplate> form, bool isParent, IFieldGenerator fieldGenerator)
            : base(form, !isParent)
        {
            _fieldGenerator = fieldGenerator;
            Initialise();
        }
        
        public override IHtmlString Begin()
        {
            return !IsParent
                ? Form.Template.Field(_fieldGenerator.GetLabelHtml(), _fieldGenerator.GetFieldHtml(), _fieldGenerator.GetValidationHtml())
                : Form.Template.BeginField(_fieldGenerator.GetLabelHtml(), _fieldGenerator.GetFieldHtml(), _fieldGenerator.GetValidationHtml());
        }

        public override IHtmlString End()
        {
            return !IsParent
                ? new HtmlString(string.Empty)
                : Form.Template.EndField();
        }
    }

    public static class FieldExtensions
    {
        public static Field<TModel, TTemplate> FieldFor<TModel, TTemplate, T>(this Section<TModel, TTemplate> section, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            return new Field<TModel, TTemplate>(section.Form, false, new DefaultFieldGenerator<TModel, T>(section.Form.HtmlHelper, property));
        }

        public static Field<TModel, TTemplate> BeginFieldFor<TModel, TTemplate, T>(this Section<TModel, TTemplate> section, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            return new Field<TModel, TTemplate>(section.Form, true, new DefaultFieldGenerator<TModel, T>(section.Form.HtmlHelper, property));
        }

        public static Field<TModel, TTemplate> FieldFor<TModel, TTemplate, T>(this Field<TModel, TTemplate> field, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            return new Field<TModel, TTemplate>(field.Form, false, new DefaultFieldGenerator<TModel, T>(field.Form.HtmlHelper, property));
        }
    }
}