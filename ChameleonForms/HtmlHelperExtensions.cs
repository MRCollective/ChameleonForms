using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using ChameleonForms.Templates;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;

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
                bindFieldsToParent ? helper.GetFieldName(propertyFor) : null);
        }
        
        /// <summary>
        /// Creates a HTML helper based on another HTML helper against a different model type.
        /// </summary>
        /// <typeparam name="TModel">The model type to create a helper for</typeparam>
        /// <param name="htmlHelper">The original HTML helper</param>
        /// <param name="model">An instance of the model type to use as the model</param>
        /// <param name="htmlFieldPrefix">A prefix value to use for field names</param>
        /// <returns>The HTML helper against the other model type</returns>
        public static DisposableHtmlHelper<TModel> For<TModel>(this IHtmlHelper htmlHelper,
            TModel model = default(TModel), string htmlFieldPrefix = null)
        {
            var viewContext = htmlHelper.ViewContext;
            var newViewData = new ViewDataDictionary<TModel>(viewContext.ViewData, model);

            if (!string.IsNullOrEmpty(htmlFieldPrefix))
            {
                newViewData.TemplateInfo.HtmlFieldPrefix = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldPrefix);
            }

            var newViewContext = new ViewContext(viewContext, viewContext.View, newViewData, viewContext.Writer);

            var services = htmlHelper.ViewContext.HttpContext.RequestServices;

            return new DisposableHtmlHelper<TModel>(
                services.GetRequiredService<IHtmlGenerator>(),
                services.GetRequiredService<ICompositeViewEngine>(),
                services.GetRequiredService<IModelMetadataProvider>(),
                services.GetRequiredService<IViewBufferScope>(),
                HtmlEncoder.Default,
                UrlEncoder.Default,
                new ModelExpressionProvider(services.GetRequiredService<IModelMetadataProvider>()), 
                newViewContext);
        }

        /// <summary>
        /// Gets the registered default form template from RequestServices.
        /// </summary>
        /// <param name="htmlHelper">The HTML Helper</param>
        /// <returns>An instance of the default <see cref="IFormTemplate"/></returns>
        public static IFormTemplate GetDefaultFormTemplate(this IHtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IFormTemplate>();
        }

        /// <summary>
        /// Returns the full HTML field name for a field in a view model within the current context / prefix.
        /// </summary>
        /// <typeparam name="TModel">The view model type</typeparam>
        /// <typeparam name="TResult">The field type</typeparam>
        /// <param name="htmlHelper">The HTML helper</param>
        /// <param name="field">The field</param>
        /// <returns>The full HTML field name</returns>
        public static string GetFullHtmlFieldName<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> field)
        {
            return htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlHelper.GetFieldName(field));
        }

        /// <summary>
        /// Returns the field name for a field in a view model.
        /// </summary>
        /// <typeparam name="TModel">The view model type</typeparam>
        /// <typeparam name="TResult">The field type</typeparam>
        /// <param name="htmlHelper">The HTML helper</param>
        /// <param name="field">The field</param>
        /// <returns>The field name</returns>
        public static string GetFieldName<TModel, TResult>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> field)
        {
            var expressionProvider = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<ModelExpressionProvider>();
            return expressionProvider.CreateModelExpression(htmlHelper.ViewData, field).Name;
        }
    }

    /// <summary>
    /// HTML helper that can be created in a using block.
    /// </summary>
    /// <typeparam name="TModel">The model type of the HTML helper</typeparam>
    public class DisposableHtmlHelper<TModel> : HtmlHelper<TModel>, IDisposable
    {
        /// <summary>
        /// Creates a <see cref="DisposableHtmlHelper{TModel}"/> to wrap a scope around a new HtmlHelper instance.
        /// </summary>
        /// <param name="htmlGenerator">The HTML generator to use</param>
        /// <param name="viewEngine">The view engine to use</param>
        /// <param name="metadataProvider">The metadata provider to use</param>
        /// <param name="bufferScope">The buffer scope to use</param>
        /// <param name="htmlEncoder">The HTML encoder to use</param>
        /// <param name="urlEncoder">The URL encoder to use</param>
        /// <param name="modelExpressionProvider">The model expression provider</param>
        /// <param name="viewContext">The new view context to wrap</param>
        public DisposableHtmlHelper(IHtmlGenerator htmlGenerator, ICompositeViewEngine viewEngine, IModelMetadataProvider metadataProvider, IViewBufferScope bufferScope, HtmlEncoder htmlEncoder, UrlEncoder urlEncoder, ModelExpressionProvider modelExpressionProvider, ViewContext viewContext)
            : base(htmlGenerator, viewEngine, metadataProvider, bufferScope, htmlEncoder, urlEncoder, modelExpressionProvider)
        {
            Contextualize(viewContext);
        }

        /// <summary>
        /// Dispose of the scope.
        /// </summary>
        public void Dispose() {}
    }
}
