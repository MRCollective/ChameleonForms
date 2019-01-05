using AngleSharp.Dom.Html;
using ChameleonForms.AcceptanceTests.Helpers.Pages;
using ChameleonForms.Example.Controllers;
using RazorPagesProject.Tests.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages
{
    public class ModelBindingExamplePage : ChameleonFormsPage<ModelBindingViewModel>
    {
        public ModelBindingExamplePage(IHtmlDocument content) 
            : base(content)
        {
        }

        public async Task<ModelBindingExamplePage> SubmitAsync(HttpClient client, ModelBindingViewModel vm)
        {
            IEnumerable<KeyValuePair<string, string>> dict = InputModel(vm);
            return new ModelBindingExamplePage(await HtmlHelpers.GetDocumentAsync(await client.SendAsync("/ExampleForms/ModelBindingExample"
                , (IHtmlElement)this.Content.QuerySelector("button[type=submit]")
                , dict
                )));
        }
    }
}
