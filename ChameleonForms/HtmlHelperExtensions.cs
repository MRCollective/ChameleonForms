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
                bindFieldsToParent ? helper.GetExpressionText(propertyFor) : null);
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
    }

    /// <summary>
    /// HTML helper that can be created in a using block.
    /// </summary>
    /// <typeparam name="TModel">The model type of the HTML helper</typeparam>
    public class DisposableHtmlHelper<TModel> : Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper<TModel>, IDisposable
    {
        public DisposableHtmlHelper(IHtmlGenerator htmlGenerator, ICompositeViewEngine viewEngine, IModelMetadataProvider metadataProvider, IViewBufferScope bufferScope, HtmlEncoder htmlEncoder, UrlEncoder urlEncoder, ModelExpressionProvider modelExpressionProvider, ViewContext viewContext)
            : base(htmlGenerator, viewEngine, metadataProvider, bufferScope, htmlEncoder, urlEncoder, modelExpressionProvider)
        {
            Contextualize(viewContext);
        }

        public void Dispose() {}
    }
    /*public class DisposableHtmlHelper<TModel> : IHtmlHelper<TModel>, IDisposable
    {
        private readonly HtmlHelper _htmlHelper;
        private ViewContext _originalContext;

        public DisposableHtmlHelper(HtmlHelper htmlHelper, ViewContext newContext)
        {
            _htmlHelper = htmlHelper;
            _originalContext = _htmlHelper.ViewContext;
            _htmlHelper.Contextualize(newContext);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _htmlHelper.Contextualize(_originalContext);
        }

        public IHtmlContent ActionLink(string linkText, string actionName, string controllerName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes)
            => _htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostname, fragment, routeValues, htmlAttributes);


        public Html5DateRenderingMode Html5DateRenderingMode => _htmlHelper.Html5DateRenderingMode;
        public string IdAttributeDotReplacement => _htmlHelper.IdAttributeDotReplacement;
        public IModelMetadataProvider MetadataProvider => _htmlHelper.MetadataProvider;
        public dynamic ViewBag => _htmlHelper.ViewBag;
        public ViewContext ViewContext => _htmlHelper.ViewContext;
        ViewDataDictionary IHtmlHelper.ViewData => _htmlHelper.ViewData;
        public ViewDataDictionary<TModel> ViewData => _htmlHelper.ViewData as ViewDataDictionary<TModel>;
        public ITempDataDictionary TempData => _htmlHelper.TempData;
        public UrlEncoder UrlEncoder => _htmlHelper.UrlEncoder;

        public IHtmlContent AntiForgeryToken()
            => _htmlHelper.AntiForgeryToken();

        public MvcForm BeginForm(string actionName, string controllerName, object routeValues, FormMethod method, bool? antiforgery, object htmlAttributes)
            => _htmlHelper.BeginForm(actionName, controllerName, routeValues, method, antiforgery, htmlAttributes);

        public MvcForm BeginRouteForm(string routeName, object routeValues, FormMethod method, bool? antiforgery, object htmlAttributes)
            => _htmlHelper.BeginRouteForm(routeName, routeValues, method, antiforgery, htmlAttributes);

        public IHtmlContent CheckBox(string expression, bool? isChecked, object htmlAttributes)
            => _htmlHelper.CheckBox(expression, isChecked, htmlAttributes);

        public IHtmlContent Display(string expression, string templateName, string htmlFieldName, object additionalViewData)
            => _htmlHelper.Display(expression, templateName, htmlFieldName, additionalViewData);

        public string DisplayName(string expression)
            => _htmlHelper.DisplayName(expression);

        public string DisplayText(string expression)
            => _htmlHelper.DisplayText(expression);

        public IHtmlContent DropDownList(string expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
            => _htmlHelper.DropDownList(expression, selectList, optionLabel, htmlAttributes);

        public IHtmlContent Editor(string expression, string templateName, string htmlFieldName, object additionalViewData)
            => _htmlHelper.Editor(expression, templateName, htmlFieldName, additionalViewData);

        string IHtmlHelper<TModel>.Encode(object value)
            => _htmlHelper.Encode(value);

        string IHtmlHelper<TModel>.Encode(string value)
            => _htmlHelper.Encode(value);

        public IHtmlContent HiddenFor<TResult>(Expression<Func<TModel, TResult>> expression, object htmlAttributes)
            => _htmlHelper.HiddenFor(expression, htmlAttributes);

        public string IdFor<TResult>(Expression<Func<TModel, TResult>> expression)
            => _htmlHelper.IdFor(expression);

        public IHtmlContent LabelFor<TResult>(Expression<Func<TModel, TResult>> expression, string labelText,
            object htmlAttributes)
            => _htmlHelper.LabelFor(expression, labelText, htmlAttributes);

        public IHtmlContent ListBoxFor<TResult>(Expression<Func<TModel, TResult>> expression,
            IEnumerable<SelectListItem> selectList, object htmlAttributes)
            => _htmlHelper.ListBoxFor<TResult>(expression, selectList, htmlAttributes);

        public string NameFor<TResult>(Expression<Func<TModel, TResult>> expression)
            => _htmlHelper.NameFor(expression);

        public IHtmlContent PasswordFor<TResult>(Expression<Func<TModel, TResult>> expression, object htmlAttributes)
            => _htmlHelper.PasswordFor(expression, htmlAttributes);

        public IHtmlContent RadioButtonFor<TResult>(Expression<Func<TModel, TResult>> expression, object value,
            object htmlAttributes)
            => _htmlHelper.RadioButtonFor(expression, value, htmlAttributes);

        IHtmlContent IHtmlHelper<TModel>.Raw(object value)
            => _htmlHelper.Raw(value);

        IHtmlContent IHtmlHelper<TModel>.Raw(string value)
            => _htmlHelper.Raw(value);

        public IHtmlContent TextAreaFor<TResult>(Expression<Func<TModel, TResult>> expression, int rows,
            int columns, object htmlAttributes)
            => _htmlHelper.TextAreaFor(expression, rows, columns, htmlAttributes);

        public IHtmlContent TextBoxFor<TResult>(Expression<Func<TModel, TResult>> expression, string format,
            object htmlAttributes)
            => _htmlHelper.TextBoxFor(expression, format, htmlAttributes);

        public IHtmlContent ValidationMessageFor<TResult>(Expression<Func<TModel, TResult>> expression,
            string message, object htmlAttributes, string tag)
            => _htmlHelper.ValidationMessageFor(expression, message, htmlAttributes, tag);

        public string ValueFor<TResult>(Expression<Func<TModel, TResult>> expression, string format)
            => _htmlHelper.ValueForModel(expression, format);

        public IHtmlContent CheckBoxFor(Expression<Func<TModel, bool>> expression, object htmlAttributes)
            => _htmlHelper.

        public IHtmlContent DisplayFor<TResult>(Expression<Func<TModel, TResult>> expression, string templateName, string htmlFieldName,
            object additionalViewData)
            => _htmlHelper.

        public string DisplayNameFor<TResult>(Expression<Func<TModel, TResult>> expression)
            => _htmlHelper.

        public string DisplayNameForInnerType<TModelItem, TResult>(Expression<Func<TModelItem, TResult>> expression)
            => _htmlHelper.

        public string DisplayTextFor<TResult>(Expression<Func<TModel, TResult>> expression)
            => _htmlHelper.

        public IHtmlContent DropDownListFor<TResult>(Expression<Func<TModel, TResult>> expression, IEnumerable<SelectListItem> selectList, string optionLabel,
            object htmlAttributes)
            => _htmlHelper.

        public IHtmlContent EditorFor<TResult>(Expression<Func<TModel, TResult>> expression, string templateName, string htmlFieldName,
            object additionalViewData)
            => _htmlHelper.

        string IHtmlHelper.Encode(object value)
            => _htmlHelper.

        string IHtmlHelper.Encode(string value)
            => _htmlHelper.

        public void EndForm()
            => _htmlHelper.

        public string FormatValue(object value, string format)
            => _htmlHelper.

        public string GenerateIdFromName(string fullName)
            => _htmlHelper.

        public IEnumerable<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : struct, new()
            => _htmlHelper.

        public IEnumerable<SelectListItem> GetEnumSelectList(Type enumType)
            => _htmlHelper.

        public IHtmlContent Hidden(string expression, object value, object htmlAttributes)
            => _htmlHelper.

        public string Id(string expression)
            => _htmlHelper.

        public IHtmlContent Label(string expression, string labelText, object htmlAttributes)
            => _htmlHelper.

        public IHtmlContent ListBox(string expression, IEnumerable<SelectListItem> selectList, object htmlAttributes)
            => _htmlHelper.

        public string Name(string expression)
            => _htmlHelper.

        public Task<IHtmlContent> PartialAsync(string partialViewName, object model, ViewDataDictionary viewData)
            => _htmlHelper.

        public IHtmlContent Password(string expression, object value, object htmlAttributes)
            => _htmlHelper.

        public IHtmlContent RadioButton(string expression, object value, bool? isChecked, object htmlAttributes)
            => _htmlHelper.

        IHtmlContent IHtmlHelper.Raw(string value)
            => _htmlHelper.

        IHtmlContent IHtmlHelper.Raw(object value)
            => _htmlHelper.

        public Task RenderPartialAsync(string partialViewName, object model, ViewDataDictionary viewData)
            => _htmlHelper.

        public IHtmlContent RouteLink(string linkText, string routeName, string protocol, string hostName, string fragment,
            object routeValues, object htmlAttributes)
            => _htmlHelper.

        public IHtmlContent TextArea(string expression, string value, int rows, int columns, object htmlAttributes)
            => _htmlHelper.

        public IHtmlContent TextBox(string expression, object value, string format, object htmlAttributes)
            => _htmlHelper.

        public IHtmlContent ValidationMessage(string expression, string message, object htmlAttributes, string tag)
            => _htmlHelper.

        public IHtmlContent ValidationSummary(bool excludePropertyErrors, string message, object htmlAttributes, string tag)
            => _htmlHelper.

        public string Value(string expression, string format)
            => _htmlHelper.
    }*/
}

public static class ExpressionHelper
{
    public static string GetExpressionText<TModel, TResult>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TResult>> expression)
    {
        var expressionProvider = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<ModelExpressionProvider>();
        return expressionProvider.CreateModelExpression(htmlHelper.ViewData, expression).Name;
    }
}
