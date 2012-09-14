using System.IO;
using System.Net.Http;
using System.Web.Mvc;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;

namespace ChameleonForms.Tests
{
    [TestFixture]
    class FormShould
    {
        [Test]
        public void Store_html_helper()
        {
            var helper = new HtmlHelper<object>(new ViewContext(), new ViewPage());
            helper.ViewContext.Writer = new StringWriter();
            var f = new Form<object, DummyFormTemplate>(helper, string.Empty, HttpMethod.Get, string.Empty);

            Assert.That(f.HtmlHelper, Is.EqualTo(helper));
        }
    }
}
