using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields
{
    internal class TextInputField : IField
    {
        private readonly IWebElement _element;

        public TextInputField(IWebElement element)
        {
            _element = element;
        }

        public void Set(IModelFieldValue value)
        {
            _element.ClearAndSendKeys(value.Value);
        }

        public object Get(IModelFieldType fieldType)
        {
            return fieldType.GetValueFromString(_element.GetAttribute("value"));
        }
    }
}