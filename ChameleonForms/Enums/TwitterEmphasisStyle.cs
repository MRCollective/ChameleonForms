namespace ChameleonForms.Enums
{
    /// <summary>
    /// Twitter Bootstrap alert/emphasis colors: http://getbootstrap.com/css/#type-emphasis
    /// </summary>
    public enum TwitterEmphasisStyle
    {
        /// <summary>
        /// Default styling.
        /// </summary>
        Default,
        /// <summary>
        /// Primary action styling.
        /// </summary>
        Primary,
        /// <summary>
        /// Success styling.
        /// </summary>
        Success,
        /// <summary>
        /// Information styling.
        /// </summary>
        Info,
        /// <summary>
        /// Warning styling.
        /// </summary>
        Warning,
        /// <summary>
        /// Danger styling.
        /// </summary>
        Danger
    }

    internal static class TwitterAlertTypeConversion
    {
        internal static TwitterEmphasisStyle ToTwitterAlertType(this MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Action: return TwitterEmphasisStyle.Primary;
                case MessageType.Success: return TwitterEmphasisStyle.Success;
                case MessageType.Failure: return TwitterEmphasisStyle.Danger;
                case MessageType.Information: return TwitterEmphasisStyle.Info;
                case MessageType.Warning: return TwitterEmphasisStyle.Warning;
                default: return TwitterEmphasisStyle.Default;
            }
        }
    }
}