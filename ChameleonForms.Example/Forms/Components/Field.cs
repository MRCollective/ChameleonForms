using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ChameleonForms.Example.Forms.Templates;

namespace ChameleonForms.Example.Forms.Components
{
    public class Field<TModel, TTemplate, T> : IFormComponent<TModel, TTemplate>, IHtmlString where TTemplate : IFormTemplate, new()
    {
        private readonly Expression<Func<TModel, T>> _property;
        public ChameleonForm<TModel, TTemplate> Form { get; private set; }

        public Field(ChameleonForm<TModel, TTemplate> form, Expression<Func<TModel, T>> property)
        {
            _property = property;
            Form = form;
        }

        // todo: Abstract this away so the template can be used in place of HtmlHelper?
        public IHtmlString GetFieldHtml()
        {
            //var metadata = ModelMetadata.FromLambdaExpression(_property, Form.HtmlHelper.ViewData);
            return Form.HtmlHelper.TextBoxFor(_property);
        }

        public IHtmlString GetLabelHtml()
        {
            return Form.HtmlHelper.LabelFor(_property);
        }

        public string ToHtmlString()
        {
            return Form.Template.Field(GetLabelHtml(), GetFieldHtml()).ToHtmlString();
        }
    }

    public static class FieldExtensions
    {
        public static Field<TModel, TTemplate, T> FieldFor<TModel, TTemplate, T>(this Section<TModel, TTemplate> section, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate, new()
        {
            return new Field<TModel, TTemplate, T>(section.Form, property);
        }
    }
}