using System;
using System.Linq.Expressions;

using ChameleonForms.Component;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ChameleonForms
{
    /// <summary>
    /// Extension methods against WebViewPage.
    /// </summary>
    public static class WebViewPageExtensions
    {
        /// <summary>
        /// Key to use in ViewData to set and retrieve the current partial view model expression.
        /// </summary>
        public const string PartialViewModelExpressionViewDataKey = "PartialViewModelExpression";

        /// <summary>
        /// Get view model expression when inside a partial view.
        /// </summary>
        /// <typeparam name="TPartialViewModel">View model type of the partial view</typeparam>
        /// <param name="partial">View page for partial view</param>
        /// <returns>current partial view model expression</returns>
        public static object PartialModelExpression<TPartialViewModel>(this RazorPage<TPartialViewModel> partial)
        {
            object expression;
            if (!partial.ViewData.TryGetValue(PartialViewModelExpressionViewDataKey, out expression))
            {
                throw new InvalidOperationException("Not currently inside a form partial view.");
            }

            return expression;
        }

        /// <summary>
        /// Key to use in ViewData to set and retrieve the current form section.
        /// </summary>
        public const string CurrentFormSectionViewDataKey = "CurrentChameleonFormSection";

        /// <summary>
        /// Get current form section when inside a partial view.
        /// </summary>
        /// <typeparam name="TPartialViewModel">View model of the partial view</typeparam>
        /// <param name="partial">View page for partial view</param>
        /// <returns>Current form section</returns>
        public static ISection<TPartialViewModel> FormSection<TPartialViewModel>(this RazorPage<TPartialViewModel> partial)
        {
            object currentSection;
            if (!partial.ViewData.TryGetValue(CurrentFormSectionViewDataKey, out currentSection))
            {
                throw new InvalidOperationException("Not currently inside a form section.");
            }

            return (currentSection as ISection).CreatePartialSection(Form(partial));
        }

        /// <summary>
        /// Key to use in ViewData to set and retrieve the current form.
        /// </summary>
        public const string CurrentFormViewDataKey = "CurrentChameleonForm";

        /// <summary>
        /// Get current form when inside a partial view.
        /// </summary>
        /// <typeparam name="TPartialViewModel">View model of the partial view</typeparam>
        /// <param name="partial">View page for partial view</param>
        /// <returns>Current form</returns>
        public static IForm<TPartialViewModel> Form<TPartialViewModel>(this RazorPage<TPartialViewModel> partial)
        {
            object currentForm;
            if (!partial.ViewData.TryGetValue(CurrentFormViewDataKey, out currentForm))
            {
                throw new InvalidOperationException("Not currently inside a form section.");
            }

            return (currentForm as IForm).CreatePartialForm<TPartialViewModel>(partial.PartialModelExpression(), (HtmlHelper<TPartialViewModel>)((dynamic)partial).Html);
        }

        /// <summary>
        /// Whether or not a partial view is within a form section.
        /// </summary>
        /// <typeparam name="TPartialViewModel">View model of the partial view</typeparam>
        /// <param name="partial">View page for partial view</param>
        /// <returns>Whether the view is within a form section</returns>
        public static bool IsInFormSection<TPartialViewModel>(this RazorPage<TPartialViewModel> partial)
        {
            object currentSection;
            return partial.ViewData.TryGetValue(CurrentFormSectionViewDataKey, out currentSection);
        }
    }
}
