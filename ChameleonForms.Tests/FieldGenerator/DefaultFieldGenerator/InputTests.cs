using ApprovalTests.Html;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class InputTests : DefaultFieldGeneratorShould
    {
        [Test]
        public void Use_correct_html_for_text_field()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_text_field_with_value()
        {
            var g = Arrange(m => m.RequiredString, m => m.RequiredString = "asdf");

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Allow_type_to_be_overridden()
        {
            var g = Arrange(m => m.RequiredString);

            var result = g.GetFieldHtml(ExampleFieldConfiguration.Attr("type", "number"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_password_field()
        {
            var g = Arrange(m => m.Password);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_textarea()
        {
            var g = Arrange(m => m.Textarea);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_textarea_with_overriden_cols_and_rows()
        {
            var g = Arrange(m => m.Textarea);

            var result = g.GetFieldHtml(ExampleFieldConfiguration.Cols(60).Rows(5));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_email()
        {
            var g = Arrange(m => m.Email);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_string_url()
        {
            var g = Arrange(m => m.UrlAsString);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_uri_url()
        {
            var g = Arrange(m => m.UrlAsUri);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
