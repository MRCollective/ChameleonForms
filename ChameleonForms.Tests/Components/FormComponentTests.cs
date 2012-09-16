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

    class MockFormComponent : FormComponent<object, IFormTemplate>
    {
        public readonly IHtmlString BeginHtml = new HtmlString("begin");
        public readonly IHtmlString EndHtml = new HtmlString("end");

        public MockFormComponent(IForm<object, IFormTemplate> form, bool isSelfClosing) : base(form, isSelfClosing) {}

        public override IHtmlString Begin() { return BeginHtml; }
        public override IHtmlString End() { return EndHtml; }
    }

    [TestFixture]
    class FormComponentShould
    {

        public MockFormComponent Arrange(bool selfClosing)
        {
            return new MockFormComponent(Substitute.For<IForm<object, IFormTemplate>>(), selfClosing);
        }

        [Test]
        public void Write_begin_html_when_constructing_if_not_self_closing()
        {
            var f = Arrange(!Self.Closing);

            f.Form.Received(1).Write(f.BeginHtml);
            f.Form.DidNotReceive().Write(Arg.Is<IHtmlString>(h => h != f.BeginHtml));
        }

        [Test]
        public void Write_end_html_when_disposing_if_not_self_closing()
        {
            var f = Arrange(!Self.Closing);

            f.Dispose();

            f.Form.Received(1).Write(f.BeginHtml);
            f.Form.Received(1).Write(f.EndHtml);
        }

        [Test]
        public void Not_write_begin_or_end_html_when_constructing_and_disposing_if_self_closing()
        {
            var f = Arrange(Self.Closing);
            
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

            Assert.That(f.ToHtmlString(), Is.EqualTo(f.BeginHtml.ToHtmlString() + f.EndHtml.ToHtmlString()));
        }
    }
}
