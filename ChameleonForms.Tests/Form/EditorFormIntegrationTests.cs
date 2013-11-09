using System.Web;
using System.Web.Mvc;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component;
using ChameleonForms.Tests.FieldGenerator;
using ChameleonForms.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Form
{
    [TestFixture]
    [UseReporter(typeof (DiffReporter))]
    internal class IntegratedEditorFormShould
    {
        [SetUp]
        public void Setup()
        {
            var autoSubstitute = AutoSubstituteContainer.Create();
            _h = autoSubstitute.Resolve<HtmlHelper<TestFieldViewModel>>();
        }

        private HtmlHelper<TestFieldViewModel> _h;

        [Test]
        public void Output_no_markup_for_an_empty_editor()
        {
            using (var f = _h.BeginChameleonEditor())
            {
            }

            _h.ViewContext.Writer.DidNotReceive().Write(Arg.Any<IHtmlString>());
        }

        [Test]
        public void Output_some_markup_for_an_editor_with_a_section_and_field()
        {
            using (var f = _h.BeginChameleonEditor())
            {
                using (var s = f.BeginSection("Section"))
                {
                    s.FieldFor(x => x.Decimal);
                }
            }

            _h.ViewContext.Writer.Received().Write(Arg.Any<IHtmlString>());
        }

        [Test]
        public void Output_markup_for_a_field()
        {
            var html = _h.BeginChameleonEditor().FieldElementFor(x => x.Decimal).ToHtmlString();

            HtmlApprovals.VerifyHtml(html);
        }
    }
}