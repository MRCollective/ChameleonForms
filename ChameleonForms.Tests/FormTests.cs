using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Templates;
using ChameleonForms.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests
{
    [TestFixture]
    class FormShould
    {
        #region Setup
        private Form<object, DummyFormTemplate> _f;
        private HtmlHelper<object> _helper;
        private ViewContext _viewContext;

        [SetUp]
        public void Setup()
        {
            _viewContext = Substitute.For<ViewContext>();
            _viewContext.Writer = Substitute.For<TextWriter>();
            _helper = new HtmlHelper<object>(_viewContext, new ViewPage());
        }

        private void CreateForm()
        {
            _f = new Form<object, DummyFormTemplate>(_helper, string.Empty, HttpMethod.Get, string.Empty);
        }
        #endregion

        [Test]
        public void Store_html_helper()
        {
            CreateForm();

            Assert.That(_f.HtmlHelper, Is.EqualTo(_helper));
        }

        [Test]
        public void Store_template()
        {
            CreateForm();

            Assert.That(_f.Template, Is.Not.Null);
        }

        [Test]
        public void Write_start_of_form_on_construction()
        {
            var t = new DummyFormTemplate();

            CreateForm();

            _viewContext.Writer.Received().Write(Arg.Is<IHtmlString>(h => h.ToString() == t.BeginForm(string.Empty, HttpMethod.Get, string.Empty).ToString()));
            _viewContext.Writer.DidNotReceive().Write(Arg.Is<IHtmlString>(h => h.ToString() == t.EndForm().ToString()));
        }

        [Test]
        public void Write_end_of_form_on_disposal()
        {
            var t = new DummyFormTemplate();
            CreateForm();

            _f.Dispose();

            _viewContext.Writer.Received().Write(Arg.Is<IHtmlString>(h => h.ToString() == t.EndForm().ToString()));
        }

        [Test]
        public void Construct_form_via_extension_method()
        {
            var t = new DefaultFormTemplate();

            var f2 = _helper.BeginChameleonForm("action", HttpMethod.Post, "enctype");

            Assert.That(f2, Is.Not.Null);
            _viewContext.Writer.Received().Write(Arg.Is<IHtmlString>(h => h.ToString() == t.BeginForm("action", HttpMethod.Post, "enctype").ToString()));
        }
    }
}
