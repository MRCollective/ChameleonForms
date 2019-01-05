using System.Web;
using ApprovalTests.Html;
using ChameleonForms.Component.Config;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class BooleanTests : DefaultFieldGeneratorShould
    {
        [Test]
        public void Use_correct_html_for_single_checkbox_with_default_label()
        {
            var g = Arrange(m => m.RequiredBoolean);

            var result = g.GetFieldHtml(default(IFieldConfiguration));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_checked_single_checkbox_with_default_label()
        {
            var g = Arrange(m => m.RequiredBoolean, m => m.RequiredBoolean = true);

            var result = g.GetFieldHtml(default(IFieldConfiguration));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_single_checkbox_with_custom_label()
        {
            var g = Arrange(m => m.RequiredBoolean);

            var result = g.GetFieldHtml(new FieldConfiguration().InlineLabel("Some label"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_single_checkbox_with_label_wrapping_element()
        {
            var g = Arrange(m => m.RequiredBoolean);

            var result = g.GetFieldHtml(new FieldConfiguration().InlineLabelWrapsElement());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_boolean_list_and_false_value()
        {
            var g = Arrange(m => m.RequiredBoolean);

            var result = g.GetFieldHtml(new FieldConfiguration().AsRadioList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_boolean_list_and_true_value()
        {
            var g = Arrange(m => m.RequiredBoolean, m => m.RequiredBoolean = true);

            var result = g.GetFieldHtml(new FieldConfiguration().AsRadioList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_boolean_list_and_no_value()
        {
            var g = Arrange(m => m.OptionalBooleanField, x => x.OptionalBooleanField = null);

            var result = g.GetFieldHtml(new FieldConfiguration().AsRadioList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_boolean_dropdown_and_no_value()
        {
            var g = Arrange(m => m.OptionalBooleanField);

            var result = g.GetFieldHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_boolean_and_no_value()
        {
            var g = Arrange(m => m.OptionalBooleanField);

            var result = g.GetFieldHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_boolean_list_and_custom_labels()
        {
            var g = Arrange(m => m.RequiredBoolean);

            var result = g.GetFieldHtml(new FieldConfiguration().AsRadioList().WithTrueAs("True").WithFalseAs("False"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_boolean_list_with_label_wrapping_element()
        {
            var g = Arrange(m => m.RequiredBoolean);

            var result = g.GetFieldHtml(new FieldConfiguration().AsRadioList().InlineLabelWrapsElement());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_boolean_dropdown_list_and_false_value()
        {
            var g = Arrange(m => m.RequiredBoolean);

            var result = g.GetFieldHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_boolean_dropdown_list_and_true_value()
        {
            var g = Arrange(m => m.RequiredBoolean, m => m.RequiredBoolean = true);

            var result = g.GetFieldHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_required_boolean_checkbox_with_no_value()
        {
            var g = Arrange(m => m.RequiredNullableBoolean);

            var result = g.GetFieldHtml(default(IFieldConfiguration));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_required_boolean_list_with_no_value()
        {
            var g = Arrange(m => m.RequiredNullableBoolean);

            var result = g.GetFieldHtml(new FieldConfiguration().AsRadioList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_required_boolean_dropdown_with_no_value()
        {
            var g = Arrange(m => m.RequiredNullableBoolean);

            var result = g.GetFieldHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_label_for_boolean_list()
        {
            var g = Arrange(m => m.RequiredNullableBoolean);

            var result = g.GetLabelHtml(new FieldConfiguration().AsRadioList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_label_for_boolean_list_with_overridden_label()
        {
            var g = Arrange(m => m.RequiredNullableBoolean);

            var result = g.GetLabelHtml(new FieldConfiguration().AsRadioList().Label(new HtmlString("<strong>lol</strong>")));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_label_for_boolean_dropdown()
        {
            var g = Arrange(m => m.RequiredNullableBoolean);

            var result = g.GetLabelHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_label_for_boolean_checkbox()
        {
            var g = Arrange(m => m.RequiredNullableBoolean);

            var result = g.GetLabelHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_without_inline_label_for_boolean_checkbox()
        {
            var g = Arrange(x => x.RequiredBoolean);

            IFieldConfiguration config = new FieldConfiguration();
            config = config.WithoutInlineLabel();

            var result = g.GetFieldHtml(config);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
