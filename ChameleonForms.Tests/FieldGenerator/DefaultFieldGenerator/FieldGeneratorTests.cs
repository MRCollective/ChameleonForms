using System.Web;
using ApprovalTests.Html;
using ChameleonForms.Component.Config;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class FieldGeneratorTests : DefaultFieldGeneratorShould
    {
        [Test]
        public void Use_overridden_html_for_field_if_provided()
        {
            var g = Arrange(m => m.RequiredString);
            var field = new HtmlString("<p>override</p>");

            var result = g.GetFieldHtml(new FieldConfiguration().OverrideFieldHtml(field));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_field_label_without_config()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetLabelHtml(default(IFieldConfiguration));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_label_class_for_field_label_with_html_attributes_including_a_class()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetLabelHtml(new FieldConfiguration().AddClass("dontshowthis").WithLabelClasses("showthis"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_field_label_with_null_label()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetLabelHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_field_label_with_display_name()
        {
            var g = Arrange(m => m.StringWithDisplayAttribute);

            var result = g.GetLabelHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_field_label_with_overridden_label()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetLabelHtml(new FieldConfiguration().Label("asdf"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_field_label_with_overridden_html_label()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetLabelHtml(new FieldConfiguration().Label(new HtmlString("<em>lay</em>bell")));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_field_label_with_overridden_id()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetLabelHtml(new FieldConfiguration().Attr(id => "DifferentId"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_field_label_with_overridden_id_2()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetLabelHtml(new FieldConfiguration().Id("DifferentId"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_label_text_when_without_label()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetLabelHtml(new FieldConfiguration().WithoutLabel());

            Assert.That(result.ToHtmlString(), Is.EqualTo("RequiredString"));
        }

        [Test]
        public void Use_correct_html_for_field_validation()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetValidationHtml(default(IFieldConfiguration));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
