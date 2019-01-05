using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Templates.TwitterBootstrap3;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates.TwitterBootstrap3
{
    [UseReporter(typeof(DiffReporter))]
    class NavigationTests_TwitterBootstrapFormTemplateShould
    {
        [Test]
        public void Output_begin_navigation()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.BeginNavigation();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_end_navigation()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.EndNavigation();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
