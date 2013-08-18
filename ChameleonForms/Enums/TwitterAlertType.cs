namespace ChameleonForms.Enums
{
    /// <summary>
    /// Twitter Bootstrap alert/emphasis colors: http://getbootstrap.com/css/#type-emphasis
    /// </summary>
    public enum TwitterAlertType
    {
        Muted,
        Primary,
        Success,
        Info,
        Warning,
        Danger,
    }

    internal static class TwitterAlertTypeConversion
    {
        internal static TwitterAlertType ToTwitterAlertType(this MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Action: return TwitterAlertType.Primary;
                case MessageType.Success: return TwitterAlertType.Success;
                case MessageType.Failure: return TwitterAlertType.Danger;
                case MessageType.Information: return TwitterAlertType.Info;
                case MessageType.Warning: return TwitterAlertType.Warning;
                // There is no case for muted in Chameleon...
                default: return TwitterAlertType.Muted;
            }
        }
    }
}