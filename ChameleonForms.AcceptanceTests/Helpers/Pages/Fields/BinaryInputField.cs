using System;
using System.Linq;
using AngleSharp.Html.Dom;

namespace ChameleonForms.AcceptanceTests.Helpers.Pages.Fields
{
    internal class BinaryInputField : IField
    {
        private readonly IHtmlInputElement _element;
        private readonly bool _isCheckbox;

        public BinaryInputField(IHtmlInputElement element)
        {
            _element = element;
            _isCheckbox = element.GetAttribute("type").ToLower() == "checkbox";
        }

        public void Set(IModelFieldValue value)
        {
            // De-check everything to start with a clean slate
            _element.IsChecked = false;

            if (value.HasMultipleValues)
            {
                if (value.Values.Contains(_element.Value, StringComparer.InvariantCultureIgnoreCase))
                    _element.IsChecked = true;
            }
            else
            {
                // Check a single checkbox wrapping a true boolean field value
                if (_isCheckbox && value.IsTrue)
                    _element.IsChecked = true;
                else if (_element.Value.Equals(value.Value, StringComparison.InvariantCultureIgnoreCase))
                    _element.IsChecked = true;
            }
        }

        public object Get(IModelFieldType fieldType)
        {
            if (_isCheckbox && fieldType.IsBoolean)
                return _element.IsChecked;

            return _element.IsChecked
                ? fieldType.GetValueFromString(_element.GetAttribute("value"))
                : null;
        }
    }
}
