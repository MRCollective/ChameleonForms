using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.Handlers
{
    class EnumListHandlerShould : FieldGeneratorHandlerTest<TestEnum>
    {
        protected override IFieldGeneratorHandler<TestFieldViewModel, TestEnum> GetHandler(IFieldGenerator<TestFieldViewModel, TestEnum> handler)
        {
            return new EnumListHandler<TestFieldViewModel, TestEnum>(handler);
        }

        [Test]
        public void Return_dropdown_for_display_type_if_default_display_type_configured()
        {
            SetDisplayConfiguration(FieldDisplayType.Default);

            var type = GetDisplayType();

            Assert.That(type, Is.EqualTo(FieldDisplayType.DropDown));
        }

        [Test]
        public void Return_dropdown_for_display_type_if_dropdown_display_type_configured()
        {
            SetDisplayConfiguration(FieldDisplayType.DropDown);

            var type = GetDisplayType();

            Assert.That(type, Is.EqualTo(FieldDisplayType.DropDown));
        }

        [Test]
        public void Return_list_for_display_type_if_list_display_type_configured()
        {
            SetDisplayConfiguration(FieldDisplayType.List);

            var type = GetDisplayType();

            Assert.That(type, Is.EqualTo(FieldDisplayType.List));
        }
    }
}
