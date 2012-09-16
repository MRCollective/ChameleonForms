using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.FieldGenerator;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator
{
    public class TestFieldViewModel
    {
        [Required]
        public string SomeProperty { get; set; }
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

        private DefaultFieldGenerator<TestFieldViewModel, T> Arrange<T>(Expression<Func<TestFieldViewModel,T>> property)
        {
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
        public void Use_correct_html_for_text_field()
        {
            var g = Arrange(m => m.SomeProperty);

            var result = g.GetFieldHtml();

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

    }
}
