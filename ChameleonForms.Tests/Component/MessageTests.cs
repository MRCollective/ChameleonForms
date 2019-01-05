using System.Web;
using ChameleonForms.Component;
using ChameleonForms.Enums;
using ChameleonForms.Templates;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Html;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component
{
    [TestFixture]
    public class MessageShould
    {
        private readonly IHtmlContent _testHeading = new HtmlString("TestHeading");
        private readonly IHtmlContent _beginHtml = new HtmlString("");
        private readonly IHtmlContent _endHtml = new HtmlString("");
        private IForm<object> _f;

        [SetUp]
        public void Setup()
        {
            _f = Substitute.For<IForm<object>>();
            _f.Template.BeginMessage(Arg.Any<MessageType>(), Arg.Any<IHtmlContent>(), Arg.Any<bool>()).Returns(_beginHtml);
            _f.Template.EndMessage().Returns(_endHtml);
        }

        private Message<object> Arrange(MessageType messageType)
        {
            return new Message<object>(_f, messageType, _testHeading);
        }

        private static readonly MessageType[] MessageTypes = EnumHelper.GetValues<MessageType>();

        [Test]
        public void Use_message_begin_from_template_for_begin_html([ValueSource("MessageTypes")] MessageType messageType)
        {
            var s = Arrange(messageType);

            Assert.That(s.Begin(), Is.EqualTo(_beginHtml));
            _f.Template.Received().BeginMessage(messageType, _testHeading, false);
        }
        
        [Test]
        public void Use_message_end_from_template_for_end_html([ValueSource("MessageTypes")] MessageType messageType)
        {
            var s = Arrange(messageType);

            Assert.That(s.End(), Is.EqualTo(_endHtml));
            _f.Template.Received().EndMessage();
        }

        [Test]
        public void Construct_section_via_extension_method_with_heading([ValueSource("MessageTypes")] MessageType messageType)
        {
            var s = _f.BeginMessage(messageType, "TestHeading");

            Assert.That(s, Is.Not.Null);
            _f.Received().Write(_beginHtml);
        }

        [Test]
        public void Construct_section_via_extension_method_without_heading([ValueSource("MessageTypes")] MessageType messageType)
        {
            var s = _f.BeginMessage(messageType);

            Assert.That(s, Is.Not.Null);
            _f.Received().Write(_beginHtml);
        }

        //[Test]
        //public void Create_a_paragraph_with_a_string()
        //{
        //    var html = Substitute.For<IHtmlContent>();
        //    var s = Arrange(MessageType.Success);
        //    _f.Template.MessageParagraph(Arg.Is<IHtmlContent>(h => h.ToHtmlString() == "aerg&amp;%^&quot;esrg&#39;"))
        //        .Returns(html);

        //    var paragraph = s.Paragraph("aerg&%^\"esrg'");

        //    Assert.That(paragraph, Is.EqualTo(html));
        //}

        [Test]
        public void Create_a_paragraph_with_html()
        {
            var inputHtml = Substitute.For<IHtmlContent>();
            var outputHtml = Substitute.For<IHtmlContent>();
            var s = Arrange(MessageType.Success);
            _f.Template.MessageParagraph(inputHtml).Returns(outputHtml);

            var paragraph = s.Paragraph(inputHtml);

            Assert.That(paragraph, Is.EqualTo(outputHtml));
        }
    }
}