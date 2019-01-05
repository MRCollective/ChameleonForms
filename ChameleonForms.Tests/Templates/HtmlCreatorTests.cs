using System.Web;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Templates;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class HtmlCreatorShould
    {
        [Test]
        public void Generate_button_with_default_options()
        {
            var h = HtmlCreator.BuildButton("value");

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Generate_submit_button_with_non_default_options()
        {
            var h = HtmlCreator.BuildButton("thevalue&", "submit", "myId", htmlAttributes: new HtmlAttributes(new {onclick = "return false;", @class = "a&^&*FGdf"}));

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Generate_reset_button_with_html_content()
        {
            var h = HtmlCreator.BuildButton(new HtmlString("<b>content</b>"), "reset", "name", "value",
                new HtmlAttributes().Attr(id => "overriddenid").AddClass("lol")
            );

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Generate_input()
        {
            var h = HtmlCreator.BuildInput("name", "value&", "submit", new HtmlAttributes().AddClass("lol"));

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Generate_input_without_name()
        {
            var h = HtmlCreator.BuildInput(null, "value&", "submit", new HtmlAttributes().AddClass("lol"));

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }
    }
}
