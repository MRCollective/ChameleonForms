using System.Web;
using ApprovalTests.Html;
using ChameleonForms.Component.Config;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class FlagsEnumTests : DefaultFieldGeneratorShould
    {
        [Test]
        public void Use_correct_html_for_required_enum_field()
        {
            var g = Arrange(m => m.RequiredFlagsEnum, m => m.RequiredFlagsEnum = TestFlagsEnum.ValueWithDescriptionAttribute | TestFlagsEnum.Simplevalue);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_enum_field_with_null_string_attribute()
        {
            var g = Arrange(m => m.OptionalFlagsEnumWithNullStringAttribute, m => m.OptionalFlagsEnumWithNullStringAttribute = null);

            var result = g.GetFieldHtml(default(IFieldConfiguration));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_enum_field_with_overridden_null_string_attribute()
        {
            var g = Arrange(m => m.OptionalFlagsEnumWithNullStringAttribute, m => m.OptionalFlagsEnumWithNullStringAttribute = null);

            var result = g.GetFieldHtml(new FieldConfiguration().WithNoneAs("Overridden"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_enum_field()
        {
            var g = Arrange(m => m.OptionalFlagsEnum, m => m.OptionalFlagsEnum = null);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_required_enum_field()
        {
            var g = Arrange(m => m.RequiredNullableFlagsEnum);

            var result = g.GetFieldHtml(ExampleFieldConfiguration);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
        
        [Test]
        public void Use_correct_html_for_label_for_enum_list()
        {
            var g = Arrange(m => m.RequiredFlagsEnum);

            var result = g.GetLabelHtml(new FieldConfiguration().AsRadioList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_label_for_enum_list_with_overridden_label()
        {
            var g = Arrange(m => m.RequiredFlagsEnum);

            var result = g.GetLabelHtml(new FieldConfiguration().AsRadioList().Label(new HtmlString("<strong>lol</strong>")));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_label_for_enum_dropdown()
        {
            var g = Arrange(m => m.RequiredFlagsEnum);

            var result = g.GetLabelHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_child_viewmodel_enum_list_field()
        {
            var g = Arrange(m => m.Child.RequiredChildFlagsEnum);

            var result = g.GetFieldHtml(ExampleFieldConfiguration.AsRadioList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_enum_list_with_excluded_value()
        {
            var g = Arrange(m => m.RequiredFlagsEnum);

            var result = g.GetFieldHtml(ExampleFieldConfiguration.Exclude(TestFlagsEnum.Simplevalue));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
