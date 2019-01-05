using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ChameleonForms.AcceptanceTests.Helpers.Pages
{
    public static class PropertyExtensions
    {
        public static bool IsReadonly(this PropertyInfo property)
        {
            var attrs = property.GetCustomAttributes(typeof(ReadOnlyAttribute), false);
            if (attrs.Length > 0)
                return ((ReadOnlyAttribute)attrs[0]).IsReadOnly;
            return false;
        }

        public static string GetFormatStringForProperty(PropertyInfo property)
        {
            var displayFormatAttr = property.GetCustomAttributes(typeof(DisplayFormatAttribute), false);
            var format = "{0}";
            if (displayFormatAttr.Any())
                format = displayFormatAttr.Cast<DisplayFormatAttribute>().First().DataFormatString;
            return format;
        }
    }
}
