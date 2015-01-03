using ChameleonForms.Component.Config;
using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc.Html;

namespace ChameleonForms.Component
{
    class PartialSection<TModel, TChild> : ISection<TChild>
    {
        private readonly Section<TModel> section;
        private readonly Expression<Func<TModel, TChild>> parEx;

        public PartialSection(Section<TModel> section, Expression<Func<TModel, TChild>> parEx)
        {
            this.section = section;
            this.parEx = parEx;
        }

        public IFieldConfiguration FieldFor<TProperty>(Expression<Func<TChild, TProperty>> expression)
        {
            return this.section.FieldFor(ExpressionExtensions.Combine(parEx, expression));
        }

        public IHtmlString PartialFor<TChild2>(Expression<Func<TChild, TChild2>> expression)
        {
            return this.section.PartialFor(ExpressionExtensions.Combine(parEx, expression));
        }

        public IHtmlString PartialFor<TChild2>(Expression<Func<TChild, TChild2>> expression, string templateName)
        {
            return this.section.PartialFor(ExpressionExtensions.Combine(parEx, expression), templateName);
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
        public static IHtmlString PartialFor<TModel, TValue>(this Section<TModel> section, Expression<Func<TModel, TValue>> expression, string templateName)
        {
            object newViewData = new { ChameleonSection = section, ChameleonExpression = expression };
            return section.Form.HtmlHelper.EditorFor(expression, templateName, null, newViewData);
        }

        /// <summary>
        /// Extension injects parent section into editor view
        /// </summary>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TValue">The view model type for the nested view</typeparam>
        /// <param name="section">Form section</param>
        /// <param name="expression">A lamdba expression to identify the field to render the field for</param>
        /// <returns>Rendered view</returns>
        public static IHtmlString PartialFor<TModel, TValue>(this Section<TModel> section, Expression<Func<TModel, TValue>> expression)
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
