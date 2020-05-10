using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace ChameleonForms.Utils
{
    /// <summary>
    /// Extension methods on <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/>.
    /// </summary>
    public static class ViewContextExtensions
    {
        /// <summary>
        /// Returns an <see cref="IHtmlHelper{TModel}"/> which has been resolved from request services
        /// and contextualised to the given <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/>.
        /// </summary>
        /// <typeparam name="TModel">The model type to return a HTML Helper instance for</typeparam>
        /// <param name="viewContext">The view context to contextualise against</param>
        /// <returns>The contextualised HTML helper</returns>
        public static IHtmlHelper<TModel> GetHtmlHelper<TModel>(this ViewContext viewContext)
        {
            var helper = viewContext.HttpContext.RequestServices.GetRequiredService<IHtmlHelper<TModel>>();
            (helper as HtmlHelper<TModel>)?.Contextualize(viewContext);
            return helper;
        }
    }
}
