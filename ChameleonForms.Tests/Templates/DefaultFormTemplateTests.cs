using System;
using System.Web;
using System.Web.Mvc;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component.Config;
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

            var result = t.BeginForm("/", FormMethod.Post, new HtmlAttributes(data_attr => "value"), EncType.Multipart);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Begin_form_without_enctype()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginForm("/", FormMethod.Post, null, null);

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

            var result = t.BeginSection(new HtmlString("Section Title"), new HtmlString("<p>hello</p>"), new { @class = "asdf" }.ToHtmlAttributes());

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

            var result = t.BeginNestedSection(new HtmlString("Section Title"), new HtmlString("<p>Hello</p>"), new { @class = "asdf" }.ToHtmlAttributes());

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

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, null, false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_hint()
        {
            var t = new DefaultFormTemplate();

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration().WithHint("hello").ToReadonly(), false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html()
        {
            var t = new DefaultFormTemplate();

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration()
                .Prepend(new HtmlString("<1>")).Prepend(new HtmlString("<2>"))
                .Append(new HtmlString("<3>")).Append(new HtmlString("<4>"))
                .ToReadonly(),
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_field()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginField(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, null, false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_end_field()
        {
            var t = new DefaultFormTemplate();

            var result = t.EndField();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_navigation()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginNavigation();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_end_navigation()
        {
            var t = new DefaultFormTemplate();

            var result = t.EndNavigation();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_information_message()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginMessage(MessageType.Information, new HtmlString("Heading"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_information_message_without_heading()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginMessage(MessageType.Information, new HtmlString(""));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_failure_message()
        {
            var t = new DefaultFormTemplate();

            var result = t.BeginMessage(MessageType.Failure, new HtmlString("Heading"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_end_message()
        {
            var t = new DefaultFormTemplate();

            var result = t.EndMessage();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_message_paragraph()
        {
            var t = new DefaultFormTemplate();

            var result = t.MessageParagraph(new HtmlString("<strong>asdf</strong>"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_button_input_when_button_with_no_content_specified()
        {
            var t = new DefaultFormTemplate();

            var result = t.Button(null, null, null, "value", null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
        
        [Test]
        public void Output_submit_input_when_button_with_no_content_and_submit_type_specified()
        {
            var t = new DefaultFormTemplate();

            var result = t.Button(null, "submit", "id", "value", new HtmlAttributes(@class => "asdf"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_button_when_button_with_content_specified()
        {
            var t = new DefaultFormTemplate();

            var result = t.Button(new HtmlString("<strong>asdf</strong>"), null, null, null, null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
        
        [Test]
        public void Output_submit_button_when_button_with_content_and_submit_type_specified()
        {
            var t = new DefaultFormTemplate();

            var result = t.Button(new HtmlString("<strong>asdf</strong>"), "submit", "id", "value", new HtmlAttributes(@class => "asdf"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Throw_exception_when_nothing_for_user_to_see()
        {
            var t = new DefaultFormTemplate();

            var e = Assert.Throws<ArgumentNullException>(() => t.Button(null, "type", "id", null, new HtmlAttributes(@class => "asdf")));

            Assert.That(e.ParamName, Is.EqualTo("content"));
        }

        [Test]
        public void Allow_for_name_to_be_specified_but_id_to_be_overwritten_when_creating_a_button()
        {
            var t = new DefaultFormTemplate();

            var result = t.Button(new HtmlString("a"), null, "name", null, new HtmlAttributes().Attr("id", "asdf"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
