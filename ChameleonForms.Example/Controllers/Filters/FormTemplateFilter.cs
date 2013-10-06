using System.Web;
using System.Web.Mvc;
using ChameleonForms.Templates;

namespace ChameleonForms.Example.Controllers.Filters
{
    public class FormTemplateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var templateInParams = filterContext.HttpContext.Request.QueryString["template"];
            var templateInCookies = filterContext.HttpContext.Request.Cookies["template"];
            if (!string.IsNullOrEmpty(templateInParams))
            {
                templateInCookies = new HttpCookie("template", templateInParams);
                filterContext.HttpContext.Response.Cookies.Add(templateInCookies);
            }

            // No this is not thread-safe, but this is an example project that will only ever be used by one person at a time
            if (templateInCookies == null || templateInCookies.Value == "default")
                FormTemplate.Default = new DefaultFormTemplate();
            else
                FormTemplate.Default = new TwitterBootstrapFormTemplate();
        }
    }
}