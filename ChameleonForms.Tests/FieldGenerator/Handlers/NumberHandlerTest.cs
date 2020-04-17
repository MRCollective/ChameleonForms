using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.Handlers
{
    class IntNumberHandlerShould : FieldGeneratorHandlerTest<int>
    {
        protected override IFieldGeneratorHandler<TestFieldViewModel, int> GetHandler(IFieldGenerator<TestFieldViewModel, int> handler)
        {
            return new NumberHandler<TestFieldViewModel, int>(handler);
        }

        [Test]
        public void Return_single_text_input_for_display_type()
        {
            var type = GetDisplayType();

            Assert.That(type, Is.EqualTo(FieldDisplayType.SingleLineText));
        }
    }

    class DecimalNumberHandlerShould : FieldGeneratorHandlerTest<decimal>
    {
        protected override IFieldGeneratorHandler<TestFieldViewModel, decimal> GetHandler(IFieldGenerator<TestFieldViewModel, decimal> handler)
        {
            return new NumberHandler<TestFieldViewModel, decimal>(handler);
        }

        [Test]
        public void Return_single_text_input_for_display_type()
        {
            var type = GetDisplayType();

            Assert.That(type, Is.EqualTo(FieldDisplayType.SingleLineText));
        }
    }
}
