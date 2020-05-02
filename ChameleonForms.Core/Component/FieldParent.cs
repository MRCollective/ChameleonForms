namespace ChameleonForms.Component
{
    /// <summary>
    /// The parent of the field being generated.
    /// </summary>
    public enum FieldParent
    {
        /// <summary>
        /// The parent is the root Form.
        /// </summary>
        Form,
        /// <summary>
        /// The parent is a section (or a field within a section).
        /// </summary>
        Section
    }
}