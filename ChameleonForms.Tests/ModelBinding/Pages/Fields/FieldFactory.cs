using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace ChameleonForms.Tests.ModelBinding.Pages.Fields
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

        public static IField Create(IEnumerable<IWebElement> elements)
        {
            var elementsList = elements.ToList();

            if (elements == null || !elementsList.Any())
                return NoField;

            if (elementsList.Count() > 1)
                return new MultipleFields(elementsList);

            var element = elementsList[0];
            var tagName = element.TagName.ToLower();
            var type = element.GetAttribute("type").ToLower();

            if (tagName == "input" && (type == "checkbox" || type == "radio"))
                return new BinaryInputField(element);

            if (tagName == "input" || tagName == "textarea")
                return new TextInputField(element);

            if (tagName == "select")
                return new SelectField(element);

            return NoField;
        }
    }
}