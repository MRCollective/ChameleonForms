using ChameleonForms.Component.Config;
using ChameleonForms.FieldGenerators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ChameleonForms.Component
{
    class PartialSection<TModel, TChild> : ISection<TChild>, ISection
    {
        private readonly ISection<TModel> section;
        private readonly Expression<Func<TModel, TChild>> parEx;
        private readonly IForm<TChild> form;

        public PartialSection(ISection<TModel> section, Expression<Func<TModel, TChild>> parEx)
        {
            this.section = section;
            this.parEx = parEx;
            this.form = new ProxyForm<TModel, TChild>(this.section.Form, parEx);
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

            return new PartialSection<TChild, TProperty>(this, express);
        }
    }

    /// <summary>
    /// Extensions to add partial for for sections
    /// </summary>
    public static class PartialExtensions
    {
        /// <summary>
        /// Extension injects parent section into editor view
        /// </summary>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TValue">The view model type for the nested view</typeparam>
        /// <param name="section">Form section</param>
        /// <param name="expression">A lamdba expression to identify the field to render the field for</param>
        /// <param name="templateName">The name of the template to use to render the object.</param>
        /// <returns>Rendered view</returns>
        public static IHtmlString PartialFor<TModel, TValue>(this ISection<TModel> section, Expression<Func<TModel, TValue>> expression, string templateName)
        {
            object newViewData = new { ChameleonSection = section, ChameleonExpression = expression };
            return section.Form.HtmlHelper.EditorFor(expression, templateName, null, newViewData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="section"></param>
        /// <param name="expression"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static IHtmlString PartialForList<TModel, TValue>(this ISection<TModel> section, Expression<Func<TModel, IList<TValue>>> expression, string templateName)
        {
            var count = expression.Compile()(section.Form.HtmlHelper.ViewData.Model).Count;
            StringBuilder bld = new StringBuilder();
            for (var i = 0; i < count; ++i)
            {
                var i1 = i;
                Expression<Func<IList<TValue>, TValue>> ex1 = x => x[i1];
                var ex2 = ExpressionExtensions.Combine(expression, ex1);

                object newViewData = new { ChameleonSection = section, ChameleonExpression = ex2 };
                bld.AppendLine(section.Form.HtmlHelper.EditorFor(ex2, templateName, null, newViewData).ToString());
            }

            return new MvcHtmlString(bld.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="form"></param>
        /// <param name="expression"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static IHtmlString PartialForList<TModel, TValue>(this IForm<TModel> form,
            Expression<Func<TModel, IList<TValue>>> expression, string templateName)
        {
            var count = expression.Compile()(form.HtmlHelper.ViewData.Model).Count;
            StringBuilder bld = new StringBuilder();
            for (var i = 0; i < count; ++i)
            {
                var i1 = i;
                Expression<Func<IList<TValue>, TValue>> ex1 = x => x[i1];
                var ex2 = ExpressionExtensions.Combine(expression, ex1);

                object newViewData = new { ChameleonForm = form, ChameleonExpression = ex2 };
                bld.AppendLine(form.HtmlHelper.EditorFor(ex2, templateName, null, newViewData).ToString());
            }

            return new MvcHtmlString(bld.ToString());
        }

        /// <summary>
        /// Extension injects parent section into editor view
        /// </summary>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TValue">The view model type for the nested view</typeparam>
        /// <param name="section">Form section</param>
        /// <param name="expression">A lamdba expression to identify the field to render the field for</param>
        /// <returns>Rendered view</returns>
        public static IHtmlString PartialFor<TModel, TValue>(this ISection<TModel> section, Expression<Func<TModel, TValue>> expression)
        {
            object newViewData = new { ChameleonSection = section, ChameleonExpression = expression };
            return section.Form.HtmlHelper.EditorFor(expression, null, null, newViewData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="form"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IHtmlString PartialFor<TModel, TValue>(this IForm<TModel> form, Expression<Func<TModel, TValue>> expression)
        {
            object newViewData = new { ChameleonForm = form, ChameleonExpression = expression };
            return form.HtmlHelper.EditorFor(expression, null, null, newViewData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="form"></param>
        /// <param name="expression"></param>
        /// 
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static IHtmlString PartialFor<TModel, TValue>(this IForm<TModel> form, Expression<Func<TModel, TValue>> expression, string templateName)
        {
            object newViewData = new { ChameleonForm = form, ChameleonExpression = expression };
            return form.HtmlHelper.EditorFor(expression, templateName, null, newViewData);
        }
    }
}
