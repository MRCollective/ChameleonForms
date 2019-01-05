using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using ChameleonForms.Templates.TwitterBootstrap3;
using Microsoft.AspNetCore.Html;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates.TwitterBootstrap3
{
    public class PrepareFieldConfigurationTests_TwitterBootstrapTemplateShould
    {
        private readonly TwitterBootstrapFormTemplate _t = new TwitterBootstrapFormTemplate();
        private IFieldConfiguration _fieldConfiguration;
        public class TestViewModel { }

        [SetUp]
        public void Setup()
        {
            _fieldConfiguration = new FieldConfiguration();
        }

        private IReadonlyFieldConfiguration Act(FieldDisplayType displayType, FieldParent parent)
        {
            var fg = Substitute.For<IFieldGenerator<TestViewModel, string>>();
            var fgh = Substitute.For<IFieldGeneratorHandler<TestViewModel, string>>();
            fgh.GetDisplayType(Arg.Any<IReadonlyFieldConfiguration>()).Returns(displayType);
            _t.PrepareFieldConfiguration(fg, fgh, _fieldConfiguration, parent);
            return _fieldConfiguration;
        }

        [Test]
        public void Add_validation_class_of_help_block_when_in_section([Values(FieldParent.Form, FieldParent.Section)] FieldParent parent)
        {
            var config = Act(FieldDisplayType.SingleLineText, parent);

            if (parent == FieldParent.Section)
                Assert.That(config.ValidationClasses, Is.EqualTo("help-block"));
            else
                Assert.That(config.ValidationClasses, Is.Null);
        }

        [Test]
        [Combinatorial]
        public void Add_field_class_of_form_control_when_in_section_and_select_input_or_textarea([Values(FieldParent.Form, FieldParent.Section)] FieldParent parent, [Values(FieldDisplayType.SingleLineText, FieldDisplayType.MultiLineText, FieldDisplayType.DropDown)] FieldDisplayType displayType)
        {
            var config = Act(displayType, parent);

            if (parent == FieldParent.Section)
                Assert.That(config.HtmlAttributes["class"], Is.EqualTo("form-control"));
            else
                Assert.That(config.HtmlAttributes.ContainsKey("class"), Is.False);
        }

        [Test]
        [Combinatorial]
        public void Add_label_class_of_control_label_when_in_section_and_select_nonfileinput_or_textarea([Values(FieldParent.Form, FieldParent.Section)] FieldParent parent, [Values(FieldDisplayType.SingleLineText, FieldDisplayType.MultiLineText, FieldDisplayType.DropDown)] FieldDisplayType displayType)
        {
            var config = Act(displayType, parent);

            if (parent == FieldParent.Section)
                Assert.That(config.LabelClasses, Is.EqualTo("control-label"));
            else
                Assert.That(config.LabelClasses, Is.Null);
        }

        [Test]
        [Combinatorial]
        public void Dont_add_label_class_or_field_class_if_in_section_and_not_select_nonfileinput_or_textarea([Values(FieldDisplayType.Custom, FieldDisplayType.Checkbox, FieldDisplayType.List, FieldDisplayType.FileUpload, FieldDisplayType.Default)] FieldDisplayType displayType)
        {
            var config = Act(displayType, FieldParent.Section);

            Assert.That(config.HtmlAttributes.ContainsKey("class"), Is.False);
            Assert.That(config.LabelClasses, Is.Null);
        }

        [Test]
        [Combinatorial]
        public void Add_label_class_of_control_label_when_in_section_and_select_input_or_textarea([Values(FieldParent.Form, FieldParent.Section)] FieldParent parent, [Values(FieldDisplayType.SingleLineText, FieldDisplayType.MultiLineText, FieldDisplayType.DropDown)] FieldDisplayType displayType)
        {
            var config = Act(displayType, parent);

            if (parent == FieldParent.Section)
                Assert.That(config.LabelClasses, Is.EqualTo("control-label"));
            else
                Assert.That(config.LabelClasses, Is.Null);
        }

        [Test]
        public void Hide_label_and_set_as_checkbox_control_if_checkbox_and_in_section()
        {
            _fieldConfiguration.Label("label");

            var config = Act(FieldDisplayType.Checkbox, FieldParent.Section);

            Assert.That(config.GetBagData<bool>("IsCheckboxControl"), Is.True);
            Assert.That(config.LabelText.ToHtmlString(), Is.EqualTo(string.Empty));
            Assert.That(config.HasLabelElement, Is.False);
        }

        [Test]
        public void Leave_label_alone_and_dont_set_as_checkbox_control_if_checkbox_and_in_form()
        {
            _fieldConfiguration.Label("label");

            var config = Act(FieldDisplayType.Checkbox, FieldParent.Form);

            Assert.That(config.GetBagData<bool>("IsCheckboxControl"), Is.False);
            Assert.That(config.LabelText.ToHtmlString(), Is.EqualTo("label"));
            Assert.That(config.HasLabelElement, Is.True);
        }

        [Test]
        public void Set_control_as_radio_list_if_radio_list_and_in_section([Values(FieldParent.Form, FieldParent.Section)] FieldParent parent)
        {
            var config = Act(FieldDisplayType.List, parent);

            Assert.That(config.GetBagData<bool>("IsRadioOrCheckboxList"), Is.EqualTo(parent == FieldParent.Section));
        }
    }
}
