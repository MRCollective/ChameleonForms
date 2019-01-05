using ApprovalTests.Html;
using ChameleonForms.Component.Config;
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
        public void Use_correct_html_for_decimal_field()
        {
            var g = Arrange(m => m.Decimal, m => m.Decimal = 1.2000m);

            var result = g.GetFieldHtml(default(IFieldConfiguration));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_decimal_field_with_format_string_attribute()
        {
            var g = Arrange(m => m.DecimalWithFormatStringAttribute, m => m.DecimalWithFormatStringAttribute = 1.2000m);

            var result = g.GetFieldHtml(default(IFieldConfiguration));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_decimal_field_with_format_string_attribute_but_overriden()
        {
            var g = Arrange(m => m.DecimalWithFormatStringAttribute, m => m.DecimalWithFormatStringAttribute = 1.2000m);

            var result = g.GetFieldHtml(new FieldConfiguration().WithFormatString("{0:F3}"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_decimal_field_with_explicit_format_string()
        {
            var g = Arrange(m => m.Decimal, m => m.Decimal = 1.2000m);

            var result = g.GetFieldHtml(new FieldConfiguration().WithFormatString("{0:f2}"));

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
    }
}
