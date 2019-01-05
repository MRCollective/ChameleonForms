
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Enums;
using ChameleonForms.Templates.TwitterBootstrap3;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates.TwitterBootstrap3
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class FormTests_TwitterBootstrapTemplateShould
    {
        [Test]
        public void Begin_form_with_enctype()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.BeginForm("/", FormMethod.Post, new HtmlAttributes(data_attr => "value"), EncType.Multipart);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Begin_form_without_enctype()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.BeginForm("/", FormMethod.Post, null, null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void End_form()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.EndForm();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
