using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders.DateTimeModelBinderTests
{
    [TestFixture(TypeArgs = new[]{typeof(DateTime)})]
    [TestFixture(TypeArgs = new[]{typeof(DateTime?)})]
    class DateTimeModelBinderShould<T> : ModelBinderTestBase<TestViewModel<T>>
    {
        [SetUp]
        public void Setup()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        [Test]
        public async Task Use_default_value_when_there_is_an_edit_format_but_no_value_submitted()
        {
            var (state, model) = await BindAndValidateAsync(m => m.DateTimeWithEditFormat);

            Assert.That(model, Is.EqualTo(default(T)));
            // Value required if DateTime, not if DateTime?
            Assert.That(state.IsValid, Is.EqualTo(typeof(T) == typeof(DateTime?)));
            if (typeof(T) == typeof(DateTime))
                AssertPropertyError(state, m => m.DateTimeWithEditFormat, $"The value '' is invalid.");
        }

        [Test]
        public async Task Use_default_value_when_there_is_an_edit_format_but_null_value_submitted([Values("", null, "  ")] string value)
        {
            var (state, model) = await BindAndValidateAsync(m => m.DateTimeWithEditFormat, value);

            Assert.That(model, Is.EqualTo(default(T)));
            // Value required if DateTime, not if DateTime?
            Assert.That(state.IsValid, Is.EqualTo(typeof(T) == typeof(DateTime?)));
            if (typeof(T) == typeof(DateTime))
                AssertPropertyError(state, m => m.DateTimeWithEditFormat, $"The value '{value}' is invalid.");
        }

        [Test]
        public async Task Use_default_value_and_add_error_when_there_is_a_display_format_and_an_invalid_value()
        {
            var (state, model) = await BindAndValidateAsync(m => m.DateTimeWithEditFormat, "invalid");

            Assert.That(model, Is.EqualTo(default(T)));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.DateTimeWithEditFormat, $"The value 'invalid' is not valid for DateTimeWithEditFormat. Format of date is dd/MM/yyyy.");
        }

        [Test]
        public async Task Use_default_value_and_add_error_when_there_is_a_display_format_and_an_invalid_format()
        {
            var (state, model) = await BindAndValidateAsync(m => m.DateTimeWithEditFormat, "2000-12-12");

            Assert.That(model, Is.EqualTo(default(T)));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.DateTimeWithEditFormat, $"The value '2000-12-12' is not valid for DateTimeWithEditFormat. Format of date is dd/MM/yyyy.");
        }

        [Test]
        public async Task Return_valid_value_if_value_ok()
        {
            var (state, model) = await BindAndValidateAsync(m => m.DateTimeWithEditFormat, "12/12/2000");

            Assert.That(model, Is.EqualTo(new DateTime(2000, 12, 12)));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task Return_valid_value_g_format_string([Values("en-GB", "uk-UA")]string culture)
        {
            using (ChangeCulture.To(culture))
            {
                var date = DateTime.UtcNow;
                var submittedValue = date.ToString("g");
                var expectedDate = DateTime.ParseExact(submittedValue, "g", CultureInfo.CurrentCulture);

                var (state, model) = await BindAndValidateAsync(m => m.DateTimeWithGFormat, submittedValue);

                Assert.That(model, Is.EqualTo(expectedDate));
                Assert.That(state.IsValid, Is.True);
            }
        }

        [Test]
        [TestCase("en-GB", "dd/MM/yyyy HH:mm")]
        [TestCase("uk-UA", "dd.MM.yyyy H:mm")]
        public async Task Use_default_value_and_add_error_with_g_format_string_and_an_invalid_format(string culture, string expectedDateFormat)
        {
            using (ChangeCulture.To(culture))
            {
                var (state, model) = await BindAndValidateAsync(m => m.DateTimeWithGFormat, "2020-02-20 11:00");

                Assert.That(model, Is.EqualTo(default(T)));
                Assert.That(state.IsValid, Is.False);
                AssertPropertyError(state, m => m.DateTimeWithGFormat, $"The value '2020-02-20 11:00' is not valid for DateTimeWithGFormat. Format of date is {expectedDateFormat}.");
            }
        }
    }
}
