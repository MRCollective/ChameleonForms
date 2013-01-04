using OpenQA.Selenium;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.PageObjects;

namespace ChameleonForms.Tests.ModelBinding.Pages
{
    public class HomePage : Page
    {
        public HomePage()
        {
            Browser.Navigate().GoToUrl(((SelenoApplication)SelenoApplicationRunner.Host).WebServer.BaseUrl);
        }

        public ModelBindingExamplePage GoToModelBindingExamplePage()
        {
            return Navigate().To<ModelBindingExamplePage>(By.LinkText("Model Binding Example"));
        }

        public ModelBindingExamplePage GoToModelBindingExamplePage2()
        {
            return Navigate().To<ModelBindingExamplePage>(By.LinkText("Model Binding Example with lists"));
        }
    }
}
