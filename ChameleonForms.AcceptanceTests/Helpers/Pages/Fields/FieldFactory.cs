﻿using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace ChameleonForms.AcceptanceTests.Helpers.Pages.Fields
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

            if (tagName == "input")
                return new TextInputField((IHtmlInputElement)element);

            if (tagName == "textarea")
                return new TextInputField((IHtmlTextAreaElement)element);

            if (tagName == "select")
                return new SelectField((IHtmlSelectElement)element);

            return NoField;
        }
    }
}