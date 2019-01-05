using System.Collections.Generic;
using System.Web;
using ApprovalTests.Html;
using ChameleonForms.Component.Config;
using Microsoft.AspNetCore.Html;
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

            var result = g.GetFieldHtml(ExampleFieldConfiguration.AsRadioList());

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

            var result = g.GetFieldHtml(ExampleFieldConfiguration.AsRadioList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_dropdown_optional_nullable_enum_list_field()
        {
            var g = Arrange(m => m.OptionalNullableEnumList, m => m.OptionalNullableEnumList = new List<TestEnum?> { TestEnum.Simplevalue, TestEnum.ValueWithDescriptionAttribute });

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_label_for_enum_list()
        {
            var g = Arrange(m => m.RequiredEnum);

            var result = g.GetLabelHtml(new FieldConfiguration().AsRadioList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_label_for_enum_list_with_overridden_label()
        {
            var g = Arrange(m => m.RequiredEnum);

            var result = g.GetLabelHtml(new FieldConfiguration().AsRadioList().Label(new HtmlString("<strong>lol</strong>")));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_label_for_enum_dropdown()
        {
            var g = Arrange(m => m.RequiredEnum);

            var result = g.GetLabelHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_child_viewmodel_enum_list_field()
        {
            var g = Arrange(m => m.Child.RequiredChildEnum);

            var result = g.GetFieldHtml(ExampleFieldConfiguration.AsRadioList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_enum_list_with_excluded_value()
        {
            var g = Arrange(m => m.RequiredEnum);

            var result = g.GetFieldHtml(ExampleFieldConfiguration.Exclude(TestEnum.Simplevalue));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
