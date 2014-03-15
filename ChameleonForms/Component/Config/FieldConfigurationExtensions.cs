namespace ChameleonForms.Component.Config
{
    /// <summary>
    /// Provides additional configuration options to FieldConfiguration
    /// </summary>
    public static class FieldConfigurationExtensions
    {
        /// <summary>
        /// Sets the tab index of a given field
        /// </summary>
        /// <param name="config">Field configuration to update</param>
        /// <param name="index">Tab index to be set</param>
        /// <returns>The instance of IFieldConfiguration passed in to continue chaining things</returns>
        public static IFieldConfiguration TabIndex(this IFieldConfiguration config, int index)
        {
            if (config != null)
                return config.Attr("tabindex", index);
            return null;
        }

        /// <summary>
        /// Applys the autofocus attribute to a given field
        /// </summary>
        /// <param name="config">Field configuration to modify</param>
        /// <returns>The instance of IFieldConfiguration passed in to continue chaining things</returns>
        public static IFieldConfiguration AutoFocus(this IFieldConfiguration config)
        {
            if (config != null)
                return config.Attr("autofocus", "autofocus");
            return null;
        }
    }
}