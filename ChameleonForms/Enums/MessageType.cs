namespace ChameleonForms.Enums
{
    /// <summary>
    /// Types of messages that can be displayed to the user
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// User action required.
        /// </summary>
        Action,

        /// <summary>
        /// Action successful.
        /// </summary>
        Success,

        /// <summary>
        /// Action failed.
        /// </summary>
        Failure,

        /// <summary>
        /// Informational message.
        /// </summary>
        Information,

        /// <summary>
        /// Warning message.
        /// </summary>
        Warning
    }
}