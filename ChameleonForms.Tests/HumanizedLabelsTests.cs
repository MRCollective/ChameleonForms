using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ChameleonForms.Tests.Helpers;
using Humanizer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ChameleonForms.Tests
{
    [TestFixture]
    class HumanizedLabelsShould
    {
        private MvcTestContext _scope;

        [SetUp]
        public void Setup()
        {
            _scope = new MvcTestContext();
        }

        [TearDown]
        public void TearDown()
        {
            _scope.Dispose();
        }

        private ModelMetadata GetMetadataFor<TProperty>(Expression<Func<NonHumanizedViewModel, TProperty>> property, IStringTransformer to = null)
        {
            var provider = _scope.Services.GetRequiredService<ModelMetadataDetailsProviderProvider>();
            provider.DisplayMetadataProviders.Add(new HumanizedLabelsDisplayMetadataProvider(to));

            var metadataProvider = _scope.Services.GetRequiredService<IModelMetadataProvider>();
            return metadataProvider.GetMetadataForProperty(typeof(NonHumanizedViewModel), ((MemberExpression)property.Body).Member.Name);
        }

        [Test]
        public void Humanize_model_labels_without_attributes()
        {
            var metadata = GetMetadataFor(m => m.FieldWithNoAttributes);

            Assert.That(metadata.DisplayName, Is.EqualTo("Field with no attributes"));
        }

        [Test]
        public void Humanize_model_labels_with_only_display_attribute_without_name()
        {
            var metadata = GetMetadataFor(m => m.FieldWithDisplayAttributeWithoutName);

            Assert.That(metadata.DisplayName, Is.EqualTo("Field with display attribute without name"));
        }

        [Test]
        [TestCase(LetterCasing.AllCaps, "SOME FIELD NAME")]
        [TestCase(LetterCasing.LowerCase, "some field name")]
        [TestCase(LetterCasing.Sentence, "Some field name")]
        [TestCase(LetterCasing.Title, "Some Field Name")]
        public void Humanize_model_labels_with_custom_casing(LetterCasing casing, string expected)
        {
            ModelMetadata metadata;
            switch (casing)
            {
                case LetterCasing.AllCaps:
                    metadata = GetMetadataFor(m => m.FieldWithNoAttributes, To.UpperCase);
                    break;
                case LetterCasing.LowerCase:
                    metadata = GetMetadataFor(m => m.FieldWithNoAttributes, To.LowerCase);
                    break;
                case LetterCasing.Sentence:
                    metadata = GetMetadataFor(m => m.FieldWithNoAttributes, To.SentenceCase);
                    break;
                case LetterCasing.Title:
                default:
                    metadata = GetMetadataFor(m => m.FieldWithNoAttributes, To.TitleCase);
                    break;
            }

            Assert.That(metadata.DisplayName, Is.EqualTo(expected));
        }

        [Test]
        public void Respect_explicit_displayname()
        {
            var metadata = GetMetadataFor(m => m.FieldWithDisplayNameAttribute);

            Assert.That(metadata.DisplayName, Is.EqualTo("Explicit display name"));
        }

        [Test]
        public void Respect_explicit_display_name()
        {
            var metadata = GetMetadataFor(m => m.FieldWithDisplayAttributeWithName);

            Assert.That(metadata.DisplayName, Is.EqualTo("Explicit display name"));
        }
    }

    internal class NonHumanizedViewModel
    {
        public string FieldWithNoAttributes { get; set; }

        [DisplayName("Explicit display name")]
        public string FieldWithDisplayNameAttribute { get; set; }

        [Display(Name = "Explicit display name")]
        public string FieldWithDisplayAttributeWithName { get; set; }

        [Display(Description = "Description")]
        public string FieldWithDisplayAttributeWithoutName { get; set; }
    }
}
