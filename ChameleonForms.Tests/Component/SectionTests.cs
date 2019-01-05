using System.Web;

using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Templates;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component
{
    [TestFixture]
    public class SectionShould
    {
        private readonly IHtmlContent _beginHtml = new HtmlString("");
        private readonly IHtmlContent _endHtml = new HtmlString("");
        private readonly IHtmlContent _nestedBeginHtml = new HtmlString("");
        private readonly IHtmlContent _nestedEndHtml = new HtmlString("");
        private IForm<object> _f;
        private readonly IHtmlContent _heading = new HtmlString("title");

        [SetUp]
        public void Setup()
        {
            _f = Substitute.For<IForm<object>>();
            _f.Template.BeginSection(Arg.Is<IHtmlContent>(h => h.ToHtmlString() == _heading.ToHtmlString()), Arg.Any<IHtmlContent>(), Arg.Any<HtmlAttributes>()).Returns(_beginHtml);
            _f.Template.EndSection().Returns(_endHtml);
            _f.Template.BeginNestedSection(Arg.Is<IHtmlContent>(h => h.ToHtmlString() == _heading.ToHtmlString()), Arg.Any<IHtmlContent>(), Arg.Any<HtmlAttributes>()).Returns(_nestedBeginHtml);
            _f.Template.EndNestedSection().Returns(_nestedEndHtml);
        }

        private Section<object> Arrange(bool isNested)
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
            var s = _f.BeginSection(_heading);

            Assert.That(s, Is.Not.Null);
            _f.Received().Write(_beginHtml);
        }

        [Test]
        public void Construct_nested_section_via_extension_method()
        {
            var s = _f.BeginSection(_heading);
            _f.ClearReceivedCalls();
            var ss = s.BeginSection(_heading);

            Assert.That(ss, Is.Not.Null);
            _f.Received().Write(_nestedBeginHtml);
        }
        
        [Test]
        public void Output_a_field([Values(true, false)] bool isValid)
        {
            var labelHtml = Substitute.For<IHtmlContent>();
            var elementHtml = Substitute.For<IHtmlContent>();
            var validationHtml = Substitute.For<IHtmlContent>();
            var metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(string));
            var expectedOutput = new HtmlString("output");
            _f.Template.Field(labelHtml, elementHtml, validationHtml, metadata, Arg.Any<IReadonlyFieldConfiguration>(), isValid).Returns(expectedOutput);
            var s = Arrange(false);

            var config = s.Field(labelHtml, elementHtml, validationHtml, metadata, isValid: isValid);

            Assert.That(config.ToHtmlString(), Is.EqualTo(expectedOutput.ToHtmlString()));
        }
    }
}
