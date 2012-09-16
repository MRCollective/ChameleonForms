using System.Web;
using ChameleonForms.Component;
using ChameleonForms.Templates;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Components
{
    [TestFixture]
    public class SectionShould
    {
        private readonly IHtmlString _beginHtml = new HtmlString("");
        private readonly IHtmlString _endHtml = new HtmlString("");
        private IForm<object, IFormTemplate> _f;
        private const string Title = "title";

        [SetUp]
        public void Setup()
        {
            _f = Substitute.For<IForm<object, IFormTemplate>>();
            _f.Template.BeginSection(Title).Returns(_beginHtml);
            _f.Template.EndSection().Returns(_endHtml);
        }

        private Section<object, IFormTemplate> Arrange()
        {
            return new Section<object, IFormTemplate>(_f, Title);
        }

        [Test]
        public void Use_section_begin_from_template_for_begin_html()
        {
            var s = Arrange();

            Assert.That(s.Begin(), Is.EqualTo(_beginHtml));
        }

        [Test]
        public void Use_section_end_from_template_for_end_html()
        {
            var s = Arrange();

            Assert.That(s.End(), Is.EqualTo(_endHtml));
        }

        [Test]
        public void Construct_section_via_extension_method()
        {
            var s = _f.BeginSection(Title);

            Assert.That(s, Is.Not.Null);
            _f.Received().Write(_beginHtml);
        }
    }
}
