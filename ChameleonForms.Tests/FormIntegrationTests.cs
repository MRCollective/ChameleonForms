using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Tests.FieldGenerator;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;
using ChameleonForms.Component;

namespace ChameleonForms.Tests
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class IntegratedFormShould
    {
        private HtmlHelper<TestFieldViewModel> _h;

        [SetUp]
        public void Setup()
        {
            var autoSubstitute = AutoSubstituteContainer.Create();
            _h = autoSubstitute.Resolve<HtmlHelper<TestFieldViewModel>>();
        }

        [Test]
        public void Output_field_html_with_field_configuration_values()
        {
            var form = _h.BeginChameleonForm();

            var html = form.FieldFor(m => m.Decimal).AddClass("a-class").ToHtmlString();

            HtmlApprovals.VerifyHtml(html);
        }

        [Test]
        public void Output_label_html_with_field_configuration_values()
        {
            var form = _h.BeginChameleonForm();

            var html = form.LabelFor(m => m.Decimal).Label("LABEL").ToHtmlString();

            HtmlApprovals.VerifyHtml(html);
        }
    }
}
