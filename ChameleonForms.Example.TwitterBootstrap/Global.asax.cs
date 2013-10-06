using System.Web;
using System.Web.Routing;
using ChameleonForms.Templates;

namespace ChameleonForms.Example.TwitterBootstrap
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FormTemplate.Default = new TwitterBootstrapFormTemplate();
        }
    }
}