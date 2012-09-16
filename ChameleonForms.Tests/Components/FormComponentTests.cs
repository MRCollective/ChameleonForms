using System.Web;
using ChameleonForms.Component;
using ChameleonForms.Templates;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Components
{
    public static class Self
    {
        public static readonly bool Closing = true;
    }

    [TestFixture]
    class FormComponentShould
    {
       private readonly IHtmlString _beginHtml = new HtmlString("");
       private readonly IHtmlString _endHtml = new HtmlString("");

        public FormComponent<object, IFormTemplate> Arrange(bool selfClosing)
        {
            var f = Substitute.For<FormComponent<object, IFormTemplate>>(Substitute.For<IForm<object, IFormTemplate>>(), selfClosing);
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
            f.Form.DidNotReceive().Write(Arg.Is<IHtmlString>(h => h != _beginHtml));
        }

        [Test]
        public void Write_end_html_when_disposing_if_not_self_closing()
        {
            var f = Arrange(!Self.Closing);

            f.Dispose();

            f.Form.DidNotReceive().Write(Arg.Is<IHtmlString>(h => h != _endHtml));
            f.Form.Received(1).Write(_endHtml);
        }

        [Test]
        public void Not_write_begin_or_end_html_when_constructing_and_disposing_if_self_closing()
        {
            var f = Arrange(Self.Closing);

            f.Initialise();
            f.Dispose();

            f.Form.DidNotReceive().Write(Arg.Any<IHtmlString>());
        }

        [Test]
        public void Return_null_when_serialising_html_string_if_not_self_closing()
        {
            var f = Arrange(!Self.Closing);

            Assert.That(f.ToHtmlString(), Is.Null);
        }

        [Test]
        public void Return_html_when_serialising_html_string_if_self_closing()
        {
            var f = Arrange(Self.Closing);

            Assert.That(f.ToHtmlString(), Is.EqualTo(_beginHtml.ToHtmlString() + _endHtml.ToHtmlString()));
        }
    }
}
