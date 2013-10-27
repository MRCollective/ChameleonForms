using System.ComponentModel;

namespace ChameleonForms.Templates.TwitterBootstrap3
{
    /// <summary>
    /// Twitter Bootstrap button sizes: http://getbootstrap.com/css/#buttons-sizes
    /// </summary>
    public enum ButtonSize
    {
        /// <summary>
        /// Extra small button size.
        /// </summary>
        [Description("xs")]
        ExtraSmall,
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
