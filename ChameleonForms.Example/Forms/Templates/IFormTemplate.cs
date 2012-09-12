using System.Net.Http;
using System.Web;

namespace ChameleonForms.Example.Forms.Templates
{
    public interface IFormTemplate
    {
        IHtmlString BeginForm(string action, HttpMethod method, string enctype);
        IHtmlString EndForm();
        IHtmlString BeginSection(string title);
        IHtmlString EndSection();
        IHtmlString Field(IHtmlString elementHtml, IHtmlString labelHtml, IHtmlString validationHtml);
    }
}