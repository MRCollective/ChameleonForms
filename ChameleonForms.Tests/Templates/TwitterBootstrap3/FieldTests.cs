using System.Web;

using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component.Config;
using ChameleonForms.Templates.TwitterBootstrap3;
using ChameleonForms.Tests.FieldGenerator;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using NUnit.Framework;
using static ChameleonForms.Tests.HtmlHelperExtensionsShould;

namespace ChameleonForms.Tests.Templates.TwitterBootstrap3
{
    [UseReporter(typeof(DiffReporter))]
    class FieldTests_TwitterBootstrapTemplateShould
    {
        [Test]
        public void Output_field()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration(), false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_container_class()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration().AddFieldContainerClass("a-container-class-1").AddFieldContainerClass("a-container-class-2"), false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_hint()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration().WithHint("hello"), false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration()
                .Prepend(new HtmlString("<1>")).Prepend(new HtmlString("<2>"))
                .Append(new HtmlString("<3>")).Append(new HtmlString("<4>"))
                .WithHint(new HtmlString("<hint>")),
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html_when_input_group()
        {
            var t = new TwitterBootstrapFormTemplate();
            var metadata = new DefaultModelMetadata(new EmptyModelMetadataProvider(), new DefaultCompositeMetadataDetailsProvider(new IMetadataDetailsProvider[0]), new DefaultMetadataDetails(ModelMetadataIdentity.ForProperty(typeof(bool), nameof(TestFieldViewModel.OptionalBooleanField), typeof(TestFieldViewModel)), ModelAttributes.GetAttributesForProperty(typeof(bool), typeof(TestFieldViewModel).GetProperty(nameof(TestFieldViewModel.OptionalBooleanField)), typeof(bool))));

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), metadata, new FieldConfiguration()
                .Prepend(new HtmlString("<1>")).Prepend(new HtmlString("<2>"))
                .Append(new HtmlString("<3>")).Append(new HtmlString("<4>"))
                .WithHint(new HtmlString("<hint>"))
                .AsInputGroup(), // This shouldn't take effect since we haven't specified this field can be an input group
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html_when_allowed_input_group()
        {
            var t = new TwitterBootstrapFormTemplate();

            DefaultMetadataDetails details = new DefaultMetadataDetails(ModelMetadataIdentity.ForType(typeof(string)), ModelAttributes.GetAttributesForType(typeof(string)));
            details.ValidationMetadata = new ValidationMetadata
            {
                IsRequired = true
            };

            var modelMetadata = new DefaultModelMetadata(new EmptyModelMetadataProvider(), new DefaultCompositeMetadataDetailsProvider(new IMetadataDetailsProvider[0]), details);

            var fc = new FieldConfiguration();
            fc.Bag.CanBeInputGroup = true;

            var result = t.Field(new HtmlString("labelhtml")
                , new HtmlString("elementhtml")
                , new HtmlString("validationhtml")
                , modelMetadata
                , fc
                    .Prepend(new HtmlString("<1>"))
                    .Prepend(new HtmlString("<2>"))
                    .Append(new HtmlString("<3>"))
                    .Append(new HtmlString("<4>"))
                    .WithHint(new HtmlString("<hint>"))
                    .AsInputGroup() // This shouldn't take effect since we haven't specified this field can be an input group
                , false
                );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html_when_required()
        {
            var t = new TwitterBootstrapFormTemplate();
            var metadata = new DefaultModelMetadata(new EmptyModelMetadataProvider(), new DefaultCompositeMetadataDetailsProvider(new IMetadataDetailsProvider[0]), new DefaultMetadataDetails(ModelMetadataIdentity.ForProperty(typeof(string), nameof(TestFieldViewModel.RequiredString), typeof(TestFieldViewModel)), ModelAttributes.GetAttributesForProperty(typeof(string), typeof(TestFieldViewModel).GetProperty(nameof(TestFieldViewModel.RequiredString)), typeof(string))));
            //metadata.IsRequired = true;

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration()
                .Prepend(new HtmlString("<1>")).Prepend(new HtmlString("<2>"))
                .Append(new HtmlString("<3>")).Append(new HtmlString("<4>"))
                .WithHint(new HtmlString("<hint>")),
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_checkbox_field_with_prepended_and_appended_html_when_required()
        {
            var t = new TwitterBootstrapFormTemplate();
            var fc = new FieldConfiguration();
            fc.Bag.IsCheckboxControl = true;

            DefaultMetadataDetails details = new DefaultMetadataDetails(ModelMetadataIdentity.ForType(typeof(string)), ModelAttributes.GetAttributesForType(typeof(string)));
            details.ValidationMetadata = new ValidationMetadata
            {
                IsRequired = true
            };

            var modelMetadata = new DefaultModelMetadata(new EmptyModelMetadataProvider(), new DefaultCompositeMetadataDetailsProvider(new IMetadataDetailsProvider[0]), details);

            var result = t.Field(new HtmlString("labelhtml")
                , new HtmlString("elementhtml")
                , new HtmlString("validationhtml")
                , modelMetadata
                , fc
                    .Prepend(new HtmlString("<1>"))
                    .Prepend(new HtmlString("<2>"))
                    .Append(new HtmlString("<3>"))
                    .Append(new HtmlString("<4>"))
                    .WithHint(new HtmlString("<hint>"))
                , false
                );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_radio_list_field_with_prepended_and_appended_html_when_required()
        {
            var t = new TwitterBootstrapFormTemplate();
            var fc = new FieldConfiguration();
            fc.Bag.IsRadioOrCheckboxList = true;

            DefaultMetadataDetails details = new DefaultMetadataDetails(ModelMetadataIdentity.ForType(typeof(string)), ModelAttributes.GetAttributesForType(typeof(string)));
            details.ValidationMetadata = new ValidationMetadata
            {
                IsRequired = true
            };

            var modelMetadata = new DefaultModelMetadata(new EmptyModelMetadataProvider(), new DefaultCompositeMetadataDetailsProvider(new IMetadataDetailsProvider[0]), details);

            var result = t.Field(new HtmlString("labelhtml")
                , new HtmlString("elementhtml")
                , new HtmlString("validationhtml")
                , modelMetadata
                , fc
                    .Prepend(new HtmlString("<1>"))
                    .Prepend(new HtmlString("<2>"))
                    .Append(new HtmlString("<3>"))
                    .Append(new HtmlString("<4>"))
                    .WithHint(new HtmlString("<hint>"))
                , false
                );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_field()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.BeginField(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration(), false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_end_field()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.EndField();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

    }
}
