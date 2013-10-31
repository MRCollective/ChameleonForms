using System.Web;
using System.Web.Mvc;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component.Config;
using ChameleonForms.Templates.TwitterBootstrap3;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates.TwitterBootstrap3
{
    [UseReporter(typeof(DiffReporter))]
    class FieldTests_TwitterBootstrapTemplateShould
    {
        [Test]
        public void Output_field()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, null, false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_hint()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration().WithHint("hello").ToReadonly(), false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration()
                .Prepend(new HtmlString("<1>")).Prepend(new HtmlString("<2>"))
                .Append(new HtmlString("<3>")).Append(new HtmlString("<4>"))
                .WithHint(new HtmlString("<hint>"))
                .ToReadonly(),
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html_when_input_group()
        {
            var t = new TwitterBootstrapFormTemplate();
            var metadata = new ModelMetadata(new EmptyModelMetadataProvider(), typeof(object), () => null, typeof(object), "");
            metadata.IsRequired = true;

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), metadata, new FieldConfiguration()
                .Prepend(new HtmlString("<1>")).Prepend(new HtmlString("<2>"))
                .Append(new HtmlString("<3>")).Append(new HtmlString("<4>"))
                .WithHint(new HtmlString("<hint>"))
                .AsInputGroup()
                .ToReadonly(),
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_field_with_prepended_and_appended_html_when_required()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.Field(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, new FieldConfiguration()
                .Prepend(new HtmlString("<1>")).Prepend(new HtmlString("<2>"))
                .Append(new HtmlString("<3>")).Append(new HtmlString("<4>"))
                .WithHint(new HtmlString("<hint>"))
                .ToReadonly(),
                false
            );

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_begin_field()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.BeginField(new HtmlString("labelhtml"), new HtmlString("elementhtml"), new HtmlString("validationhtml"), null, null, false);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Output_end_field()
        {
            var t = new TwitterBootstrapFormTemplate();

            var result = t.EndField();

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

    }
}
