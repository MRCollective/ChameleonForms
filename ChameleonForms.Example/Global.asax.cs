using System.Web.Routing;

namespace ChameleonForms.Example
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            HumanizedLabels.Register();
        }
    }
}