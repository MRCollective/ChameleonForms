using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc.Html;
using ChameleonForms.Component;
using ChameleonForms.Templates;

namespace ChameleonForms.Example.Forms.Components
{
    public class Field<TModel, TTemplate, T> : IFormComponent<TModel, TTemplate>, IHtmlString where TTemplate : IFormTemplate
    {
        private readonly Expression<Func<TModel, T>> _property;
        public IForm<TModel, TTemplate> Form { get; private set; }

        public Field(IForm<TModel, TTemplate> form, Expression<Func<TModel, T>> property)
        {
            _property = property;
            Form = form;
        }

        // todo: Abstract this away so the template can be used in place of HtmlHelper?
        //  or possibly instead I could just let someone extend this class and provide a different extension method?
        // Let's make them virtual for now :)
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

        public string ToHtmlString()
        {
            return Form.Template.Field(GetLabelHtml(), GetFieldHtml(), GetValidationHtml()).ToHtmlString();
        }
    }

    public static class FieldExtensions
    {
        public static Field<TModel, TTemplate, T> FieldFor<TModel, TTemplate, T>(this Section<TModel, TTemplate> section, Expression<Func<TModel, T>> property) where TTemplate : IFormTemplate
        {
            return new Field<TModel, TTemplate, T>(section.Form, property);
        }
    }
}