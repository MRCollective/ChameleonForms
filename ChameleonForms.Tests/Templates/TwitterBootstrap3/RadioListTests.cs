using System.Web;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Templates.TwitterBootstrap3;
using Microsoft.AspNetCore.Html;
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

            var result = t.RadioOrCheckboxList(new IHtmlContent[] {new HtmlString("1"), new HtmlString("2")}, false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Render_checkbox_list()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.RadioOrCheckboxList(new IHtmlContent[] { new HtmlString("1"), new HtmlString("2") }, true);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
