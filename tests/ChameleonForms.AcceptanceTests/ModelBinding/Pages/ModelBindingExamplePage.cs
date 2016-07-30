using ChameleonForms.Example.Controllers;
using OpenQA.Selenium;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages
{
    public class ModelBindingExamplePage : ChameleonFormsPage<ModelBindingViewModel>
    {
        public ModelBindingExamplePage Submit(ModelBindingViewModel vm)
        {
            InputModel(vm);
            return Navigate.To<ModelBindingExamplePage>(By.CssSelector("button[type=submit]"));
        }
    }
}
