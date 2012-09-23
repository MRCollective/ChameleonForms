using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Templates;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class HtmlHelperExtensionsShould
    {
        [Test]
        public void Generate_submit_button_with_default_options()
        {
            var h = Html.BuildSubmitButton("value");

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Generate_submit_button_with_non_default_options()
        {
            var h = Html.BuildSubmitButton("thevalue", "reset", "myId", new {onclick = "return false;", @class = "a&^&*FGdf"});

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }
    }
}
