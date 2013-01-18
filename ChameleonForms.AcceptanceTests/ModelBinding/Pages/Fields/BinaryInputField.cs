using System;
using System.Linq;
using OpenQA.Selenium;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields
{
    internal class BinaryInputField : IField
    {
        private readonly IWebElement _element;
        private readonly bool _isCheckbox;

        public BinaryInputField(IWebElement element)
        {
            _element = element;
            _isCheckbox = element.GetAttribute("type").ToLower() == "checkbox";
        }

        public void Set(IModelFieldValue value)
        {
            // Deselect if selected checkbox
            if (_isCheckbox && _element.Selected)
                _element.Click();

            if (value.HasMultipleValues)
            {
                if (value.Values.Contains(_element.GetAttribute("value"), StringComparer.InvariantCultureIgnoreCase))
                    _element.Click();
            }
            else
            {
                // Check a single checkbox wrapping a true boolean field value
                if (_isCheckbox && value.IsTrue)
                    _element.Click();
                else if (_element.GetAttribute("value").Equals(value.Value, StringComparison.InvariantCultureIgnoreCase))
                    _element.Click();
            }
        }

        public object Get(IModelFieldType fieldType)
        {
            if (_isCheckbox && fieldType.IsBoolean)
                return _element.Selected;

            return _element.Selected
                ? fieldType.GetValueFromString(_element.GetAttribute("value"))
                : null;
        }
    }
}
