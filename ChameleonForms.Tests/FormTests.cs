using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Autofac;
using AutofacContrib.NSubstitute;
using ChameleonForms.Enums;
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
        private AutoSubstitute _autoSubstitute;
        private HtmlHelper<object> _h;
        private IFormTemplate _t;

        private readonly IHtmlString _beginHtml = new HtmlString("");
        private readonly IHtmlString _endHtml = new HtmlString("");

        private readonly string _action = "/";
        private readonly HttpMethod _method = HttpMethod.Post;
        private readonly EncType _enctype = EncType.Multipart;

        [SetUp]
        public void Setup()
        {
            _autoSubstitute = AutoSubstituteContainer.Create();
            // todo: Update AutoSubstitute and change these to ResolveAndSubstituteFor calls
            _h = _autoSubstitute.Provide(_autoSubstitute.Resolve<HtmlHelper<object>>());
            _t = _autoSubstitute.Resolve<IFormTemplate>();
            _t.BeginForm(_action, _method, _enctype).Returns(_beginHtml);
            _t.EndForm().Returns(_endHtml);
        }

        private Form<object, IFormTemplate> CreateForm()
        {
            return _autoSubstitute.Resolve<Form<object, IFormTemplate>>(
                new NamedParameter("action", _action),
                new NamedParameter("method", _method),
                new NamedParameter("enctype", _enctype)
            );
        }
        #endregion

        [Test]
        public void Store_html_helper()
        {
            var f = CreateForm();

            Assert.That(f.HtmlHelper, Is.EqualTo(_h));
        }

        [Test]
        public void Store_template()
        {
            var f = CreateForm();

            Assert.That(f.Template, Is.EqualTo(_t));
        }

        [Test]
        public void Write_start_of_form_on_construction()
        {
            CreateForm();

            _h.ViewContext.Writer.Received().Write(_beginHtml);
            _h.ViewContext.Writer.DidNotReceive().Write(_endHtml);
        }

        [Test]
        public void Write_end_of_form_on_disposal()
        {
            var f = CreateForm();

            f.Dispose();

            _h.ViewContext.Writer.Received().Write(_endHtml);
        }

        [Test]
        public void Construct_form_via_extension_method()
        {
            var t = new DefaultFormTemplate();

            var f2 = _h.BeginChameleonForm(_action, _method);

            Assert.That(f2, Is.Not.Null);
            _h.ViewContext.Writer.Received().Write(Arg.Is<IHtmlString>(h => h.ToHtmlString() == t.BeginForm(_action, _method, null).ToHtmlString()));
        }
    }
}
