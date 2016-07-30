using System.Web.Mvc;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component.Config;
using ChameleonForms.Templates.TwitterBootstrap3;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates.TwitterBootstrap3
{
    [UseReporter(typeof(DiffReporter))]
    class FieldTests_TwitterBootstrapTemplateShould
    {
        [Test]
        public void Output_field()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new Html("labelhtml"), new Html("elementhtml"), new Html("validationhtml"), null, new FieldConfiguration(), false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_container_class()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new Html("labelhtml"), new Html("elementhtml"), new Html("validationhtml"), null, new FieldConfiguration().AddFieldContainerClass("a-container-class-1").AddFieldContainerClass("a-container-class-2"), false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_hint()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new Html("labelhtml"), new Html("elementhtml"), new Html("validationhtml"), null, new FieldConfiguration().WithHint("hello"), false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new Html("labelhtml"), new Html("elementhtml"), new Html("validationhtml"), null, new FieldConfiguration()
                .Prepend(new Html("<1>")).Prepend(new Html("<2>"))
                .Append(new Html("<3>")).Append(new Html("<4>"))
                .WithHint(new Html("<hint>")),
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html_when_input_group()
        {
            var t = new TwitterBootstrapFormTemplate();
            var metadata = Substitute.For<IFieldMetadata>();
            metadata.IsRequired.Returns(true);

            var result = t.Field(new Html("labelhtml"), new Html("elementhtml"), new Html("validationhtml"), metadata, new FieldConfiguration()
                .Prepend(new Html("<1>")).Prepend(new Html("<2>"))
                .Append(new Html("<3>")).Append(new Html("<4>"))
                .WithHint(new Html("<hint>"))
                .AsInputGroup(), // This shouldn't take effect since we haven't specified this field can be an input group
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html_when_allowed_input_group()
        {
            var t = new TwitterBootstrapFormTemplate();
            var metadata = Substitute.For<IFieldMetadata>();
            metadata.IsRequired.Returns(true);
            var fc = new FieldConfiguration();
            fc.Bag.CanBeInputGroup = true;

            var result = t.Field(new Html("labelhtml"), new Html("elementhtml"), new Html("validationhtml"), metadata, fc
                .Prepend(new Html("<1>")).Prepend(new Html("<2>"))
                .Append(new Html("<3>")).Append(new Html("<4>"))
                .WithHint(new Html("<hint>"))
                .AsInputGroup(), // This shouldn't take effect since we haven't specified this field can be an input group
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html_when_required()
        {
            var t = new TwitterBootstrapFormTemplate();
            var metadata = new ModelMetadata(new EmptyModelMetadataProvider(), typeof(object), () => null, typeof(object), "");
            metadata.IsRequired = true;

            var result = t.Field(new Html("labelhtml"), new Html("elementhtml"), new Html("validationhtml"), null, new FieldConfiguration()
                .Prepend(new Html("<1>")).Prepend(new Html("<2>"))
                .Append(new Html("<3>")).Append(new Html("<4>"))
                .WithHint(new Html("<hint>")),
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
            var metadata = Substitute.For<IFieldMetadata>();
            metadata.IsRequired.Returns(true);

            var result = t.Field(new Html("labelhtml"), new Html("elementhtml"), new Html("validationhtml"), metadata, fc
                .Prepend(new Html("<1>")).Prepend(new Html("<2>"))
                .Append(new Html("<3>")).Append(new Html("<4>"))
                .WithHint(new Html("<hint>")),
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_radio_list_field_with_prepended_and_appended_html_when_required()
        {
            var t = new TwitterBootstrapFormTemplate();
            var fc = new FieldConfiguration();
            fc.Bag.IsRadioOrCheckboxList = true;
            var metadata = Substitute.For<IFieldMetadata>();
            metadata.IsRequired.Returns(true);

            var result = t.Field(new Html("labelhtml"), new Html("elementhtml"), new Html("validationhtml"), metadata, fc
                .Prepend(new Html("<1>")).Prepend(new Html("<2>"))
                .Append(new Html("<3>")).Append(new Html("<4>"))
                .WithHint(new Html("<hint>")),
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_field()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.BeginField(new Html("labelhtml"), new Html("elementhtml"), new Html("validationhtml"), null, new FieldConfiguration(), false);

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
