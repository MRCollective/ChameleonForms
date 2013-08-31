using System;
using System.Web;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component;
using ChameleonForms.Templates;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class NavigationShould
    {
        private readonly IHtmlString _beginHtml = new HtmlString("");
        private readonly IHtmlString _endHtml = new HtmlString("");
        private readonly IHtmlString _buttonHtml = new HtmlString("");
        private readonly IHtmlString _html = new HtmlString("");
        private IForm<object, IFormTemplate> _f;
        private readonly HtmlAttributes _attrs = new HtmlAttributes(new { @class = "someclass" });

        [SetUp]
        public void Setup()
        {
            _f = Substitute.For<IForm<object, IFormTemplate>>();
            _f.Template.BeginNavigation().Returns(_beginHtml);
            _f.Template.EndNavigation().Returns(_endHtml);
            _f.Template.Button(null, null, null, null, null).ReturnsForAnyArgs(_buttonHtml);
        }

        private Navigation<object, IFormTemplate> Arrange()
        {
            return new Navigation<object, IFormTemplate>(_f);
        }

        [Test]
        public void Use_navigation_begin_from_template_for_begin_html()
        {
            var n = Arrange();

            Assert.That(n.Begin(), Is.EqualTo(_beginHtml));
        }

        [Test]
        public void Use_navigation_end_from_template_for_end_html()
        {
            var n = Arrange();

            Assert.That(n.End(), Is.EqualTo(_endHtml));
        }

        [Test]
        public void Construct_navigation_via_extension_method()
        {
            var n = _f.BeginNavigation();

            Assert.That(n, Is.Not.Null);
            _f.Received().Write(_beginHtml);
        }

        [Test]
        public void Construct_text_button()
        {
            var n = Arrange();

            Assert.That(n.Button("te&xt", _attrs), Is.EqualTo(_buttonHtml));
            _f.Template.Received().Button(Arg.Is<IHtmlString>(h => h.ToHtmlString() == "te&amp;xt"), null, null, null, _attrs);
        }

        [Test]
        public void Construct_html_button()
        {
            var n = Arrange();

            Assert.That(n.Button(_html, _attrs), Is.EqualTo(_buttonHtml));
            _f.Template.Received().Button(_html, null, null, null, _attrs);
        }

        [Test]
        public void Construct_text_reset_button()
        {
            var n = Arrange();

            Assert.That(n.Reset("te&xt", _attrs), Is.EqualTo(_buttonHtml));
            _f.Template.Received().Button(Arg.Is<IHtmlString>(h => h.ToHtmlString() == "te&amp;xt"), "reset", null, null, _attrs);
        }

        [Test]
        public void Construct_html_reset_button()
        {
            var n = Arrange();

            Assert.That(n.Reset(_html, _attrs), Is.EqualTo(_buttonHtml));
            _f.Template.Received().Button(_html, "reset", null, null, _attrs);
        }

        [Test]
        public void Construct_text_submit_button()
        {
            var n = Arrange();

            Assert.That(n.Submit("te&xt", _attrs), Is.EqualTo(_buttonHtml));
            _f.Template.Received().Button(Arg.Is<IHtmlString>(h => h.ToHtmlString() == "te&amp;xt"), "submit", null, null, _attrs);
        }

        [Test]
        public void Construct_html_submit_button()
        {
            var n = Arrange();

            Assert.That(n.Submit(_html, _attrs), Is.EqualTo(_buttonHtml));
            _f.Template.Received().Button(_html, "submit", null, null, _attrs);
        }

        [Test]
        public void Construct_html_submit_button_with_value()
        {
            var n = Arrange();

            Assert.That(n.Submit("name", "value", _html, _attrs), Is.EqualTo(_buttonHtml));
            _f.Template.Received().Button(_html, "submit", "name", "value", _attrs);
        }

        [Test]
        public void Throw_exception_when_submit_button_with_value_has_null_value()
        {
            var n = Arrange();

            var e = Assert.Throws<ArgumentNullException>(() => n.Submit("name", null, _html, _attrs));

            Assert.That(e.ParamName, Is.EqualTo("value"));
        }

        [Test]
        public void Throw_exception_when_button_has_no_content()
        {
            var n = Arrange();

            var e = Assert.Throws<ArgumentNullException>(() => n.Button(default(IHtmlString), _attrs));

            Assert.That(e.ParamName, Is.EqualTo("content"));
        }

        [Test]
        public void Throw_exception_when_reset_button_has_no_content()
        {
            var n = Arrange();

            var e = Assert.Throws<ArgumentNullException>(() => n.Reset(default(IHtmlString), _attrs));

            Assert.That(e.ParamName, Is.EqualTo("content"));
        }

        [Test]
        public void Throw_exception_when_submit_button_has_no_content()
        {
            var n = Arrange();

            var e = Assert.Throws<ArgumentNullException>(() => n.Submit(default(IHtmlString), _attrs));

            Assert.That(e.ParamName, Is.EqualTo("content"));
        }
    }
}
