using ChameleonForms;
using ChameleonForms.Component;
using ChameleonForms.Utils;

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
            if (!self.ViewData.TryGetValue(ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonSection), out parentSectionObject))
            {
                throw new InvalidOperationException("Chameleon Section is unavailable for now");
            }

            object parentExpression;
            if (!self.ViewData.TryGetValue(ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonExpression), out parentExpression))
            {
                throw new InvalidOperationException("Chameleon Section is unavailable for now");
            }

            var parentSection = parentSectionObject as ISection;
            if (parentSection == null)
            {
                throw new InvalidOperationException("Chameleon Section is unavailable for now");
            }

            return parentSection.CreateChildSection<TModel>(parentExpression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IForm<TModel> ChameleonForm<TModel>(this WebViewPage<TModel> self)
        {
            object parentFormObject;
            if (!self.ViewData.TryGetValue(ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonForm), out parentFormObject))
            {
                throw new InvalidOperationException("Chameleon Section is unavailable for now");
            }

            object parentExpression;
            if (!self.ViewData.TryGetValue(ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonExpression), out parentExpression))
            {
                throw new InvalidOperationException("Chameleon Section is unavailable for now");
            }

            var parentForm = parentFormObject as IForm;
            if (parentForm == null)
            {
                throw new InvalidOperationException("Chameleon Section is unavailable for now");
            }

            return parentForm.CreateChildProxy<TModel>(parentExpression, self.Html);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsChameleonForm<TModel>(this WebViewPage<TModel> self)
        {
            object parentFormObject;
            if (!self.ViewData.TryGetValue(ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonForm), out parentFormObject))
            {
                return false;
            }

            object parentExpression;
            if (!self.ViewData.TryGetValue(ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonExpression), out parentExpression))
            {
                return false;
            }

            var parentForm = parentFormObject as IForm;
            return parentForm != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsChameleonSection<TModel>(this WebViewPage<TModel> self)
        {
            object parentSectionObject;
            if (!self.ViewData.TryGetValue(ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonSection), out parentSectionObject))
            {
                return false;
            }

            object parentExpression;
            if (!self.ViewData.TryGetValue(ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.ChameleonExpression), out parentExpression))
            {
                return false;
            }

            var parentSection = parentSectionObject as ISection;
            return parentSection != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int ChameleonPartialIndex<TModel>(this WebViewPage<TModel> self)
        {
            var prop = ReflectionUtils.GetPropertyName((PartialExtensions.ObjectViewData x) => x.Index);
            return (int)self.ViewData[prop];
        }
    }
}
