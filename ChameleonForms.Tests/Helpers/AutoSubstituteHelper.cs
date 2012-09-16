using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using AutofacContrib.NSubstitute;
using NSubstitute;

namespace ChameleonForms.Tests.Helpers
{
    public static class AutoSubstituteContainer
    {
        public static AutoSubstitute Create()
        {
            return Create(null);
        }

        public static AutoSubstitute Create(Action<ContainerBuilder> createAction)
        {
            var autoSubstitute = createAction == null ? new AutoSubstitute() : new AutoSubstitute(createAction);

            var httpContext = Substitute.For<HttpContextBase>();
            autoSubstitute.Provide(httpContext);

            var server = Substitute.For<HttpServerUtilityBase>();
            httpContext.Server.Returns(server);

            var request = Substitute.For<HttpRequestBase>();
            var parameters = new NameValueCollection();
            request.Params.Returns(parameters);
            var formParameters = new NameValueCollection();
            request.Form.Returns(formParameters);
            var qsParameters = new NameValueCollection();
            request.QueryString.Returns(qsParameters);
            var headers = new NameValueCollection();
            headers.Add("Host", "localhost");
            request.Headers.Returns(headers);
            request.AppRelativeCurrentExecutionFilePath.Returns("~/");
            request.ApplicationPath.Returns("/");
            request.Url.Returns(new Uri("http://localhost/"));
            request.Cookies.Returns(new HttpCookieCollection());
            request.ServerVariables.Returns(new NameValueCollection());
            autoSubstitute.Provide(request);
            httpContext.Request.Returns(request);

            var response = Substitute.For<HttpResponseBase>();
            response.Cookies.Returns(new HttpCookieCollection());
            response.ApplyAppPathModifier(Arg.Any<string>()).Returns(a => a.Arg<string>());
            autoSubstitute.Provide(response);
            httpContext.Response.Returns(response);

            var routeData = new RouteData();

            var requestContext = Substitute.For<RequestContext>();
            requestContext.RouteData = routeData;
            requestContext.HttpContext = httpContext;
            autoSubstitute.Provide(requestContext);

            var actionExecutingContext = Substitute.For<ActionExecutingContext>();
            actionExecutingContext.HttpContext.Returns(httpContext);
            actionExecutingContext.RouteData.Returns(routeData);
            actionExecutingContext.RequestContext = requestContext;
            autoSubstitute.Provide(actionExecutingContext);

            var actionExecutedContext = Substitute.For<ActionExecutedContext>();
            actionExecutedContext.HttpContext.Returns(httpContext);
            actionExecutedContext.RouteData.Returns(routeData);
            actionExecutedContext.RequestContext = requestContext;
            autoSubstitute.Provide(actionExecutedContext);

            var controller = Substitute.For<ControllerBase>();
            autoSubstitute.Provide(controller);
            actionExecutingContext.Controller.Returns(controller);

            var controllerContext = Substitute.For<ControllerContext>();
            controllerContext.HttpContext = httpContext;
            controllerContext.RouteData = routeData;
            controllerContext.RequestContext = requestContext;
            controllerContext.Controller = controller;
            autoSubstitute.Provide(controllerContext);
            controller.ControllerContext = controllerContext;

            var iView = Substitute.For<IView>();
            autoSubstitute.Provide(iView);

            var viewDataDictionary = new ViewDataDictionary();
            autoSubstitute.Provide(viewDataDictionary);

            var iViewDataContainer = Substitute.For<IViewDataContainer>();
            iViewDataContainer.ViewData.Returns(viewDataDictionary);
            autoSubstitute.Provide(iViewDataContainer);

            var textWriter = Substitute.For<TextWriter>();
            autoSubstitute.Provide(textWriter);

            var viewContext = new ViewContext(controllerContext, iView, viewDataDictionary, new TempDataDictionary(), textWriter)
            {
                HttpContext = httpContext,
                RouteData = routeData,
                RequestContext = requestContext,
                Controller = controller
            };
            autoSubstitute.Provide(viewContext);

            return autoSubstitute;
        }
    }
}