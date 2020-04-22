using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders.UriModelBinderTests
{
    class UriModelBinderShould : ModelBinderTestBase<TestViewModel>
    {
        [Test]
        public async Task Use_valid_default_value_when_there_is_empty_value([Values("", null, "  ")] string value)
        {
            var (state, model) = await BindAsync(m => m.Uri, value);
            
            Assert.That(model, Is.EqualTo(null));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task Use_valid_default_value_when_there_is_no_value()
        {
            var (state, model) = await BindAsync(m => m.Uri);

            Assert.That(model, Is.EqualTo(null));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task Use_default_value_and_add_error_when_there_is_an_invalid_value()
        {
            var (state, model) = await BindAsync(m => m.Uri, "invalid");

            Assert.That(model, Is.EqualTo(null));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.Uri, "The value 'invalid' is not a valid URL for Uri.");
        }

        [Test]
        public async Task Use_default_value_and_add_error_when_there_is_a_non_http_uri_for_a_url_field()
        {
            var (state, model) = await BindAsync(m => m.UriAsUrl, "ftp://someserver.com/somepath");

            Assert.That(model, Is.EqualTo(null));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.UriAsUrl, "The value 'ftp://someserver.com/somepath' is not a valid HTTP(S) URL for UriAsUrl.");
        }

        [Test]
        public async Task Return_and_bind_valid_value_if_there_us_an_ok_http_value_for_a_url_field([Values("http://someserver.com/somepath", "https://someserver.com/somepath", "https://serverwithoutpath.io")] string okValue)
        {
            var (state, model) = await BindAsync(m => m.UriAsUrl, okValue);

            Assert.That(model, Is.EqualTo(new Uri(okValue)));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task Return_and_bind_valid_value_if_there_us_an_ok_uri_value_for_a_uri_field([Values("http://someserver.com/somepath", "https://someserver.com/somepath", "https://serverwithoutpath.io", "ftp://someserver/somefile", "c:\\somefile.txt")] string okValue)
        {
            var (state, model) = await BindAsync(m => m.Uri, okValue);

            Assert.That(model, Is.EqualTo(new Uri(okValue)));
            Assert.That(state.IsValid, Is.True);
        }
    }
}
