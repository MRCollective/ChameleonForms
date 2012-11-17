using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component.Config;
using ChameleonForms.FieldGenerator;
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

        public TestEnum TestEnum { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.MultilineText)]
        public string Textarea { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }

        public bool BooleanField { get; set; }
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

        private DefaultFieldGenerator<TestFieldViewModel, T> Arrange<T>(Expression<Func<TestFieldViewModel,T>> property, Action<TestFieldViewModel> vmSetter = null)
        {
            _h.ViewContext.ClientValidationEnabled = true;
            _h.ViewContext.ViewData.ModelState.AddModelError(ExpressionHelper.GetExpressionText(property), "asdf");
            var vm = new TestFieldViewModel();
            if (vmSetter != null)
                vmSetter(vm);
            _h.ViewData.ModelMetadata.Model = vm;

            return new DefaultFieldGenerator<TestFieldViewModel, T>(_h, property);
        }

        [Test]
        public void Use_correct_html_for_field_label()
        {
            var g = Arrange(m => m.SomeProperty);

            var result = g.GetLabelHtml(null);

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
        public void Use_correct_html_for_enum_field()
        {
            var g = Arrange(m => m.TestEnum, m => m.TestEnum = TestEnum.ValueWithDescriptionAttribute);

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
            var g = Arrange(m => m.BooleanField);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_checked_single_checkbox_with_default_label()
        {
            var g = Arrange(m => m.BooleanField, m => m.BooleanField = true);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_single_checkbox_with_custom_label()
        {
            var g = Arrange(m => m.BooleanField);

            var result = g.GetFieldHtml(new FieldConfiguration().InlineLabel("Some label"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
