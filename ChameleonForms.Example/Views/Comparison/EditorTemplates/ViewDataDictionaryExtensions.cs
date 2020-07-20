using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ChameleonForms.Example.Views.Comparison.EditorTemplates
{
    public static class ViewDataDictionaryExtensions
    {
        public static Dictionary<string, object> GetFieldAttrs(this ViewDataDictionary viewData)
        {
            var attrs = viewData["attrs"] as Dictionary<string, object> ?? new Dictionary<string, object>();
            viewData["attrs"] = attrs;
            return attrs;
        }

        public static Dictionary<string, object> SetupFieldAttrs(this ViewDataDictionary viewData)
        {
            var attrs = viewData.GetFieldAttrs();
            if (viewData["hint"] != null)
            {
                attrs["aria-describedby"] = $"{viewData.ModelMetadata.Name}--Hint";
            }
            if (viewData.ModelMetadata.IsRequired)
            {
                attrs["required"] = "required";
            }
            return attrs;
        }
    }
}
