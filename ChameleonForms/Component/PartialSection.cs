using ChameleonForms.Component.Config;
using ChameleonForms.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc.Html;

namespace ChameleonForms.Component
{
    class PartialSection<TModel, TChild, TTemplate> : ISection<TChild> where TTemplate : IFormTemplate
    {
        private Section<TModel, TTemplate> section;
        private Expression<Func<TModel, TChild>> parEx;

        public PartialSection(Section<TModel, TTemplate> section, Expression<Func<TModel, TChild>> parEx)
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
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="TValue">The view model type for the nested view</typeparam>
        /// <param name="section">Form section</param>
        /// <param name="expression">A lamdba expression to identify the field to render the field for</param>
        /// <param name="templateName">The name of the template to use to render the object.</param>
        /// <returns>Rendered view</returns>
        public static IHtmlString PartialFor<TModel, TTemplate, TValue>(this Section<TModel, TTemplate> section, Expression<Func<TModel, TValue>> expression, string templateName) where TTemplate : IFormTemplate
        {
            object newViewData = new { ChameleonSection = section, ChameleonExpression = expression };
            return section.Form.HtmlHelper.EditorFor(expression, templateName, null, newViewData);
        }

        /// <summary>
        /// Extension injects parent section into editor view
        /// </summary>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="TTemplate">The type of HTML template renderer the form is using</typeparam>
        /// <typeparam name="TValue">The view model type for the nested view</typeparam>
        /// <param name="section">Form section</param>
        /// <param name="expression">A lamdba expression to identify the field to render the field for</param>
        /// <returns>Rendered view</returns>
        public static IHtmlString PartialFor<TModel, TTemplate, TValue>(this Section<TModel, TTemplate> section, Expression<Func<TModel, TValue>> expression) where TTemplate : IFormTemplate
        {
            object newViewData = new { ChameleonSection = section, ChameleonExpression = expression };
            return section.Form.HtmlHelper.EditorFor(expression, null, null, newViewData);
        }
    }
}
