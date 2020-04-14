using System.ComponentModel;

namespace ChameleonForms.Enums
{
    ///<summary>
    /// Representation of the different form encoding types.
    /// Use .Humanize() to get the enc type for output.
    ///</summary>
    public enum EncType
    {
        ///<summary>
        /// URL encoded
        ///</summary>
        [Description("application/x-www-form-urlencoded")]
        UrlEncoded,
        ///<summary>
        /// Multipart
        ///</summary>
        [Description("multipart/form-data")]
        Multipart,
        ///<summary>
        /// Plain text
        ///</summary>
        [Description("text/plain")]
        Plain
    }
}
