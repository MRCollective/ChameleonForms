using Microsoft.AspNetCore.Mvc.Filters;

namespace ChameleonForms.Example.Controllers.Filters
{
    public class FormTemplateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var templateInParams = filterContext.HttpContext.Request.Query["template"];
            if (!string.IsNullOrEmpty(templateInParams))
            {
                filterContext.HttpContext.Response.Cookies.Append("template", templateInParams);
            }
        }
    }
}