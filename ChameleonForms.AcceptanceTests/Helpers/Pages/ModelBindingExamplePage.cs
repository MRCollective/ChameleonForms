using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ChameleonForms.Example.Controllers;

namespace ChameleonForms.AcceptanceTests.Helpers.Pages
{
    public class ModelBindingExamplePage : ChameleonFormsPage<ModelBindingViewModel>
    {
        public ModelBindingExamplePage(IHtmlDocument content) : base(content)
        {}

        public async Task<ModelBindingExamplePage> SubmitAsync(ModelBindingViewModel vm)
        {
            InputModel(vm);
            return await NavigateToAsync<ModelBindingExamplePage>(Content.QuerySelector("button[type=submit]"));
        }
    }
}
