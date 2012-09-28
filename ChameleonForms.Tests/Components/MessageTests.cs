using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component;
using ChameleonForms.Enums;
using ChameleonForms.Templates;
using ChameleonForms.Tests.Helpers;
using FizzWare.NBuilder;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Components
{
    [TestFixture]
    public class MessageShould
    {
        private const string TestHeading = "TestHeading";
        private readonly IHtmlString _beginHtml = new HtmlString("");
        private readonly IHtmlString _endHtml = new HtmlString("");
        private IForm<object, IFormTemplate> _f;

        [SetUp]
        public void Setup()
        {
            _f = Substitute.For<IForm<object, IFormTemplate>>();
            _f.Template.BeginMessage(Arg.Any<MessageType>(), Arg.Any<string>()).Returns(_beginHtml);
            _f.Template.EndMessage().Returns(_endHtml);
        }

        private Message<object, IFormTemplate> Arrange(MessageType messageType)
        {
            return new Message<object, IFormTemplate>(_f, messageType, TestHeading);
        }

        private static readonly MessageType[] MessageTypes = EnumHelper.GetValues<MessageType>();

        [Test]
        public void Use_message_begin_from_template_for_begin_html([ValueSource("MessageTypes")] MessageType messageType)
        {
            var s = Arrange(messageType);

            Assert.That(s.Begin(), Is.EqualTo(_beginHtml));
            _f.Template.Received().BeginMessage(messageType, TestHeading);
        }
        
        [Test]
        public void Use_message_end_from_template_for_end_html([ValueSource("MessageTypes")] MessageType messageType)
        {
            var s = Arrange(messageType);

            Assert.That(s.End(), Is.EqualTo(_endHtml));
            _f.Template.Received().EndMessage();
        }

        [Test]
        public void Construct_section_via_extension_method([ValueSource("MessageTypes")] MessageType messageType)
        {
            var s = _f.BeginMessage(messageType, TestHeading);

            Assert.That(s, Is.Not.Null);
            _f.Received().Write(_beginHtml);
        }

        [Test]
        public void Wrap_message_in_a_paragraph_html_tag()
        {
            var s = Arrange(MessageType.Success);

            var h = s.Paragraph("aerg&%^\"esrg'");

            Assert.That(h.ToString(), Is.EqualTo("<p>aerg&amp;%^&quot;esrg&#39;</p>\r\n"));
        }
    }
}