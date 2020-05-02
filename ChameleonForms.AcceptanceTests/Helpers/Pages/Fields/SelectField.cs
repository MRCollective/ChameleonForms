using System.Linq;
using AngleSharp.Html.Dom;

namespace ChameleonForms.AcceptanceTests.Helpers.Pages.Fields
{
    internal class SelectField : IField
    {
        private readonly IHtmlSelectElement _select;

        public SelectField(IHtmlSelectElement element)
        {
            _select = element;
        }

        public void Set(IModelFieldValue value)
        {
            _select.Options.ToList().ForEach(o => o.IsSelected = false);

            if (!_select.IsMultiple)
            {
                _select.Options.Single(o => o.Value == value.Value).IsSelected = true;
                return;
            }

            if (value.HasMultipleValues)
                foreach (var selectedValue in value.Values)
                    _select.Options.Single(o => o.Value == selectedValue).IsSelected = true;
            else
                _select.Options.Single(o => o.Value == value.Value).IsSelected = true;
        }

        public object Get(IModelFieldType fieldType)
        {
            if (_select.IsMultiple)
            {
                return fieldType.GetValueFromStrings(_select.SelectedOptions.Select(o => o.GetAttribute("value")));
            }

            return fieldType.GetValueFromString(_select.SelectedOptions.Single().GetAttribute("value"));
        }
    }
}
