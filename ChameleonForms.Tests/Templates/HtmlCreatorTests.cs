using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Templates;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class BuildSubmitButtonShould
    {
        [Test]
        public void Generate_submit_button_with_default_options()
        {
            var h = HtmlCreator.BuildSubmitButton("value");

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Generate_submit_button_with_non_default_options()
        {
            var h = HtmlCreator.BuildSubmitButton("thevalue", "reset", "myId", new HtmlAttributes(new {onclick = "return false;", @class = "a&^&*FGdf"}));

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }
    }
}
