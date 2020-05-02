using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace ChameleonForms.Tests
{
    [TestFixture]
    class HumanizedLabelsShould
    {
        private HumanizedLabelsDisplayMetadataProvider _provider;
        private ServiceProvider _scope;

        [SetUp]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMvc();
            serviceCollection.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            serviceCollection.Configure<MvcOptions>(x =>
            {
                x.ModelMetadataDetailsProviders.Add(new AnotherModelMetadataProvider());
                x.ModelMetadataDetailsProviders.Add(_provider);
            });
            _scope = serviceCollection.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            _scope.Dispose();
        }

        private ModelMetadata GetMetadataFor<TProperty>(Expression<Func<NonHumanizedViewModel, TProperty>> property, IStringTransformer to = null)
        {
            _provider = new HumanizedLabelsDisplayMetadataProvider(to);
            var metadataProvider = _scope.GetRequiredService<IModelMetadataProvider>();
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
        [TestCase(LetterCasing.AllCaps, "FIELD WITH NO ATTRIBUTES")]
        [TestCase(LetterCasing.LowerCase, "field with no attributes")]
        [TestCase(LetterCasing.Sentence, "Field with no attributes")]
        [TestCase(LetterCasing.Title, "Field With No Attributes")]
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
                // ReSharper disable once RedundantCaseLabel
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

        [Test]
        public void Respect_display_name_from_another_provider()
        {
            var metadata = GetMetadataFor(m => m.FieldWithDisplayNameFromAnotherMetadataProvider);

            Assert.That(metadata.DisplayName, Is.EqualTo(AnotherModelMetadataProvider.DisplayName));
        }

        [Test]
        public void Respect_simple_display_property_name_from_another_provider()
        {
            var metadata = GetMetadataFor(m => m.FieldWithSimpleDisplayPropertyFromAnotherMetadataProvider);

            Assert.That(metadata.DisplayName, Is.EqualTo(null));
            Assert.That(metadata.SimpleDisplayProperty, Is.EqualTo(AnotherModelMetadataProvider.DisplayName));
        }

        [Test]
        public void Humanize_label_when_another_provider_results_in_null_display_name()
        {
            var metadata = GetMetadataFor(m => m.FieldWithNullDisplayNameFromAnotherMetadataProvider);

            Assert.That(metadata.DisplayName, Is.EqualTo("Field with null display name from another metadata provider"));
        }
    }

    class AnotherModelMetadataProvider : IDisplayMetadataProvider
    {
        public const string DisplayName = "Another display name";

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (context.Key.Name == nameof(NonHumanizedViewModel.FieldWithSimpleDisplayPropertyFromAnotherMetadataProvider))
            {
                context.DisplayMetadata.SimpleDisplayProperty = DisplayName;
            }

            if (context.Key.Name == nameof(NonHumanizedViewModel.FieldWithDisplayNameFromAnotherMetadataProvider))
            {
                context.DisplayMetadata.DisplayName = () => DisplayName;
            }

            if (context.Key.Name == nameof(NonHumanizedViewModel.FieldWithNullDisplayNameFromAnotherMetadataProvider))
            {
                context.DisplayMetadata.DisplayName = () => null;
            }
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

        public string FieldWithDisplayNameFromAnotherMetadataProvider { get; set; }
        public string FieldWithSimpleDisplayPropertyFromAnotherMetadataProvider { get; set; }
        public string FieldWithNullDisplayNameFromAnotherMetadataProvider { get; set; }
    }
}
