using NUnit.Framework;

namespace ChameleonForms.Tests
{
    class ExtensionsShould
    {
        [Test]
        public void Leave_string_alone_when_converting_string_with_no_special_characters_to_html()
        {
            const string str = "asDS f123$";

            var h = str.ToHtml();

            Assert.That(h.ToHtmlString(), Is.EqualTo(str));
        }

        [Test]
        public void Encode_special_characters_when_converting_string_with_special_characters_to_html()
        {
            const string str = "as<D>Sf&1\"2 3$";

            var h = str.ToHtml();

            Assert.That(h.ToHtmlString(), Is.EqualTo("as&lt;D&gt;Sf&amp;1&quot;2 3$"));
        }

        [Test]
        public void Encode_null_or_empty_string_as_empty_html([Values(null, "")] string str)
        {
            var h = str.ToHtml();

            Assert.That(h.ToHtmlString(), Is.EqualTo(string.Empty));
        }
    }
}
