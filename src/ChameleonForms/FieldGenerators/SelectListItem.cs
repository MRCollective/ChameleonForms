namespace ChameleonForms.FieldGenerators
{
    /// <summary>
    /// Represents an item in a select list.
    /// </summary>
    public class SelectListItem
    {
        /// <summary>
        /// Whether or not the item is disabled.
        /// </summary>
        public bool Disabled { get; set; }
        
        /// <summary>
        /// Whether or not the item is selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// The text representation of the item.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The value of the item.
        /// </summary>
        public string Value { get; set; }
    }
}
