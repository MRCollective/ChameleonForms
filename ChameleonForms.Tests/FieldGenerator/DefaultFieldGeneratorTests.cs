using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.FieldGenerator;
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

            var result = g.GetLabelHtml();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        [Ignore("Can't figure out a good way to test this properly.")]
        public void Use_correct_html_for_field_validation()
        {
            var g = Arrange(m => m.SomeProperty);

            var result = g.GetValidationHtml();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_text_field()
        {
            var g = Arrange(m => m.SomeProperty);

            var result = g.GetFieldHtml();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_enum_field()
        {
            var g = Arrange(m => m.TestEnum, m => m.TestEnum = TestEnum.ValueWithDescriptionAttribute);

            var result = g.GetFieldHtml();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
