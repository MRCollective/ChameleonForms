using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.Handlers
{
    class TextAreaHandlerShould : FieldGeneratorHandlerTest<string>
    {
        protected override IFieldGeneratorHandler<TestFieldViewModel, string> GetHandler(IFieldGenerator<TestFieldViewModel, string> handler)
        {
            return new TextAreaHandler<TestFieldViewModel, string>(handler);
        }

        [Test]
        public void Return_multipleline_text_input_control_for_display_type()
        {
            var type = GetDisplayType();

            Assert.That(type, Is.EqualTo(FieldDisplayType.MultiLineText));
        }
    }
}
