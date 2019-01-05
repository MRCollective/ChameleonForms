using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields
{
    internal class MultipleFields : IField
    {
        private readonly IEnumerable<IElement> _elements;

        public MultipleFields(IEnumerable<IElement> elements)
        {
            _elements = elements;
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