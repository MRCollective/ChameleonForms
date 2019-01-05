using System.Linq;
using AngleSharp.Dom.Html;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields
{
    internal class SelectField : IField
    {
        private readonly IHtmlSelectElement _select;

        public SelectField(IHtmlSelectElement element)
        {
            _select = element;
        }

        //public void Set(IModelFieldValue value)
        //{
        //    if (!_select.IsMultiple)
        //    {
        //        _select.SelectByValue(value.Value);
        //        return;
        //    }

        //    _select.DeselectAll();
        //    if (value.HasMultipleValues)
        //        foreach (var selectedValue in value.Values)
        //            _select.SelectByValue(selectedValue);
        //    else
        //        _select.SelectByValue(value.Value);
        //}

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
