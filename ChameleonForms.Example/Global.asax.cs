using System;
using System.Web.Mvc;
using System.Web.Routing;
using ChameleonForms.Example.Controllers.Filters;
using ChameleonForms.ModelBinders;

namespace ChameleonForms.Example
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            HumanizedLabels.Register();
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
            GlobalFilters.Filters.Add(new FormTemplateFilter());
        }
    }
}