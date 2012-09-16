using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc.Html;
using ChameleonForms.Templates;

namespace ChameleonForms.Component
{
    public class Field<TModel, TTemplate, T> : FormComponent<TModel, TTemplate> where TTemplate : IFormTemplate
    {
        private readonly Expression<Func<TModel, T>> _property;

        public Field(IForm<TModel, TTemplate> form, bool isSelfClosing, Expression<Func<TModel, T>> property)
            : base(form, isSelfClosing)
        {
            _property = property;
        }
        
        public virtual IHtmlString GetFieldHtml()
        {
            //var metadata = ModelMetadata.FromLambdaExpression(_property, Form.HtmlHelper.ViewData);
            return Form.HtmlHelper.TextBoxFor(_property);
        }

        public virtual IHtmlString GetLabelHtml()
        {
            return Form.HtmlHelper.LabelFor(_property);
        }

        public virtual IHtmlString GetValidationHtml()
        {
            return Form.HtmlHelper.ValidationMessageFor(_property);
        }

        public override IHtmlString Begin()
        {
            return IsSelfClosing ? Form.Template.Field(GetLabelHtml(), GetFieldHtml(), GetValidationHtml()) : Form.Template.BeginField(GetLabelHtml(), GetFieldHtml(), GetValidationHtml());
        }

        public override IHtmlString End()
        {
            return IsSelfClosing ? new HtmlString(string.Empty) : Form.Template.EndField();
        }
    }

    public static class FieldExtensions
    {
        public static Field<TModel, TTemplate, T> FieldFor<TModel, TTemplate, T>(this Section<TModel, TTemplate> section, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            return new Field<TModel, TTemplate, T>(section.Form, true, property);
        }

        public static Field<TModel, TTemplate, T> BeginFieldFor<TModel, TTemplate, T>(this Section<TModel, TTemplate> section, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            return new Field<TModel, TTemplate, T>(section.Form, false, property);
        }
    }
}