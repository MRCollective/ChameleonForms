namespace ChameleonForms.Enums
{
    /// <summary>
    /// The display type for a field control.
    /// </summary>
    public enum FieldDisplayType
    {
        /// <summary>
        /// The default display type for the field control.
        /// </summary>
        Default,
        /// <summary>
        /// Display the field as a list of checkboxes or radio button controls.
        /// </summary>
        List,
        /// <summary>
        /// Display the field as a drop-down control.
        /// </summary>
        DropDown,
        /// <summary>
        /// Display the field as a single line text input control.
        /// </summary>
        SingleLineText,
        /// <summary>
        /// Display the field as a file upload control.
        /// </summary>
        FileUpload,
        /// <summary>
        /// Display the field as a multi-line text input control.
        /// </summary>
        MultiLineText,
        /// <summary>
        /// Display the field as a single checkbox control.
        /// </summary>
        Checkbox,
        /// <summary>
        /// Display the field as a custom control.
        /// </summary>
        Custom
    }
}
