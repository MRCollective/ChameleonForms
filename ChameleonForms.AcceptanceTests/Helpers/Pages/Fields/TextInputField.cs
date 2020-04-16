using System;
using AngleSharp.Html.Dom;

namespace ChameleonForms.AcceptanceTests.Helpers.Pages.Fields
{
    internal class TextInputField : IField
    {
        private readonly Func<string> _getter;
        private readonly Action<string> _setter;

        public TextInputField(IHtmlInputElement element)
        {
            _getter = () => element.Value;
            _setter = (value) => element.Value = value;
        }

        public TextInputField(IHtmlTextAreaElement element)
        {
            _getter = () => element.Value;
            _setter = (value) => element.Value = value;
        }

        public void Set(IModelFieldValue value)
        {
            _setter(value.Value);
        }

        public object Get(IModelFieldType fieldType)
        {
            return fieldType.GetValueFromString(_getter());
        }
    }
}