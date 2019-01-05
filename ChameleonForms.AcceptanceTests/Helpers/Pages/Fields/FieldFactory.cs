using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using System.Collections.Generic;
using System.Linq;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields
{
    internal static class FieldFactory
    {
        private static IField NoField
        {
            get
            {
                return new NoField();
            }
        }

        public static IField Create(IEnumerable<IElement> elements)
        {
            var elementsList = elements.ToList();

            if (elements == null || !elementsList.Any())
                return NoField;

            if (elementsList.Count() > 1)
                return new MultipleFields(elementsList);

            var element = elementsList[0];
            var tagName = element.TagName.ToLower();
            var type = element.GetAttribute("type")?.ToLower();

            if (tagName == "input" && (type == "checkbox" || type == "radio"))
                return new BinaryInputField((IHtmlInputElement)element);

            if (tagName == "input" || tagName == "textarea")
                return new TextInputField((IHtmlInputElement)element);

            if (tagName == "select")
                return new SelectField((IHtmlSelectElement)element);

            return NoField;
        }
    }
}