using System.ComponentModel;

namespace ChameleonForms.Templates.Bootstrap4
{
    /// <summary>
    /// Bootstrap 4 button styles: https://getbootstrap.com/docs/4.5/components/buttons/#examples
    /// </summary>
    public enum ButtonStyle
    {
        /// <summary>
        /// Default styling.
        /// </summary>
        Default,
        /// <summary>
        /// Primary styling.
        /// </summary>
        [Description("btn-primary")]
        Primary,
        /// <summary>
        /// Secondary styling.
        /// </summary>
        [Description("btn-secondary")]
        Secondary,
        /// <summary>
        /// Success styling.
        /// </summary>
        [Description("btn-success")]
        Success,
        /// <summary>
        /// Information styling.
        /// </summary>
        [Description("btn-info")]
        Info,
        /// <summary>
        /// Warning styling.
        /// </summary>
        [Description("btn-warning")]
        Warning,
        /// <summary>
        /// Danger styling.
        /// </summary>
        [Description("btn-primary")]
        Danger,
        /// <summary>
        /// Light styling.
        /// </summary>
        [Description("btn-light")]
        Light,
        /// <summary>
        /// Dark styling.
        /// </summary>
        [Description("btn-dark")]
        Dark,
        /// <summary>
        /// Primary outline styling.
        /// </summary>
        [Description("btn-outline-primary")]
        PrimaryOutline,
        /// <summary>
        /// Secondary outline styling.
        /// </summary>
        [Description("btn-outline-secondary")]
        SecondaryOutline,
        /// <summary>
        /// Success outline styling.
        /// </summary>
        [Description("btn-outline-success")]
        SuccessOutline,
        /// <summary>
        /// Information outline styling.
        /// </summary>
        [Description("btn-outline-info")]
        InfoOutline,
        /// <summary>
        /// Warning outline styling.
        /// </summary>
        [Description("btn-outline-warning")]
        WarningOutline,
        /// <summary>
        /// Danger outline styling.
        /// </summary>
        [Description("btn-outline-danger")]
        DangerOutline,
        /// <summary>
        /// Light outline styling.
        /// </summary>
        [Description("btn-outline-light")]
        LightOutline,
        /// <summary>
        /// Dark outline styling.
        /// </summary>
        [Description("btn-outline-dark")]
        DarkOutline,
        /// <summary>
        /// Link styling.
        /// </summary>
        [Description("btn-link")]
        Link
    }
}