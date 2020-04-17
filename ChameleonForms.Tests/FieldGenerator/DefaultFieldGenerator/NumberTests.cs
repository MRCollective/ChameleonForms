using ApprovalTests.Html;
using ChameleonForms.Component.Config;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class NumberTests : DefaultFieldGeneratorShould
    {
        [Test]
        public void Return_correct_html_for_int_field()
        {
            var generator = Arrange(m => m.IntField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_nullable_int_field()
        {
            var generator = Arrange(m => m.NullableIntField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_byte_field()
        {
            var generator = Arrange(m => m.ByteField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_byte_field_with_overidden_step_min_and_max()
        {
            var generator = Arrange(m => m.ByteField);

            var html = generator.GetFieldHtml(new FieldConfiguration().Step(2).Min(2).Max(10));

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_sbyte_field()
        {
            var generator = Arrange(m => m.SbyteField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_short_field()
        {
            var generator = Arrange(m => m.ShortField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_ushort_field()
        {
            var generator = Arrange(m => m.UshortField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_uint_field()
        {
            var generator = Arrange(m => m.UintField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_long_field()
        {
            var generator = Arrange(m => m.LongField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_ulong_field()
        {
            var generator = Arrange(m => m.UlongField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_float_field()
        {
            var generator = Arrange(m => m.FloatField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_double_field()
        {
            var generator = Arrange(m => m.DoubleField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_decimal_field()
        {
            var generator = Arrange(m => m.DecimalField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_currency_field()
        {
            var generator = Arrange(m => m.MoneyField);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_int_with_range_field()
        {
            var generator = Arrange(m => m.IntWithRange);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }

        [Test]
        public void Return_correct_html_for_decimal_with_range_field()
        {
            var generator = Arrange(m => m.DecimalWithRange);

            var html = generator.GetFieldHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(html.ToHtmlString());
        }
    }
}
