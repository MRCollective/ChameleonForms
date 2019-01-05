using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Text.Encodings.Web;

namespace ChameleonForms
{
    /// <summary>
    /// Extension methods against HtmlHelper.
    /// </summary>
    // http://maxtoroq.github.io/2012/07/patterns-for-aspnet-mvc-plugins-viewmodels.html
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Creates a HTML helper from a parent model to use a sub-property as it's model.
        /// </summary>
        /// <typeparam name="TParentModel">The model of the parent type</typeparam>
        /// <typeparam name="TChildModel">The model of the sub-property to use</typeparam>
        /// <param name="helper">The parent HTML helper</param>
        /// <param name="propertyFor">The sub-property to use</param>
        /// <param name="bindFieldsToParent">Whether to set field names to bind to the parent model type (true) or the sub-property type (false)</param>
        /// <returns>A HTML helper against the sub-property</returns>
        public static DisposableHtmlHelper<TChildModel> For<TParentModel, TChildModel>(this IHtmlHelper<TParentModel> helper,
            Expression<Func<TParentModel, TChildModel>> propertyFor, bool bindFieldsToParent)
        {
            return helper.For(helper.ViewData.Model != null ? propertyFor.Compile().Invoke(helper.ViewData.Model) : default(TChildModel),
                bindFieldsToParent ? ExpressionHelper.GetExpressionText(propertyFor) : null);
        }

        /// <summary>
        /// Creates a HTML helper based on another HTML helper against a different model type.
        /// </summary>
        /// <typeparam name="TModel">The model type to create a helper for</typeparam>
        /// <param name="htmlHelper">The original HTML helper</param>
        /// <param name="model">An instance of the model type to use as the model</param>
        /// <param name="htmlFieldPrefix">A prefix value to use for field names</param>
        /// <returns>The HTML helper against the other model type</returns>
        public static DisposableHtmlHelper<TModel> For<TModel>(this IHtmlHelper htmlHelper
            , TModel model = default(TModel)
            , string htmlFieldPrefix = null
            )
        {
            var viewContext = htmlHelper.ViewContext;
            var newViewData = new ViewDataDictionary<TModel>(viewContext.ViewData, model);

            if (!string.IsNullOrEmpty(htmlFieldPrefix))
            {
                newViewData.TemplateInfo.HtmlFieldPrefix = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldPrefix);
            }

            var newViewContext = new ViewContext(viewContext
                , viewContext.View
                , newViewData
                , viewContext.Writer
                );

            var htmlGenerator = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IHtmlGenerator>();
            var viewEngine = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<ICompositeViewEngine>();
            var bufferScope = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IViewBufferScope>();
            var ret = new DisposableHtmlHelper<TModel>(htmlGenerator
                , viewEngine
                , htmlHelper.MetadataProvider
                , bufferScope
                , HtmlEncoder.Default
                , htmlHelper.UrlEncoder
                , new ExpressionTextCache()
                );
            ret.Contextualize(newViewContext);
            return ret;
        }
    }

    /// <summary>
    /// HTML helper that can be created in a using block.
    /// </summary>
    /// <typeparam name="TModel">The model type of the HTML helper</typeparam>
    public class DisposableHtmlHelper<TModel> : HtmlHelper<TModel>, IDisposable
    {
        public DisposableHtmlHelper(IHtmlGenerator htmlGenerator
            , ICompositeViewEngine viewEngine
            , IModelMetadataProvider metadataProvider
            , IViewBufferScope bufferScope
            , HtmlEncoder htmlEncoder
            , UrlEncoder urlEncoder
            , ExpressionTextCache expressionTextCache
            ) 
            : base(htmlGenerator
                  , viewEngine
                  , metadataProvider
                  , bufferScope
                  , htmlEncoder
                  , urlEncoder
                  , expressionTextCache
                  )
        {
        }

        /// <inheritdoc />
        public void Dispose() {}
    }
}
