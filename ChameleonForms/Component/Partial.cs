using ChameleonForms.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ChameleonForms.Component
{
    /// <summary>
    /// Extensions to add partial for for sections
    /// </summary>
    public static class PartialExtensions
    {
        internal class ObjectViewData : ViewDataDictionary
        {
            public ObjectViewData()
            {
            }

            public object ChameleonSection
            {
                get
                {
                    return this[ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonSection)];
                }
                set
                {
                    this[ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonSection)] = value;
                }
            }

            public object ChameleonForm
            {
                get
                {
                    return this[ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonForm)];
                }
                set
                {
                    this[ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonForm)] = value;
                }
            }

            public object ChameleonExpression
            {
                get
                {
                    return this[ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonExpression)];
                }
                set
                {
                    this[ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonExpression)] = value;
                }
            }

            public int Index
            {
                get
                {
                    return (int)this[ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.Index)];
                }
                set
                {
                    this[ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.Index)] = value;
                }
            }
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
            var list = expression.Compile()(section.Form.HtmlHelper.ViewData.Model);
            StringBuilder bld = new StringBuilder();
            for (var i = 0; i < list.Count; ++i)
            {
                var i1 = i;
                Expression<Func<IList<TValue>, TValue>> ex1 = x => x[i1];
                var ex2 = ExpressionExtensions.Combine(expression, ex1);

                ViewDataDictionary newViewData = new ObjectViewData { ChameleonSection = section, ChameleonForm = section.Form, ChameleonExpression = ex2, Index = i1 };
                bld.AppendLine(section.Form.HtmlHelper.Partial(templateName, list[i], newViewData).ToString());
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
            var list = expression.Compile()(form.HtmlHelper.ViewData.Model);
            StringBuilder bld = new StringBuilder();
            for (var i = 0; i < list.Count; ++i)
            {
                var i1 = i;
                Expression<Func<IList<TValue>, TValue>> ex1 = x => x[i1];
                var ex2 = ExpressionExtensions.Combine(expression, ex1);

                ObjectViewData newViewData = new ObjectViewData { ChameleonForm = form, ChameleonExpression = ex2, Index = i1 };
                bld.AppendLine(form.HtmlHelper.Partial(templateName, list[i], newViewData).ToString());
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
        ///
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static IHtmlString PartialFor<TModel, TValue>(this IForm<TModel> form, Expression<Func<TModel, TValue>> expression, string templateName)
        {
            ObjectViewData newViewData = new ObjectViewData { ChameleonForm = form, ChameleonExpression = expression };
            return form.HtmlHelper.Partial(templateName, expression.Compile()(form.HtmlHelper.ViewData.Model), newViewData);
        }

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
            ObjectViewData newViewData = new ObjectViewData { ChameleonSection = section, ChameleonExpression = expression, ChameleonForm = section.Form };
            return section.Form.HtmlHelper.Partial(templateName, expression.Compile()(section.Form.HtmlHelper.ViewData.Model), newViewData);
        }
    }
}