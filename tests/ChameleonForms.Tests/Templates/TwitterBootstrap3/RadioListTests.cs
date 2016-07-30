using System.Web;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Templates.TwitterBootstrap3;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates.TwitterBootstrap3
{
    [UseReporter(typeof(DiffReporter))]
    class RadioListTests_TwitterBootstrapTemplateShould
    {
        [Test]
        public void Render_radio_list()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.RadioOrCheckboxList(new IHtml[] {new Html("1"), new Html("2")}, false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        [Ignore("Broken due to https://github.com/RazorGenerator/RazorGenerator/issues/71")]
        public void Render_checkbox_list()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.RadioOrCheckboxList(new IHtml[] { new Html("1"), new Html("2") }, true);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
