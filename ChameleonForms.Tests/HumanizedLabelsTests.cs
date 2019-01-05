using System.ComponentModel;

using Humanizer;
using NUnit.Framework;

namespace ChameleonForms.Tests
{
    [TestFixture]
    class HumanizedLabelsShould
    {
        [Test]
        public void Humanize_model_labels()
        {
            HumanizedLabels.Register();

            var m = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(NonHumanizedViewModel), "SomeFieldName");

            Assert.That(m.DisplayName, Is.EqualTo("Some field name"));
        }

        [Test]
        [TestCase(LetterCasing.AllCaps, "SOME FIELD NAME")]
        [TestCase(LetterCasing.LowerCase, "some field name")]
        [TestCase(LetterCasing.Sentence, "Some field name")]
        [TestCase(LetterCasing.Title, "Some Field Name")]
        public void Humanize_model_labels_with_custom_casing(LetterCasing casing, string expected)
        {
            HumanizedLabels.Register(casing);

            var m = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(NonHumanizedViewModel), "SomeFieldName");

            Assert.That(m.DisplayName, Is.EqualTo(expected));
        }

        [Test]
        public void Respect_explicit_display_name()
        {
            HumanizedLabels.Register();

            var m = ModelMetadataProviders.Current.GetMetadataForProperty(null, typeof(NonHumanizedViewModel), "FieldWithDisplayNameAttribute");

            Assert.That(m.DisplayName, Is.EqualTo("Existing display name"));
        }
    }

    internal class NonHumanizedViewModel
    {
        public string SomeFieldName { get; set; }

        [DisplayName("Existing display name")]
        public string FieldWithDisplayNameAttribute { get; set; }
    }
}
