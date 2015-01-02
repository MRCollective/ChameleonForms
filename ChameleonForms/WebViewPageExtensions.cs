using ChameleonForms;
using ChameleonForms.Component;
using ChameleonForms.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Web.Mvc
{
    /// <summary>
    /// Extension to use in nested partial view.
    /// </summary>
    public static class WebViewPageExtensions
    {
        /// <summary>
        /// Get section from upper view
        /// </summary>
        /// <typeparam name="TModel">View model of nested partial view</typeparam>
        /// <param name="self">View page for nested view</param>
        /// <returns>Returns section of parent view</returns>
        public static ISection<TModel> ChameleonSection<TModel>(this WebViewPage<TModel> self)
        {
            object parentSectionObject;
            if (!self.ViewData.TryGetValue("ChameleonSection", out parentSectionObject))
            {
                throw new InvalidOperationException("Chameleon Section is unavailable for now");
            }

            object parentExpression;
            if (!self.ViewData.TryGetValue("ChameleonExpression", out parentExpression))
            {
                throw new InvalidOperationException("Chameleon Section is unavailable for now");
            }
            
            ISection parentSection = parentSectionObject as ISection;
            return parentSection.CreateChildSection<TModel>(parentExpression);
        }
    }
}
