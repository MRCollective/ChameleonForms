﻿using ApprovalTests.Html;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class DateTimeTests : DefaultFieldGeneratorShould
    {
        [Test]
        public void Use_correct_html_for_datetime_field()
        {
            using (ChangeCulture.To("en-AU"))
            {
                var g = Arrange(m => m.DateTime);

                var result = g.GetFieldHtml(ExampleFieldConfiguration);

                HtmlApprovals.VerifyHtml(result.ToHtmlString());
            }
        }

        [Test]
        public void Use_correct_html_for_nullable_datetime_field()
        {
            var g = Arrange(m => m.NullableDateTime);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_datetime_field_with_format()
        {
            var g = Arrange(m => m.DateTimeWithFormat);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_datetime_field_with_format()
        {
            var g = Arrange(m => m.NullableDateTimeWithFormat);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
