using System.Web;
using ChameleonForms.Component;
using ChameleonForms.Templates;
using Microsoft.AspNetCore.Html;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component
{
    public static class Self
    {
        public static readonly bool Closing = true;
    }

    [TestFixture]
    class FormComponentShould
    {
       private readonly IHtmlContent _beginHtml = new HtmlString("");
       private readonly IHtmlContent _endHtml = new HtmlString("");

        public FormComponent<object> Arrange(bool selfClosing)
        {
            var f = Substitute.For<FormComponent<object>>(Substitute.For<IForm<object>>(), selfClosing);
            f.Begin().Returns(_beginHtml);
            f.End().Returns(_endHtml);
            return f;
        }

        [Test]
        public void Write_begin_html_when_constructing_if_not_self_closing()
        {
            var f = Arrange(!Self.Closing);

            f.Initialise();

            f.Form.Received(1).Write(_beginHtml);
            f.Form.DidNotReceive().Write(Arg.Is<IHtmlContent>(h => h != _beginHtml));
        }

        [Test]
        public void Write_end_html_when_disposing_if_not_self_closing()
        {
            var f = Arrange(!Self.Closing);

            f.Dispose();

            f.Form.DidNotReceive().Write(Arg.Is<IHtmlContent>(h => h != _endHtml));
            f.Form.Received(1).Write(_endHtml);
        }

        [Test]
        public void Not_write_begin_or_end_html_when_constructing_and_disposing_if_self_closing()
        {
            var f = Arrange(Self.Closing);

            f.Initialise();
            f.Dispose();

            f.Form.DidNotReceive().Write(Arg.Any<IHtmlContent>());
        }

        [Test]
        public void Return_empty_string_when_serialising_html_string_if_not_self_closing()
        {
            var f = Arrange(!Self.Closing);

            Assert.That(f.ToHtmlString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void Return_html_when_serialising_html_string_if_self_closing()
        {
            var f = Arrange(Self.Closing);

            Assert.That(f.ToHtmlString(), Is.EqualTo(_beginHtml.ToHtmlString() + _endHtml.ToHtmlString()));
        }
    }
}
