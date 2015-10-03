using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component.Config;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Utils;

namespace ChameleonForms.Component.Partial
{
    internal class PartialProxySection<TModel, TChild> : ISection<TChild>, ISection
    {
        private readonly ISection<TModel> section;
        private readonly Expression<Func<TModel, TChild>> parEx;
        private readonly IForm<TChild> form;

        public PartialProxySection(ISection<TModel> section, Expression<Func<TModel, TChild>> parEx)
        {
            this.section = section;
            this.parEx = parEx;
            this.form = new PartialProxyForm<TModel, TChild>(this.section.Form, parEx, null);
        }

        public IFieldConfiguration Field(IHtmlString labelHtml
            , IHtmlString elementHtml
            , IHtmlString validationHtml = null
            , ModelMetadata metadata = null, bool isValid = true)
        {
            return this.section.Field(labelHtml, elementHtml, validationHtml, metadata, isValid);
        }

        public IHtmlString Begin()
        {
            return this.section.Begin();
        }

        public IHtmlString End()
        {
            return this.section.End();
        }

        public IForm<TChild> Form
        {
            get { return this.form; }
        }

        public void Initialise()
        {
            this.section.Initialise();
        }

        public string ToHtmlString()
        {
            return this.section.ToHtmlString();
        }

        public void Dispose()
        {
            this.section.Dispose();
        }

        public IFieldGenerator GetFieldGenerator<TProperty>(Expression<Func<TChild, TProperty>> property)
        {
            return this.section.GetFieldGenerator(ExpressionExtensions.Combine(parEx, property));
        }

        public ISection<TProperty> CreateChildSection<TProperty>(object parentExpression)
        {
            var express = parentExpression as Expression<Func<TChild, TProperty>>;
            if (express == null)
            {
                throw new ArgumentNullException("parentExpression");
            }

            return new PartialProxySection<TChild, TProperty>(this, express);
        }
    }
}
