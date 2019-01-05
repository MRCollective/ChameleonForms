using ChameleonForms.Templates.Default;
using ChameleonForms.Templates.TwitterBootstrap3;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChameleonForms.Example.Controllers.Filters
{
    public class FormTemplateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var templateInParams = filterContext.HttpContext.Request.Query["template"];
            var templateInCookies = filterContext.HttpContext.Request.Cookies["template"];
            if (!string.IsNullOrEmpty(templateInParams))
            {
                filterContext.HttpContext.Response.Cookies.Append("template", templateInParams);
            }

            // No this is not thread-safe, but this is an example project that will only ever be used by one person at a time
            if (templateInCookies == null || templateInCookies == "default")
                FormTemplate.Default = new DefaultFormTemplate();
            else
                FormTemplate.Default = new TwitterBootstrapFormTemplate();
        }
    }
}