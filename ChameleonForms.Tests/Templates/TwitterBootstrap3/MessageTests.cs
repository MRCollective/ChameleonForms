﻿using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Enums;
using ChameleonForms.Templates.TwitterBootstrap3;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates.TwitterBootstrap3
{
    [UseReporter(typeof(DiffReporter))]
    class MessageTests_TwitterBootstrapFormTemplateShould
    {
        [Test]
        public void Output_begin_information_message()
        {
            var t = new TwitterBootstrap3FormTemplate();

            var result = t.BeginMessage(MessageType.Information, new HtmlString("Heading"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_information_message_without_heading()
        {
            var t = new TwitterBootstrap3FormTemplate();

            var result = t.BeginMessage(MessageType.Information, new HtmlString(""));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_failure_message()
        {
            var t = new TwitterBootstrap3FormTemplate();

            var result = t.BeginMessage(MessageType.Failure, new HtmlString("Heading"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_end_message()
        {
            var t = new TwitterBootstrap3FormTemplate();

            var result = t.EndMessage();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_message_paragraph()
        {
            var t = new TwitterBootstrap3FormTemplate();

            var result = t.MessageParagraph(new HtmlString("<strong>asdf</strong>"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
