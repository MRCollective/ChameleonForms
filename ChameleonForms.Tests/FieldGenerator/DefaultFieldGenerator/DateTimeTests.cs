using ApprovalTests.Html;
using ChameleonForms.Component.Config;
using ChameleonForms.Templates;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class DateTimeTests : DefaultFieldGeneratorShould
    {
        [Test]
        public void Use_correct_html_for_datetime_field()
        {
            var g = Arrange(m => m.DateTime);

            var result = g.GetFieldHtml(new FieldConfiguration { Attributes = new HtmlAttributes(data_attr => "value") });

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_datetime_field()
        {
            var g = Arrange(m => m.NullableDateTime);

            var result = g.GetFieldHtml(new FieldConfiguration { Attributes = new HtmlAttributes(data_attr => "value") });

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_datetime_field_with_format()
        {
            var g = Arrange(m => m.DateTimeWithFormat);

            var result = g.GetFieldHtml(new FieldConfiguration { Attributes = new HtmlAttributes(data_attr => "value") });

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_datetime_field_with_format()
        {
            var g = Arrange(m => m.NullableDateTimeWithFormat);

            var result = g.GetFieldHtml(new FieldConfiguration { Attributes = new HtmlAttributes(data_attr => "value") });

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
