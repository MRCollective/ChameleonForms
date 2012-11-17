using System.Collections.Generic;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component;
using NUnit.Framework;

namespace ChameleonForms.Tests.Components.Config
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class FieldConfigurationShould
    {
        [Test]
        public void ProxyHtmlAttributes()
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
    }
}
