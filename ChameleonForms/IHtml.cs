namespace ChameleonForms
{
    /// <summary>Represents an HTML-encoded string that should not be encoded again.</summary>
    public interface IHtml
    {
        /// <summary>Returns an HTML-encoded string.</summary>
        /// <returns>An HTML-encoded string.</returns>
        string ToHtmlString();
    }

    /// <summary>
    /// Converts a HTML-safe string to a <see cref="IHtml" />.
    /// </summary>
    public class Html : IHtml
    {
        private readonly string _htmlSafeString;

        /// <summary>
        /// Creates a <see cref="Html"/>.
        /// </summary>
        /// <param name="htmlSafeString">A HTML-safe string</param>
        public Html(string htmlSafeString)
        {
            _htmlSafeString = htmlSafeString;
        }

        /// <inheritdoc />
        public string ToHtmlString()
        {
            return _htmlSafeString;
        }
    }
}
