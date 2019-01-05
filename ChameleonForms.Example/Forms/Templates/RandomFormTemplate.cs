using System.Web;
using ChameleonForms.Templates.Default;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Example.Forms.Templates
{
    public class RandomFormTemplate : DefaultFormTemplate
    {
        public string RandomComponent()
        {
            return "RANDOM unencoded string &\"<g>!";
        }

        public IHtmlContent RandomComponent2()
        {
            return new HtmlString("<p>Some encoded HTML</p>");
        }
    }
}