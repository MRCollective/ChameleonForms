using System.Net.Http;
using System.Web;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Enums;
using ChameleonForms.Templates;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class DefaultFormTemplateShould
    {
        [Test]
        public void Begin_form_with_enctype()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginForm("/", HttpMethod.Post, EncType.Multipart);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Begin_form_without_enctype()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginForm("/", HttpMethod.Post, null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void End_form()
        {
            var t = new DefaultFormTemplate();

            var result = t.EndForm();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Begin_section()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginSection("Section Title", new HtmlString("<p>hello</p>"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void End_section()
        {
            var t = new DefaultFormTemplate();

            var result = t.EndSection();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Begin_nested_section()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginNestedSection("Section Title", new HtmlString("<p>Hello</p>"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void End_nested_section()
        {
            var t = new DefaultFormTemplate();

            var result = t.EndNestedSection();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field()
        {
            var t = new DefaultFormTemplate();

            var result = t.Field(new HtmlString("<elementhtml>"), new HtmlString("<labelhtml>"), new HtmlString("<validationhtml>"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_field()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginField(new HtmlString("<elementhtml>"), new HtmlString("<labelhtml>"), new HtmlString("<validationhtml>"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_end_field()
        {
            var t = new DefaultFormTemplate();

            var result = t.EndField();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
