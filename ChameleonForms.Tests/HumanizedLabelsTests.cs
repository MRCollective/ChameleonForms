using System.ComponentModel;
using System.Web.Mvc;
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
            var model = new NonHumanizedViewModel();

            var m = ModelMetadataProviders.Current.GetMetadataForProperty(() => model, typeof(NonHumanizedViewModel), "SomeFieldName");

            Assert.That(m.DisplayName, Is.EqualTo("Some field name"));
        }

        [Test]
        public void Respect_explicit_display_name()
        {
            HumanizedLabels.Register();
            var model = new NonHumanizedViewModel();

            var m = ModelMetadataProviders.Current.GetMetadataForProperty(() => model, typeof(NonHumanizedViewModel), "FieldWithDisplayNameAttribute");

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
