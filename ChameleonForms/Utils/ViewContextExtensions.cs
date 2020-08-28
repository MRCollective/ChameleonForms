using System.Linq;
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
            // If the view data dictionary isn't typed correctly, then replace it with the correctly-typed version
            // This can happen when you have a partial view which is a base type of the model type
            var viewDataType = viewContext.ViewData.GetType();
            if (viewDataType.IsGenericType && viewDataType.GetGenericTypeDefinition() == typeof(ViewDataDictionary<>) && viewDataType.GetGenericArguments()[0] != typeof(TModel) && viewContext.ViewData.ModelMetadata.ModelType == typeof(TModel))
            {
                viewContext.ViewData = new ViewDataDictionary<TModel>(viewContext.ViewData);
            }
            (helper as HtmlHelper<TModel>)?.Contextualize(viewContext);
            return helper;
        }
    }
}
