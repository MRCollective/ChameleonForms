using System.Web;
using ChameleonForms.Templates.Default;

namespace ChameleonForms.Example.Forms.Templates
{
    public class RandomFormTemplate : DefaultFormTemplate
    {
        public string RandomComponent()
        {
            return "RANDOM unencoded string &\"<g>!";
        }

        public IHtmlString RandomComponent2()
        {
            return new HtmlString("<p>Some encoded HTML</p>");
        }
    }
}