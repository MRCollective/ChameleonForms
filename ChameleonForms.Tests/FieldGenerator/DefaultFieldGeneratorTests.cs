using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Attributes;
using ChameleonForms.Component.Config;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Templates;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace ChameleonForms.Tests.FieldGenerator
{
    public enum TestEnum
    {
        Simplevalue,
        [Description("Description attr text")]
        ValueWithDescriptionAttribute,
        ValueWithMultpipleWordsAndNoDescriptionAttribute,
    }

    public class TestFieldViewModel
    {
        [Required]
        public string SomeProperty { get; set; }

        public TestEnum RequiredTestEnum { get; set; }

        [Required]
        public TestEnum? RequiredNullableEnum { get; set; }

        public TestEnum? OptionalTestEnum { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.MultilineText)]
        public string Textarea { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }

        public bool RequiredBooleanField { get; set; }

        [Required]
        public bool? RequiredNullableBooleanField { get; set; }

        public bool? OptionalBooleanField { get; set; }

        public List<IntListItem> IntList { get; set; }
        public List<StringListItem> StringList { get; set; }

        [ExistsIn("IntList", "Id", "Name")]
        public int? OptionalIntListId { get; set; }

        [Required]
        [ExistsIn("IntList", "Id", "Name")]
        public int? RequiredNullableIntListId { get; set; }

        [ExistsIn("StringList", "Value", "Label")]
        public string OptionalStringListId { get; set; }

        [ExistsIn("IntList", "Id", "Name")]
        public int RequiredIntListId { get; set; }

        [Required]
        [ExistsIn("StringList", "Value", "Label")]
        public string RequiredStringListId { get; set; }
    }

    public class IntListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class StringListItem
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class DefaultFieldGeneratorShould
    {
        private HtmlHelper<TestFieldViewModel> _h;

        [SetUp]
        public void Setup()
        {
            var autoSubstitute = AutoSubstituteContainer.Create();
            _h = autoSubstitute.Resolve<HtmlHelper<TestFieldViewModel>>();
        }

        private DefaultFieldGenerator<TestFieldViewModel, T> Arrange<T>(Expression<Func<TestFieldViewModel,T>> property, params Action<TestFieldViewModel>[] vmSetter)
        {
            _h.ViewContext.UnobtrusiveJavaScriptEnabled = true;
            _h.ViewContext.ClientValidationEnabled = true;
            _h.ViewContext.ViewData.ModelState.AddModelError(ExpressionHelper.GetExpressionText(property), "asdf");
            var vm = new TestFieldViewModel();
            foreach (var action in vmSetter)
            {
                action(vm);
            }
            _h.ViewData.ModelMetadata.Model = vm;

            return new DefaultFieldGenerator<TestFieldViewModel, T>(_h, property);
        }

        [Test]
        public void Use_correct_html_for_field_label_without_config()
        {
            var g = Arrange(m => m.SomeProperty);

            var result = g.GetLabelHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_field_label_with_null_label()
        {
            var g = Arrange(m => m.SomeProperty);

            var result = g.GetLabelHtml(new FieldConfiguration());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_field_label_with_overridden_label()
        {
            var g = Arrange(m => m.SomeProperty);

            var result = g.GetLabelHtml(new FieldConfiguration().Label("asdf"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_field_validation()
        {
            var g = Arrange(m => m.SomeProperty);

            var result = g.GetValidationHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_text_field()
        {
            var g = Arrange(m => m.SomeProperty);

            var result = g.GetFieldHtml(new FieldConfiguration {Attributes = new HtmlAttributes(data_attr => "value")});

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_password_field()
        {
            var g = Arrange(m => m.Password);

            var result = g.GetFieldHtml(new FieldConfiguration { Attributes = new HtmlAttributes(data_attr => "value") });

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_textarea()
        {
            var g = Arrange(m => m.Textarea);

            var result = g.GetFieldHtml(new FieldConfiguration { Attributes = new HtmlAttributes(data_attr => "value") });

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_enum_field()
        {
            var g = Arrange(m => m.RequiredTestEnum, m => m.RequiredTestEnum = TestEnum.ValueWithDescriptionAttribute);

            var result = g.GetFieldHtml(new FieldConfiguration { Attributes = new HtmlAttributes(data_attr => "value") });

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_enum_field()
        {
            var g = Arrange(m => m.OptionalTestEnum, m => m.OptionalTestEnum = null);

            var result = g.GetFieldHtml(new FieldConfiguration { Attributes = new HtmlAttributes(data_attr => "value") });

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_required_enum_field()
        {
            var g = Arrange(m => m.RequiredNullableEnum);

            var result = g.GetFieldHtml(new FieldConfiguration { Attributes = new HtmlAttributes(data_attr => "value") });

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_file_upload()
        {
            var g = Arrange(m => m.FileUpload);

            var result = g.GetFieldHtml(new FieldConfiguration { Attributes = new HtmlAttributes(data_attr => "value") });

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_single_checkbox_with_default_label()
        {
            var g = Arrange(m => m.RequiredBooleanField);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_checked_single_checkbox_with_default_label()
        {
            var g = Arrange(m => m.RequiredBooleanField, m => m.RequiredBooleanField = true);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_single_checkbox_with_custom_label()
        {
            var g = Arrange(m => m.RequiredBooleanField);

            var result = g.GetFieldHtml(new FieldConfiguration().InlineLabel("Some label"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_boolean_list_and_false_value()
        {
            var g = Arrange(m => m.RequiredBooleanField);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_boolean_list_and_true_value()
        {
            var g = Arrange(m => m.RequiredBooleanField, m => m.RequiredBooleanField = true);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_boolean_list_and_no_value()
        {
            var g = Arrange(m => m.OptionalBooleanField);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

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
            var g = Arrange(m => m.RequiredBooleanField);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList().WithTrueAs("True").WithFalseAs("False"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_boolean_dropdown_list_and_false_value()
        {
            var g = Arrange(m => m.RequiredBooleanField);

            var result = g.GetFieldHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_boolean_dropdown_list_and_true_value()
        {
            var g = Arrange(m => m.RequiredBooleanField, m => m.RequiredBooleanField = true);

            var result = g.GetFieldHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_required_boolean_checkbox_with_no_value()
        {
            var g = Arrange(m => m.RequiredNullableBooleanField);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_required_boolean_list_with_no_value()
        {
            var g = Arrange(m => m.RequiredNullableBooleanField);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_nullable_required_boolean_dropdown_with_no_value()
        {
            var g = Arrange(m => m.RequiredNullableBooleanField);

            var result = g.GetFieldHtml(new FieldConfiguration().AsDropDown());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_int_list_id()
        {
            var list = new List<IntListItem> { new IntListItem {Id = 1, Name = "A"}, new IntListItem {Id = 2, Name = "B"}};
            var g = Arrange(m => m.OptionalIntListId, m => m.OptionalIntListId = null, m => m.IntList = list);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_nullable_int_list_id()
        {
            var list = new List<IntListItem> { new IntListItem { Id = 1, Name = "A" }, new IntListItem { Id = 2, Name = "B" } };
            var g = Arrange(m => m.RequiredNullableIntListId, m => m.RequiredNullableIntListId = null, m => m.IntList = list);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_string_list_id_as_list()
        {
            var list = new List<StringListItem> { new StringListItem { Value = "1", Label = "A" }, new StringListItem { Value = "2", Label = "B" } };
            var g = Arrange(m => m.OptionalStringListId, m => m.OptionalStringListId = "", m => m.StringList = list);
            
            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_null_required_string_list_id_as_list()
        {
            var list = new List<StringListItem> { new StringListItem { Value = "1", Label = "A" }, new StringListItem { Value = "2", Label = "B" } };
            var g = Arrange(m => m.RequiredStringListId, m => m.RequiredStringListId = null, m => m.StringList = list);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_int_list_id_with_none_string_override()
        {
            var list = new List<IntListItem> { new IntListItem { Id = 1, Name = "A" }, new IntListItem { Id = 2, Name = "B" } };
            var g = Arrange(m => m.OptionalIntListId, m => m.OptionalIntListId = null, m => m.IntList = list);

            var result = g.GetFieldHtml(new FieldConfiguration().WithNoneAs("-- Select Item"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_string_list_id_as_list_with_none_string_override()
        {
            var list = new List<StringListItem> { new StringListItem { Value = "1", Label = "A" }, new StringListItem { Value = "2", Label = "B" } };
            var g = Arrange(m => m.OptionalStringListId, m => m.OptionalStringListId = "2", m => m.StringList = list);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList().WithNoneAs("No Value"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_int_list_id()
        {
            var list = new List<IntListItem> { new IntListItem { Id = 1, Name = "A" }, new IntListItem { Id = 2, Name = "B" } };
            var g = Arrange(m => m.RequiredIntListId, m => m.RequiredIntListId = 2, m => m.IntList = list);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_string_list_id_as_list()
        {
            var list = new List<StringListItem> { new StringListItem { Value = "1", Label = "A" }, new StringListItem { Value = "2", Label = "B" } };
            var g = Arrange(m => m.RequiredStringListId, m => m.RequiredStringListId = "2", m => m.StringList = list);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
