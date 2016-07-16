using System;
using System.ComponentModel.DataAnnotations;

namespace ChameleonForms.Attributes
{
    /// <summary>
    /// Marks a Flags enum property as required.
    /// </summary>
    public class RequiredFlagsEnumAttribute : RequiredAttribute
    {
        /// <inheritdoc />
        public override bool IsValid(object value)
        {
            if (Convert.ToInt64(value) == 0L)
                return false;

            return base.IsValid(value);
        }
    }
}
