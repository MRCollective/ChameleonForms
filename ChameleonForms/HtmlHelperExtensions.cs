using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

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
        public static DisposableHtmlHelper<TChildModel> For<TParentModel, TChildModel>(this HtmlHelper<TParentModel> helper,
            Expression<Func<TParentModel, TChildModel>> propertyFor, bool bindFieldsToParent)
        {
            return helper.For(propertyFor.Compile().Invoke(helper.ViewData.Model),
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
        public static DisposableHtmlHelper<TModel> For<TModel>(this HtmlHelper htmlHelper, TModel model = default(TModel), string htmlFieldPrefix = null)
        {
            var viewDataContainer = CreateViewDataContainer(htmlHelper.ViewData, model);

            var templateInfo = viewDataContainer.ViewData.TemplateInfo;

            if (!String.IsNullOrEmpty(htmlFieldPrefix))
                templateInfo.HtmlFieldPrefix = templateInfo.GetFullHtmlFieldName(htmlFieldPrefix);

            var viewContext = htmlHelper.ViewContext;
            var newViewContext = new ViewContext(viewContext.Controller.ControllerContext, viewContext.View, viewDataContainer.ViewData, viewContext.TempData, viewContext.Writer);

            return new DisposableHtmlHelper<TModel>(newViewContext, viewDataContainer, htmlHelper.RouteCollection);
        }

        static IViewDataContainer CreateViewDataContainer(ViewDataDictionary viewData, object model)
        {

            var newViewData = new ViewDataDictionary(viewData)
            {
                Model = model
            };

            newViewData.TemplateInfo = new TemplateInfo
            {
                HtmlFieldPrefix = newViewData.TemplateInfo.HtmlFieldPrefix
            };

            return new ViewDataContainer
            {
                ViewData = newViewData
            };
        }

        class ViewDataContainer : IViewDataContainer
        {

            public ViewDataDictionary ViewData { get; set; }
        }
    }

    /// <summary>
    /// HTML helper that can be created in a using block.
    /// </summary>
    /// <typeparam name="TModel">The model type of the HTML helper</typeparam>
    public class DisposableHtmlHelper<TModel> : HtmlHelper<TModel>, IDisposable
    {
        /// <inheritdoc />
        public DisposableHtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer) : base(viewContext, viewDataContainer) {}
        /// <inheritdoc />
        public DisposableHtmlHelper(ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection) : base(viewContext, viewDataContainer, routeCollection) {}
        /// <inheritdoc />
        public void Dispose() {}
    }
}
