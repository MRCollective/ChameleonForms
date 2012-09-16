using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component;
using ChameleonForms.FieldGenerator;
using ChameleonForms.Templates;
using ChameleonForms.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Components
{
    public class TestFieldViewModel
    {
        public string SomeProperty { get; set; }
    }

    [TestFixture]
    public class FieldShould
    {
        #region Setup
        private readonly IHtmlString _beginHtml = new HtmlString("b");
        private readonly IHtmlString _endHtml = new HtmlString("e");
        private readonly IHtmlString _html = new HtmlString("h");
        private readonly IHtmlString _label = new HtmlString("l");
        private readonly IHtmlString _field = new HtmlString("f");
        private readonly IHtmlString _validation = new HtmlString("v");
        private IForm<TestFieldViewModel, IFormTemplate> _f;
        private IFieldGenerator _g;

        [SetUp]
        public void Setup()
        {
            _f = Substitute.For<IForm<TestFieldViewModel, IFormTemplate>>();
            _f.Template.BeginField(_label, _field, _validation).Returns(_beginHtml);
            _f.Template.Field(_label, _field, _validation).Returns(_html);
            _f.Template.EndField().Returns(_endHtml);

            var autoSubstitute = AutoSubstituteContainer.Create();
            var helper = autoSubstitute.Resolve<HtmlHelper<TestFieldViewModel>>();
            _f.HtmlHelper.Returns(helper);

            _g = Substitute.For<IFieldGenerator>();
            _g.GetLabelHtml().Returns(_label);
            _g.GetFieldHtml().Returns(_field);
            _g.GetValidationHtml().Returns(_validation);
        }

        private Field<TestFieldViewModel, IFormTemplate> Arrange(bool isParent)
        {
            return new Field<TestFieldViewModel, IFormTemplate>(_f, isParent, _g);
        }
        #endregion

        [Test]
        public void Use_field_from_template_for_begin_html()
        {
            var s = Arrange(false);

            Assert.That(s.Begin(), Is.EqualTo(_html));
        }

        [Test]
        public void Use_empty_string_for_end_html()
        {
            var s = Arrange(false);

            Assert.That(s.End().ToHtmlString(), Is.Empty);
        }

        [Test]
        public void Use_field_begin_from_template_for_parent_begin_html()
        {
            var s = Arrange(true);

            Assert.That(s.Begin(), Is.EqualTo(_beginHtml));
        }

        [Test]
        public void Use_field_end_from_template_for_parent_end_html()
        {
            var s = Arrange(true);

            Assert.That(s.End(), Is.EqualTo(_endHtml));
        }

        [Test]
        public void Construct_field_via_extension_method()
        {
            var s = new Section<TestFieldViewModel, IFormTemplate>(_f, "", false);
            _f.ClearReceivedCalls();

            var f = s.FieldFor(m => m.SomeProperty);

            Assert.That(f, Is.Not.Null);
            _f.DidNotReceive().Write(Arg.Any<IHtmlString>());
        }

        [Test]
        public void Construct_nested_field_via_extension_method()
        {
            var s = new Field<TestFieldViewModel, IFormTemplate>(_f, false, _g);
            _f.ClearReceivedCalls();

            var f = s.FieldFor(m => m.SomeProperty);

            Assert.That(f, Is.Not.Null);
            _f.DidNotReceive().Write(Arg.Any<IHtmlString>());
        }


        [Test]
        public void Construct_parent_field_via_extension_method()
        {
            var h = new HtmlString("");
            var s = new Section<TestFieldViewModel, IFormTemplate>(_f, "", false);
            _f.Template.BeginField(Arg.Any<IHtmlString>(), Arg.Any<IHtmlString>(), Arg.Any<IHtmlString>()).Returns(h);
            _f.ClearReceivedCalls();

            var f = s.BeginFieldFor(m => m.SomeProperty);

            Assert.That(f, Is.Not.Null);
            _f.Received().Write(h);
        }
    }
}
