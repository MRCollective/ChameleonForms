using System.Collections.Generic;
using System.ComponentModel;

namespace ChameleonForms.Utils
{
    /// <summary>
    /// Extension methods on object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Creates a dictionary of HTML attributes from the input object, 
        /// translating underscores to dashes.
        /// </summary>
        /// <example>
        /// new <c>{ data_name="value" }</c> will translate to the entry <c>{ "data-name" , "value" }</c>
        /// in the resulting dictionary.
        /// </example>
        /// <param name="htmlAttributes">Anonymous object describing HTML attributes.</param>
        /// <returns>A dictionary that represents HTML attributes.</returns>
        public static IDictionary<string, object> AnonymousObjectToHtmlAttributes(this object htmlAttributes)
        {
            var result = new Dictionary<string, object>();

            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    result.Add(property.Name.Replace('_', '-'), property.GetValue(htmlAttributes));
                }
            }

            return result;
        }
    }
}
