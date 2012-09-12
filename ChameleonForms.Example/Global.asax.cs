using System.Web.Routing;
using ChameleonForms.Example.Forms;

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