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
        private IForm<object, IFormTemplate> _f;
        private readonly HtmlAttributes _attrs = new HtmlAttributes(new { @class = "someclass" });

        [SetUp]
        public void Setup()
        {
            _f = Substitute.For<IForm<object, IFormTemplate>>();
            _f.Template.BeginNavigation().Returns(_beginHtml);
            _f.Template.EndNavigation().Returns(_endHtml);
        }

        private Navigation<object, IFormTemplate> Arrange()
        {
            return new Navigation<object, IFormTemplate>(_f);
        }

        [Test]
        public void Use_navigation_begin_from_template_for_begin_html()
        {
            var s = Arrange();

            Assert.That(s.Begin(), Is.EqualTo(_beginHtml));
        }

        [Test]
        public void Use_navigation_end_from_template_for_end_html()
        {
            var s = Arrange();

            Assert.That(s.End(), Is.EqualTo(_endHtml));
        }

        [Test]
        public void Construct_navigation_via_extension_method()
        {
            var s = _f.BeginNavigation();

            Assert.That(s, Is.Not.Null);
            _f.Received().Write(_beginHtml);
        }

        [Test]
        public void Construct_submit_button()
        {
            var s = Arrange();

            var h = s.Submit("Text&", _attrs);

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Construct_submit_button_with_value()
        {
            var s = Arrange();

            var h = s.Submit("name", "value&", new HtmlString("<strong>value</strong>"), _attrs);

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Construct_submit_input_with_value()
        {
            var s = Arrange();

            var h = s.Submit("name", "value&", htmlAttributes: _attrs);

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Construct_submit_button_with_html_contents()
        {
            var s = Arrange();

            var h = s.Submit(new HtmlString("<strong>hello</strong>"), _attrs);

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Construct_reset_button()
        {
            var s = Arrange();

            var h = s.Reset("value&", _attrs);

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Construct_reset_button_with_html_contents()
        {
            var s = Arrange();

            var h = s.Reset(new HtmlString("<strong>value</strong>"), _attrs);

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Construct_button()
        {
            var s = Arrange();

            var h = s.Button("value&", _attrs);

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Construct_button_with_html_contents()
        {
            var s = Arrange();

            var h = s.Button(new HtmlString("<strong>value</strong>"), _attrs);

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }
    }
}
