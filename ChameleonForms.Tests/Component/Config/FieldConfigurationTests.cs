using System.Collections.Generic;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component.Config
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class FieldConfigurationShould
    {
        [Test]
        public void Proxy_html_attributes()
        {
            var fc = Field.Configure()
                .Attr("data-attr1", "value")
                .Attr(data_attr2 => "value")
                .Attrs(new Dictionary<string, object> {{"data-attr3", "value"}})
                .Attrs(new {data_attr4 = "value"})
                .Attrs(data_attr5 => "value")
                .AddClass("someclass");

            HtmlApprovals.VerifyHtml(fc.Attributes.ToHtmlString());
        }

        [Test]
        public void Set_textarea_attributes()
        {
            var fc = Field.Configure()
                .Rows(5)
                .Cols(60);

            HtmlApprovals.VerifyHtml(fc.Attributes.ToHtmlString());
        }

        [Test]
        public void Set_inline_label()
        {
            var fc = Field.Configure()
                .InlineLabel("Some label text");

            Assert.That(fc.InlineLabelText, Is.EqualTo("Some label text"));
        }
    }
}
