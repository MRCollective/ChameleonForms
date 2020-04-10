using System;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyTested.AspNetCore.Mvc.Internal;
using MyTested.AspNetCore.Mvc.Internal.Application;
using NSubstitute;

namespace ChameleonForms.Tests.Helpers
{
    public class MvcTestContext : IDisposable
    {
        static MvcTestContext()
        {
            TestApplication.TryInitialize();
        }

        public MvcTestContext()
        {
            HttpContext = new DefaultHttpContext { RequestServices = TestApplication.Services };

            RouteData = new RouteData();
            ModelState = new ModelStateDictionary();
            ActionDescriptor = new ControllerActionDescriptor();
            ActionContext = new ActionContext(HttpContext, RouteData, ActionDescriptor, ModelState);

            ControllerContext = new ControllerContext(ActionContext);

            ModelMetadataProvider = TestApplication.Services.GetRequiredService<IModelMetadataProvider>();
            HtmlGenerator = TestApplication.Services.GetRequiredService<IHtmlGenerator>();

            ValidationHtmlAttributeProvider = new DefaultValidationHtmlAttributeProvider(
                Options.Create<MvcViewOptions>(new MvcViewOptions()), ModelMetadataProvider,
                new ClientValidatorCache());

            UrlHelper = new UrlHelper(ActionContext);
        }

        public MvcViewTestContext<TModel> GetViewTestContext<TModel>()
        {
            return new MvcViewTestContext<TModel>(ActionContext, ModelMetadataProvider, HtmlGenerator);
        }

        public DefaultValidationHtmlAttributeProvider ValidationHtmlAttributeProvider { get; }

        public UrlHelper UrlHelper { get; }

        public ControllerContext ControllerContext { get; }

        public IHtmlGenerator HtmlGenerator { get; }

        public IModelMetadataProvider ModelMetadataProvider { get; }

        public ActionContext ActionContext { get; }

        public ControllerActionDescriptor ActionDescriptor { get; }

        public ModelStateDictionary ModelState { get; }

        public RouteData RouteData { get; }

        public DefaultHttpContext HttpContext { get; }

        public void Dispose() { }
    }

    public class MvcViewTestContext<TModel> : IDisposable
    {
        public MvcViewTestContext(ActionContext actionContext, IModelMetadataProvider modelMetadataProvider, IHtmlGenerator htmlGenerator)
        {
            ViewData = new ViewDataDictionary<TModel>(modelMetadataProvider, new ModelStateDictionary());
            View = new NullView();
            Writer = Substitute.For<TextWriter>();
            TempData = new TempDataDictionary(actionContext.HttpContext, new TempDataProviderMock());
            var htmlHelperOptions = new HtmlHelperOptions();
            htmlHelperOptions.ClientValidationEnabled = true;
            ViewContext = new ViewContext(actionContext, View, ViewData, TempData, Writer, htmlHelperOptions);

            HtmlHelper = new HtmlHelper<TModel>(
                htmlGenerator,
                TestApplication.Services.GetRequiredService<ICompositeViewEngine>(),
                TestApplication.Services.GetRequiredService<IModelMetadataProvider>(),
                TestApplication.Services.GetRequiredService<IViewBufferScope>(),
                HtmlEncoder.Default,
                UrlEncoder.Default,
                new ModelExpressionProvider(TestApplication.Services.GetRequiredService<IModelMetadataProvider>()));
            HtmlHelper.Contextualize(ViewContext);
        }

        public HtmlHelper<TModel> HtmlHelper { get; }

        public ViewContext ViewContext { get; }

        public TempDataDictionary TempData { get; }

        public TextWriter Writer { get; }

        public IView View { get; }

        public ViewDataDictionary<TModel> ViewData { get; }

        public void Dispose() { }

    }
}
