namespace ChameleonForms
{
    /// <summary>
    /// Global ChameleonForms constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The key that is used to stash a ChameleonForm <see cref="Form{TModel}"/> within ViewData.
        /// </summary>
        public const string ViewDataFormKey = "ChameleonForms_Form";

        /// <summary>
        /// The key that is used to stash a ChameleonForm <see cref="Form{TModel}"/> within ViewData.
        /// </summary>
        public const string ViewDataSectionKey = "ChameleonForms_Section";

        /// <summary>
        /// The key that is used to stash a ChameleonForm <see cref="Component.Navigation{TModel}"/> within ViewData.
        /// </summary>
        public const string ViewDataNavigationKey = "ChameleonForms_Navigation";

        /// <summary>
        /// The key that is used to stash a ChameleonForm <see cref="Component.Field{TModel}"/> within ViewData.
        /// </summary>
        public const string ViewDataFieldKey = "ChameleonForms_Field";

        /// <summary>
        /// The key that is used to stash a ChameleonForm <see cref="Component.Message{TModel}"/> within ViewData.
        /// </summary>
        public const string ViewDataMessageKey = "ChameleonForms_Message";

    }
}
