using AngleSharp.Dom.Html;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields
{
    internal class TextInputField : IField
    {
        private readonly IHtmlInputElement _element;

        public TextInputField(IHtmlInputElement element)
        {
            _element = element;
        }

        public void Set(IModelFieldValue value)
        {
            _element.Value = value.Value;
        }

        public object Get(IModelFieldType fieldType)
        {
            return fieldType.GetValueFromString(_element.GetAttribute("value"));
        }
    }
}