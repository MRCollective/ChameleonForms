using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Templates;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component
{
    [TestFixture]
    public class SectionShould
    {
        private readonly IHtmlString _beginHtml = new HtmlString("");
        private readonly IHtmlString _endHtml = new HtmlString("");
        private readonly IHtmlString _nestedBeginHtml = new HtmlString("");
        private readonly IHtmlString _nestedEndHtml = new HtmlString("");
        private IForm<object> _f;
        private readonly IHtmlString _heading = new HtmlString("title");

        [SetUp]
        public void Setup()
        {
            _f = Substitute.For<IForm<object>>();
            _f.Template.BeginSection(Arg.Is<IHtmlString>(h => h.ToHtmlString() == _heading.ToHtmlString()), Arg.Any<IHtmlString>(), Arg.Any<HtmlAttributes>()).Returns(_beginHtml);
            _f.Template.EndSection().Returns(_endHtml);
            _f.Template.BeginNestedSection(Arg.Is<IHtmlString>(h => h.ToHtmlString() == _heading.ToHtmlString()), Arg.Any<IHtmlString>(), Arg.Any<HtmlAttributes>()).Returns(_nestedBeginHtml);
            _f.Template.EndNestedSection().Returns(_nestedEndHtml);
        }

        private ISection<object> Arrange(bool isNested)
        {
            return new Section<object>(_f, _heading, isNested);
        }

        [Test]
        public void Use_section_begin_from_template_for_begin_html()
        {
            var s = Arrange(false);

            Assert.That(s.Begin(), Is.EqualTo(_beginHtml));
        }

        [Test]
        public void Use_section_end_from_template_for_end_html()
        {
            var s = Arrange(false);

            Assert.That(s.End(), Is.EqualTo(_endHtml));
        }

        [Test]
        public void Use_nested_section_begin_from_template_for_nested_begin_html()
        {
            var s = Arrange(true);

            Assert.That(s.Begin(), Is.EqualTo(_nestedBeginHtml));
        }

        [Test]
        public void Use_nested_section_end_from_template_for_nested_end_html()
        {
            var s = Arrange(true);

            Assert.That(s.End(), Is.EqualTo(_nestedEndHtml));
        }

        [Test]
        public void Construct_section_via_extension_method()
        {
            var s = _f.BeginSection(_heading.ToHtmlString());

            Assert.That(s, Is.Not.Null);
            _f.Received().Write(_beginHtml);
        }

        [Test]
        public void Construct_nested_section_via_extension_method()
        {
            var s = _f.BeginSection(_heading.ToHtmlString());
            _f.ClearReceivedCalls();
            var ss = s.BeginSection(_heading.ToHtmlString());

            Assert.That(ss, Is.Not.Null);
            _f.Received().Write(_nestedBeginHtml);
        }
        
        [Test]
        public void Output_a_field([Values(true, false)] bool isValid)
        {
            var labelHtml = Substitute.For<IHtmlString>();
            var elementHtml = Substitute.For<IHtmlString>();
            var validationHtml = Substitute.For<IHtmlString>();
            var metadata = new ModelMetadata(Substitute.For<ModelMetadataProvider>(), null, null, typeof(string), null);
            var expectedOutput = new HtmlString("output");
            _f.Template.Field(labelHtml, elementHtml, validationHtml, metadata, Arg.Any<IReadonlyFieldConfiguration>(), isValid).Returns(expectedOutput);
            var s = Arrange(false);

            var config = s.Field(labelHtml, elementHtml, validationHtml, metadata, isValid: isValid);

            Assert.That(config.ToHtmlString(), Is.EqualTo(expectedOutput.ToHtmlString()));
        }
    }
}
