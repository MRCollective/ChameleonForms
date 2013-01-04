using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ChameleonForms.Tests.ModelBinding.Pages.Fields
{
    internal class SelectField : IField
    {
        private readonly SelectElement _select;

        public SelectField(IWebElement element)
        {
            _select = new SelectElement(element);
        }

        public void Set(IModelFieldValue value)
        {
            if (!_select.IsMultiple)
            {
                _select.SelectByValue(value.Value);
                return;
            }

            _select.DeselectAll();
            if (value.HasMultipleValues)
                foreach (var selectedValue in value.Values)
                    _select.SelectByValue(selectedValue);
            else
                _select.SelectByValue(value.Value);
        }

        public object Get(IModelFieldType fieldType)
        {
            if (_select.IsMultiple)
                return fieldType.GetValueFromStrings(_select.AllSelectedOptions.Select(o => o.GetAttribute("value")));

            return fieldType.GetValueFromString(_select.SelectedOption.GetAttribute("value"));
        }
    }
}
