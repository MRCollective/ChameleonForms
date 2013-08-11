using System.Collections.Generic;
using ApprovalTests.Html;
using ChameleonForms.Component.Config;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class EnumTests : DefaultFieldGeneratorShould
    {
        [Test]
        public void Use_correct_html_for_required_enum_field()
        {
            var g = Arrange(m => m.RequiredEnum, m => m.RequiredEnum = TestEnum.ValueWithDescriptionAttribute);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_enum_field_with_null_string_attribute()
        {
            var g = Arrange(m => m.OptionalEnumWithNullStringAttribute, m => m.OptionalEnumWithNullStringAttribute = null);

            var result = g.GetFieldHtml(default(IFieldConfiguration));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_enum_field_with_overridden_null_string_attribute()
        {
            var g = Arrange(m => m.OptionalEnumWithNullStringAttribute, m => m.OptionalEnumWithNullStringAttribute = null);

            var result = g.GetFieldHtml(new FieldConfiguration().WithNoneAs("Overridden"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_enum_field()
        {
            var g = Arrange(m => m.OptionalEnum, m => m.OptionalEnum = null);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_required_enum_field()
        {
            var g = Arrange(m => m.RequiredNullableEnum);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_enum_list_field()
        {
            var g = Arrange(m => m.RequiredEnumList, m => m.RequiredEnumList = new List<TestEnum>{TestEnum.Simplevalue,TestEnum.ValueWithDescriptionAttribute});

            var result = g.GetFieldHtml(ExampleFieldConfiguration.AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_nullable_enum_list_field()
        {
            var g = Arrange(m => m.RequiredNullableEnumList);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_enum_list_field()
        {
            var g = Arrange(m => m.OptionalEnumList);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_nullable_enum_list_field()
        {
            var g = Arrange(m => m.OptionalNullableEnumList, m => m.OptionalNullableEnumList = new List<TestEnum?> { TestEnum.Simplevalue, TestEnum.ValueWithDescriptionAttribute });

            var result = g.GetFieldHtml(ExampleFieldConfiguration.AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_dropdown_optional_nullable_enum_list_field()
        {
            var g = Arrange(m => m.OptionalNullableEnumList, m => m.OptionalNullableEnumList = new List<TestEnum?> { TestEnum.Simplevalue, TestEnum.ValueWithDescriptionAttribute });

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
