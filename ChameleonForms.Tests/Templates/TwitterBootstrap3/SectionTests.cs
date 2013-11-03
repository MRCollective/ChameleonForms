using System.Web;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Templates.TwitterBootstrap3;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates.TwitterBootstrap3
{
    [UseReporter(typeof(DiffReporter))]
    class SectionTests_TwitterBootstrapFormTemplateShould
    {

        [Test]
        public void Begin_section()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.BeginSection(new HtmlString("Section Title"), new HtmlString("<p>hello</p>"), new { @class = "asdf" }.ToHtmlAttributes());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Begin_section_without_leading_html_or_title()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.BeginSection();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void End_section()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.EndSection();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Begin_nested_section_without_leading_html_or_title()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.BeginNestedSection();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Begin_nested_section()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.BeginNestedSection(new HtmlString("Section Title"), new HtmlString("<p>Hello</p>"), new { @class = "asdf" }.ToHtmlAttributes());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void End_nested_section()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.EndNestedSection();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

    }
}
