using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.Handlers
{
    [TestFixture(typeof(bool))]
    [TestFixture(typeof(bool?))]
    class BooleanHandlerShould<T> : FieldGeneratorHandlerTest<T>
    {
        protected override IFieldGeneratorHandler<TestFieldViewModel, T> GetHandler(IFieldGenerator<TestFieldViewModel, T> handler)
        {
            return new BooleanHandler<TestFieldViewModel, T>(handler);
        }

        [Test]
        public void Return_checkbox_for_boolean_and_dropdown_for_nullable_boolean_with_default_display_configuration()
        {
            var displayType = GetDisplayType();

            Assert.That(displayType, typeof (T) == typeof (bool)
                ? Is.EqualTo(FieldDisplayType.Checkbox)
                : Is.EqualTo(FieldDisplayType.DropDown)
            );
        }

        [Test]
        public void Return_dropdown_for_boolean_with_dropdown_display_configuration()
        {
            SetDisplayConfiguration(FieldDisplayType.DropDown);

            var displayType = GetDisplayType();

            Assert.That(displayType, Is.EqualTo(FieldDisplayType.DropDown));
        }

        [Test]
        public void Return_list_for_boolean_with_list_display_configuration()
        {
            SetDisplayConfiguration(FieldDisplayType.List);

            var displayType = GetDisplayType();

            Assert.That(displayType, Is.EqualTo(FieldDisplayType.List));
        }
    }
}
