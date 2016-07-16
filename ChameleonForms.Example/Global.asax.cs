using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using ChameleonForms.Example.Controllers;
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
            typeof(ExampleFormsController).Assembly.GetTypes().Where(t => t.IsEnum && t.GetCustomAttributes(typeof(FlagsAttribute), false).Any())
                .ToList().ForEach(t =>
                {
                    System.Web.Mvc.ModelBinders.Binders.Add(t, new FlagsEnumModelBinder());
                    System.Web.Mvc.ModelBinders.Binders.Add(typeof(Nullable<>).MakeGenericType(t), new FlagsEnumModelBinder());
                });
            GlobalFilters.Filters.Add(new FormTemplateFilter());
        }
    }
}