using System.ComponentModel;

namespace ChameleonForms.Templates.Bootstrap4
{
    /// <summary>
    /// Bootstrap 4 button sizes: https://getbootstrap.com/docs/4.5/components/buttons/#sizes
    /// </summary>
    public enum ButtonSize
    {
        /// <summary>
        /// None specified.
        /// </summary>
        [Description("")]
        NoneSpecified,
        /// <summary>
        /// Small button size.
        /// </summary>
        [Description("sm")]
        Small,
        /// <summary>
        /// Default button size.
        /// </summary>
        Default,
        /// <summary>
        /// Large button size.
        /// </summary>
        [Description("lg")]
        Large
    }
}
