using System;
using System.Linq.Expressions;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.FieldGenerators;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component
{
    public class TestFieldViewModel
    {
        public string SomeProperty { get; set; }
    }

    [TestFixture]
    public class FieldShould
    {
        private const string FieldId = "FieldId";
        private readonly IHtml _beginHtml = new Html("b");
        private readonly IHtml _endHtml = new Html("e");
        private readonly IHtml _html = new Html("h");
        private readonly IHtml _label = new Html("l");
        private readonly IHtml _field = new Html("f");
        private readonly IHtml _validation = new Html("v");
        private IFieldMetadata _metadata;
        private IViewWithModel<TestFieldViewModel> _view;
        private IForm<TestFieldViewModel> _f;
        private IFieldGenerator _g;
        private IFieldConfiguration _fc;

        [SetUp]
        public void Setup()
        {
            _metadata = Substitute.For<IFieldMetadata>();
            _view = Substitute.For<IViewWithModel<TestFieldViewModel>>();

            _fc = Substitute.For<IFieldConfiguration>();

            _f = Substitute.For<IForm<TestFieldViewModel>>();
            _f.Template.BeginField(_label, _field, _validation, _metadata, Arg.Any<IReadonlyFieldConfiguration>(), Arg.Any<bool>()).Returns(_beginHtml);
            _f.Template.Field(_label, _field, _validation, _metadata, Arg.Any<IReadonlyFieldConfiguration>(), Arg.Any<bool>()).Returns(_html);
            _f.Template.EndField().Returns(_endHtml);

            _g = Substitute.For<IFieldGenerator>();
            _g.GetLabelHtml(Arg.Any<IReadonlyFieldConfiguration>()).Returns(_label);
            _g.GetFieldHtml(Arg.Any<IReadonlyFieldConfiguration>()).Returns(_field);
            _g.GetValidationHtml(Arg.Any<IReadonlyFieldConfiguration>()).Returns(_validation);
            _g.Metadata.Returns(_metadata);
            _g.GetFieldId().Returns(FieldId);

            _f.View.Returns(_view);
            _f.GetFieldGenerator(Arg.Any<Expression<Func<TestFieldViewModel, string>>>()).Returns(_g);

        }

        private Field<TestFieldViewModel> Arrange(bool isParent)
        {
            return new Field<TestFieldViewModel>(_f, isParent, _g, _fc);
        }

        [Test]
        public void Passthrough_true_to_template_for_valid_child_field()
        {
            var f = Arrange(false);
            _metadata.IsValid.Returns(true);

            f.Begin();

            _f.Template.Received().Field(Arg.Any<IHtml>(), Arg.Any<IHtml>(), Arg.Any<IHtml>(), Arg.Any<IFieldMetadata>(), Arg.Any<IReadonlyFieldConfiguration>(),
                true
            );
        }

        [Test]
        public void Passthrough_false_to_template_for_invalid_child_field()
        {
            var f = Arrange(false);
            _metadata.IsValid.Returns(false);

            f.Begin();

            _f.Template.Received().Field(Arg.Any<IHtml>(), Arg.Any<IHtml>(), Arg.Any<IHtml>(), Arg.Any<IFieldMetadata>(), Arg.Any<IReadonlyFieldConfiguration>(),
                false
            );
        }

        [Test]
        public void Passthrough_true_to_template_for_valid_parent_field()
        {
            var f = Arrange(true);
            _metadata.IsValid.Returns(true);

            f.Begin();

            _f.Template.Received().BeginField(Arg.Any<IHtml>(), Arg.Any<IHtml>(), Arg.Any<IHtml>(), Arg.Any<IFieldMetadata>(), Arg.Any<IReadonlyFieldConfiguration>(),
                true
            );
        }

        [Test]
        public void Passthrough_false_to_template_for_valid_parent_field()
        {
            var f = Arrange(true);
            _metadata.IsValid.Returns(false);

            f.Begin();

            _f.Template.Received().BeginField(Arg.Any<IHtml>(), Arg.Any<IHtml>(), Arg.Any<IHtml>(), Arg.Any<IFieldMetadata>(), Arg.Any<IReadonlyFieldConfiguration>(),
                false
            );
        }

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
            var s = new Section<TestFieldViewModel>(_f, new Html(""), false);
            _f.ClearReceivedCalls();

            var f = s.FieldFor(m => m.SomeProperty);

            Assert.That(f, Is.Not.Null);
            _f.DidNotReceive().Write(Arg.Any<IHtml>());
        }

        [Test]
        public void Construct_nested_field_via_extension_method()
        {
            var s = new Field<TestFieldViewModel>(_f, false, _g, null);
            _f.ClearReceivedCalls();

            var f = s.FieldFor(m => m.SomeProperty);

            Assert.That(f, Is.Not.Null);
            _f.DidNotReceive().Write(Arg.Any<IHtml>());
        }


        [Test]
        public void Construct_parent_field_via_extension_method()
        {
            var h = new Html("");
            var s = new Section<TestFieldViewModel>(_f, new Html(""), false);
            _f.Template.BeginField(Arg.Any<IHtml>(), Arg.Any<IHtml>(), Arg.Any<IHtml>(), Arg.Any<IFieldMetadata>(), Arg.Any<IReadonlyFieldConfiguration>(), Arg.Any<bool>()).Returns(h);
            _f.ClearReceivedCalls();

            var f = s.BeginFieldFor(m => m.SomeProperty);

            Assert.That(f, Is.Not.Null);
            _f.Received().Write(h);
        }

        [Test]
        public void Pass_field_into_field_configuration()
        {
            _fc = new FieldConfiguration();
            var field = Arrange(false);

            Assert.That(_fc.ToHtmlString(), Is.EqualTo(field.ToHtmlString()));
        }

        [Test]
        public void Pass_lazy_initialised_field_into_field_configuration()
        {
            _fc = new FieldConfiguration();
            var field = Arrange(false);
            _fc.SetField(() => field);

            Assert.That(_fc.ToHtmlString(), Is.EqualTo(field.ToHtmlString()));
        }

        [Test]
        public void Construct_field_from_form()
        {
            var fieldHtml = _f.FieldElementFor(m => m.SomeProperty);
            
            _g.GetFieldHtml(fieldHtml).Returns(_field);
            Assert.That(fieldHtml.ToHtmlString(), Is.EqualTo(_field.ToHtmlString()));
        }

        [Test]
        public void Construct_label_from_form()
        {
            var fieldHtml = _f.LabelFor(m => m.SomeProperty);

            _g.GetLabelHtml(fieldHtml).Returns(_label);
            Assert.That(fieldHtml.ToHtmlString(), Is.EqualTo(_label.ToHtmlString()));
        }

        [Test]
        public void Construct_validation_from_form()
        {
            var fieldHtml = _f.ValidationMessageFor(m => m.SomeProperty);

            _g.GetValidationHtml(fieldHtml).Returns(_validation);
            Assert.That(fieldHtml.ToHtmlString(), Is.EqualTo(_validation.ToHtmlString()));
        }
    }
}
