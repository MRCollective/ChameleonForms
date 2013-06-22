using ApprovalTests.Html;
using ChameleonForms.Component.Config;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class LabelTests : DefaultFieldGeneratorShould
    {
        [Test]
        public void Use_correct_html_for_field_label_without_config()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetLabelHtml(null);

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
        public void Use_correct_html_for_field_label_with_overridden_label()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetLabelHtml(new FieldConfiguration().Label("asdf"));

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
        public void Use_correct_html_for_field_validation()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetValidationHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
