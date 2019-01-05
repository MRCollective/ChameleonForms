using System;
using System.Web;
using ApprovalTests.Reporters;
using ChameleonForms.Component;
using ChameleonForms.Templates;
using Microsoft.AspNetCore.Html;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class NavigationShould
    {
        private readonly IHtmlContent _beginHtml = new HtmlString("");
        private readonly IHtmlContent _endHtml = new HtmlString("");
        private const string ButtonHtml = "buttonHtml";
        private readonly IHtmlContent _html = new HtmlString("");
        private IForm<object> _f;

        [SetUp]
        public void Setup()
        {
            _f = Substitute.For<IForm<object>>();
            _f.Template.BeginNavigation().Returns(_beginHtml);
            _f.Template.EndNavigation().Returns(_endHtml);
            _f.Template.Button(null, null, null, null, null).ReturnsForAnyArgs(new HtmlString(ButtonHtml));
        }

        private Navigation<object> Arrange()
        {
            return new Navigation<object>(_f);
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

            var attrs = n.Button("te&xt");

            Assert.That(attrs.ToHtmlString(), Is.EqualTo(ButtonHtml));
            _f.Template.Received().Button(Arg.Is<IHtmlContent>(h => h.ToHtmlString() == "te&amp;xt"), null, null, null, attrs);
        }

        [Test]
        public void Construct_html_button()
        {
            var n = Arrange();

            var attrs = n.Button(_html);

            Assert.That(attrs.ToHtmlString(), Is.EqualTo(ButtonHtml));
            _f.Template.Received().Button(_html, null, null, null, attrs);
        }

        [Test]
        public void Construct_text_reset_button()
        {
            var n = Arrange();

            var attrs = n.Reset("te&xt");

            Assert.That(attrs.ToHtmlString(), Is.EqualTo(ButtonHtml));
            _f.Template.Received().Button(Arg.Is<IHtmlContent>(h => h.ToHtmlString() == "te&amp;xt"), "reset", null, null, attrs);
        }

        [Test]
        public void Construct_html_reset_button()
        {
            var n = Arrange();

            var attrs = n.Reset(_html);

            Assert.That(attrs.ToHtmlString(), Is.EqualTo(ButtonHtml));
            _f.Template.Received().Button(_html, "reset", null, null, attrs);
        }

        [Test]
        public void Construct_text_submit_button()
        {
            var n = Arrange();

            var attrs = n.Submit("te&xt");

            Assert.That(attrs.ToHtmlString(), Is.EqualTo(ButtonHtml));
            _f.Template.Received().Button(Arg.Is<IHtmlContent>(h => h.ToHtmlString() == "te&amp;xt"), "submit", null, null, attrs);
        }

        [Test]
        public void Construct_html_submit_button()
        {
            var n = Arrange();

            var attrs = n.Submit(_html);

            Assert.That(attrs.ToHtmlString(), Is.EqualTo(ButtonHtml));
            _f.Template.Received().Button(_html, "submit", null, null, attrs);
        }

        [Test]
        public void Construct_html_submit_button_with_value()
        {
            var n = Arrange();

            var attrs = n.Submit("name", "value", _html);

            Assert.That(attrs.ToHtmlString(), Is.EqualTo(ButtonHtml));
            _f.Template.Received().Button(_html, "submit", "name", "value", attrs);
        }

        [Test]
        public void Throw_exception_when_submit_button_with_value_has_null_value()
        {
            var n = Arrange();

            var e = Assert.Throws<ArgumentNullException>(() => n.Submit("name", null, _html));

            Assert.That(e.ParamName, Is.EqualTo("value"));
        }

        [Test]
        public void Throw_exception_when_button_has_no_content()
        {
            var n = Arrange();

            var e = Assert.Throws<ArgumentNullException>(() => n.Button(default(IHtmlContent)));

            Assert.That(e.ParamName, Is.EqualTo("content"));
        }

        [Test]
        public void Throw_exception_when_reset_button_has_no_content()
        {
            var n = Arrange();

            var e = Assert.Throws<ArgumentNullException>(() => n.Reset(default(IHtmlContent)));

            Assert.That(e.ParamName, Is.EqualTo("content"));
        }

        [Test]
        public void Throw_exception_when_submit_button_has_no_content()
        {
            var n = Arrange();

            var e = Assert.Throws<ArgumentNullException>(() => n.Submit(default(IHtmlContent)));

            Assert.That(e.ParamName, Is.EqualTo("content"));
        }
    }
}
