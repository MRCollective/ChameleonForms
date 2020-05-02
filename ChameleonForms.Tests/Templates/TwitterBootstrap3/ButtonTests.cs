using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component;
using ChameleonForms.Templates.TwitterBootstrap3;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates.TwitterBootstrap3
{
    [UseReporter(typeof(DiffReporter))]
    class ButtonTests
    {
        [Test]
        public void Output_button_input_when_button_with_no_content_specified()
        {
            var t = new TwitterBootstrap3FormTemplate();

            var result = t.Button(null, null, null, "value", null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_submit_input_when_button_with_no_content_and_submit_type_specified()
        {
            var t = new TwitterBootstrap3FormTemplate();

            var result = t.Button(null, "submit", "id", "value", new HtmlAttributes(@class => "asdf"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_button_when_button_with_content_specified()
        {
            var t = new TwitterBootstrap3FormTemplate();

            var result = t.Button(new HtmlString("<strong>asdf</strong>"), null, null, null, null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_submit_button_when_button_with_content_and_submit_type_specified()
        {
            var t = new TwitterBootstrap3FormTemplate();

            var result = t.Button(new HtmlString("<strong>asdf</strong>"), "submit", "id", "value", new HtmlAttributes(@class => "asdf"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_submit_button_with_non_default_style_or_size()
        {
            var t = new TwitterBootstrap3FormTemplate();

            var result = new ButtonHtmlAttributes(h => t.Button(new HtmlString("content"), "submit", null, null, h))
                .WithStyle(EmphasisStyle.Danger)
                .WithSize(ButtonSize.Large)
                .WithIcon("star");

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
