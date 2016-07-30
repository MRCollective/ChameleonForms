using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields
{
    internal class MultipleFields : IField
    {
        private readonly IList<IWebElement> _elements;

        public MultipleFields(IList<IWebElement> elements)
        {
            _elements = elements;
        }

        public void Set(IModelFieldValue value)
        {
            foreach (var element in _elements)
            {
                FieldFactory.Create(new[] {element}).Set(value);
            }
        }

        public object Get(IModelFieldType fieldType)
        {
            var values = _elements
                .Select(e => FieldFactory.Create(new[] {e}).Get(new ModelFieldType(fieldType.BaseType, fieldType.Format)))
                .Where(e => e != null)
                .ToArray();

            if (fieldType.IsFlagsEnum)
            {
                if (values.Length == 1)
                    return values.First();
                return fieldType.GetValueFromStrings(values.Select(v => v.ToString()));
            }

            if (fieldType.IsEnumerable)
                return fieldType.Cast(values);

            return values.FirstOrDefault() ?? fieldType.DefaultValue;
        }
    }
}