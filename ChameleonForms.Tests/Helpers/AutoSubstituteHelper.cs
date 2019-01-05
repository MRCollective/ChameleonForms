using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text.Encodings.Web;
using System.Web;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacContrib.NSubstitute;
using ChameleonForms.Attributes;
using ChameleonForms.Example;
using ChameleonForms.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using NSubstitute;

namespace ChameleonForms.Tests.Helpers
{
    public static class AutoSubstituteContainer
    {
        public static AutoSubstitute Create()
        {
            return Create(null);
        }

        public static IModelMetadataProvider CreateDefaultProvider(AutoSubstitute autoSubstitute)
        {
            var detailsProviders = new IMetadataDetailsProvider[]
            {
                new DefaultBindingMetadataProvider(),
                new DefaultValidationMetadataProvider(),
                autoSubstitute.Resolve<DataAnnotationsMetadataProvider>(),
                // new DataMemberRequiredBindingMetadataProvider(),
                new ModelMetadataAwareDisplayMetadataProvider(),
            };

            var compositeDetailsProvider = new DefaultCompositeMetadataDetailsProvider(detailsProviders);
            return new DefaultModelMetadataProvider(compositeDetailsProvider);
        }

        public static AutoSubstitute Create(Action<ContainerBuilder> createAction)
        {
            AutoSubstitute autoSubstitute = createAction != null
                ? new AutoSubstitute(createAction)
                : new AutoSubstitute();

            var httpContext = new DefaultHttpContext();

            httpContext.RequestServices = new AutofacServiceProvider(autoSubstitute.Container);

            autoSubstitute.Provide<HttpContext>(httpContext);

            var request = httpContext.Request;
            request.ContentType = "application/x-www-form-urlencoded";
            var formParameters = new FormCollection(new Dictionary<string, StringValues>());
            request.Form = formParameters;
            var qsParameters = new QueryCollection();
            request.Query = qsParameters;
            var headers = new HeaderDictionary { { "Host", "localhost" } };
            request.Headers.Add("Host", "localhost");
            request.Cookies = new RequestCookieCollection();
            autoSubstitute.Provide<HttpRequest>(request);

            var response = httpContext.Response;
            //response.Cookies.Returns(new ResponseCookies());
            autoSubstitute.Provide<HttpResponse>(response);

            var routeData = new RouteData();
            var modelState = new ModelStateDictionary();
            var actionDescriptor = new ControllerActionDescriptor();
            var actionContext = new ActionContext(httpContext, routeData, actionDescriptor, modelState);
            autoSubstitute.Provide(actionContext);

            autoSubstitute.Provide(HtmlEncoder.Default);
            autoSubstitute.Provide(UrlEncoder.Default);

            IOptions<MvcDataAnnotationsLocalizationOptions> dataAnnotationsLocalizationOptions = Substitute.For<IOptions<MvcDataAnnotationsLocalizationOptions>>();
            dataAnnotationsLocalizationOptions.Value.Returns(new MvcDataAnnotationsLocalizationOptions());
            autoSubstitute.Provide(dataAnnotationsLocalizationOptions);
            IOptions<MvcOptions> mvcOptions = Substitute.For<IOptions<MvcOptions>>();
            mvcOptions.Value.Returns(new MvcOptions());
            autoSubstitute.Provide(mvcOptions);

            var metadataProvider = CreateDefaultProvider(autoSubstitute);
            var viewData = new ViewDataDictionary(metadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Substitute.For<ITempDataProvider>());
            var pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };
            autoSubstitute.Provide(pageContext);
            autoSubstitute.Provide(tempData);
            autoSubstitute.Provide(viewData);

            //var actionExecutingContext = Substitute.For<ActionExecutingContext>(requestContext);
            //actionExecutingContext.HttpContext.Returns(httpContext);
            //actionExecutingContext.RouteData.Returns(routeData);
            //autoSubstitute.Provide(actionExecutingContext);

            //var actionExecutedContext = Substitute.For<ActionExecutedContext>(requestContext);
            //actionExecutedContext.HttpContext.Returns(httpContext);
            //actionExecutedContext.RouteData.Returns(routeData);
            //autoSubstitute.Provide(actionExecutedContext);

            //var controller = Substitute.For<ControllerBase>();
            //autoSubstitute.Provide(controller);
            ////actionExecutingContext.Controller.Returns(controller);

            //var controllerContext = new ControllerContext(actionContext);
            //controllerContext.HttpContext = httpContext;
            //controllerContext.RouteData = routeData;
            //autoSubstitute.Provide(controllerContext);
            //controller.ControllerContext = controllerContext;


            IOptions<MvcDataAnnotationsLocalizationOptions> dataAnnotationOptions = Substitute.For<IOptions<MvcDataAnnotationsLocalizationOptions>>();
            dataAnnotationOptions.Value.Returns(new MvcDataAnnotationsLocalizationOptions());

            autoSubstitute.Provide<IModelMetadataProvider>(metadataProvider);

            var iView = Substitute.For<IView>();
            autoSubstitute.Provide(iView);
            var viewDataDictionary = new ViewDataDictionary(metadataProvider, new ModelStateDictionary());
            autoSubstitute.Provide(viewDataDictionary);

            var textWriter = Substitute.For<TextWriter>();
            autoSubstitute.Provide(textWriter);

            IValidationAttributeAdapterProvider validationAttributeAdapterProvider = new RequiredFlagsEnumAttributeAdapterProvider();

            autoSubstitute.Provide(validationAttributeAdapterProvider);

            var mvcViewOptionsWrap = Substitute.For<IOptions<MvcViewOptions>>();

            MvcViewOptionsSetup optionsSetup = new MvcViewOptionsSetup(dataAnnotationOptions, validationAttributeAdapterProvider);
            var mvcViewOptions = new MvcViewOptions();
            mvcViewOptionsWrap.Value.Returns(mvcViewOptions);
            optionsSetup.Configure(mvcViewOptions);
            autoSubstitute.Provide<IOptions<MvcViewOptions>>(mvcViewOptionsWrap);
            autoSubstitute.Provide<ValidationHtmlAttributeProvider>(autoSubstitute.Resolve<DefaultValidationHtmlAttributeProvider>());
            autoSubstitute.Provide<IHtmlGenerator>(autoSubstitute.Resolve<DefaultHtmlGenerator>());

            //var viewContext = new ViewContext(controllerContext, iView, viewDataDictionary, tempDataDictionary, textWriter, new HtmlHelperOptions())
            //{
            //    HttpContext = httpContext,
            //    RouteData = routeData,
            //    //RequestContext = requestContext,
            //    //Controller = controller
            //};
            //autoSubstitute.Provide(viewContext);

            //var htmlHelper = new HtmlHelper(viewContext, new ViewPage());
            //autoSubstitute.Provide(htmlHelper);

            autoSubstitute.Provide(new UrlHelper(autoSubstitute.Resolve<ActionContext>()));

            return autoSubstitute;
        }

        public static T GetController<T>(this AutoSubstitute autoSubstitute) where T : Controller
        {
            var controller = autoSubstitute.Resolve<T>();
            controller.ControllerContext = autoSubstitute.Resolve<ControllerContext>();
            controller.Url = autoSubstitute.Resolve<UrlHelper>();
            return controller;
        }
    }
}
