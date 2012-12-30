using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;

namespace ChameleonForms.Tests.ModelBinding.Pages
{
    public class HomePage : Page
    {
        public ModelBindingExamplePage GoToModelBindingExamplePage()
        {
            return Navigate().To<ModelBindingExamplePage>(By.LinkText("Model Binding Example"));
        }
    }
}
