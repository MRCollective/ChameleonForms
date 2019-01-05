using System;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields
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

        //public void Set(IModelFieldValue value)
        //{
        //    // Deselect if selected checkbox
        //    if (_isCheckbox && _element.IsSelected)
        //        _element.DoClick();

        //    if (value.HasMultipleValues)
        //    {
        //        if (value.Values.Contains(_element.GetAttribute("value"), StringComparer.InvariantCultureIgnoreCase))
        //            _element.DoClick();
        //    }
        //    else
        //    {
        //        // Check a single checkbox wrapping a true boolean field value
        //        if (_isCheckbox && value.IsTrue)
        //            _element.DoClick();
        //        else if (_element.GetAttribute("value").Equals(value.Value, StringComparison.InvariantCultureIgnoreCase))
        //            _element.DoClick();
        //    }
        //}

        public object Get(IModelFieldType fieldType)
        {
            return fieldType.GetValueFromString(_element.Value);
            //if (_isCheckbox && fieldType.IsBoolean)
            //    return _element.IsSelected;

            //return _element.IsSelected
            //    ? fieldType.GetValueFromString(_element.GetAttribute("value"))
            //    : null;
        }
    }
}
