using System;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.Handlers
{
    [TestFixture(typeof(DateTime))]
    [TestFixture(typeof(DateTime?))]
    class DateTimeHandlerShould<T> : FieldGeneratorHandlerTest<T>
    {
        protected override IFieldGeneratorHandler<TestFieldViewModel, T> GetHandler(IFieldGenerator<TestFieldViewModel, T> handler)
        {
            return new DateTimeHandler<TestFieldViewModel, T>(handler);
        }

        [Test]
        public void Return_single_text_input_for_display_type()
        {
            var type = GetDisplayType();

            Assert.That(type, Is.EqualTo(FieldDisplayType.SingleLineText));
        }
    }
}
