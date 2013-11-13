using System.Web.Mvc;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component;
using ChameleonForms.Tests.FieldGenerator;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;

namespace ChameleonForms.Tests.Form
{
    [TestFixture]
    [UseReporter(typeof (DiffReporter))]
    internal class IntegratedFormContextShould
    {
        [SetUp]
        public void Setup()
        {
            var autoSubstitute = AutoSubstituteContainer.Create();
            _h = autoSubstitute.Resolve<HtmlHelper<TestFieldViewModel>>();
        }

        private HtmlHelper<TestFieldViewModel> _h;

        [Test]
        public void Output_markup_for_a_field()
        {
            var html = _h.BeginChameleonFormContext().FieldElementFor(x => x.Decimal).ToHtmlString();

            HtmlApprovals.VerifyHtml(html);
        }
    }
}