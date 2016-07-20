using ChameleonForms.Attributes;
using ChameleonForms.Tests.FieldGenerator;
using NUnit.Framework;

namespace ChameleonForms.Tests.Attributes
{
    class RequiredFlagsEnumAttributeTests
    {
        [Test]
        public void DefaultFlagsEnumValueShouldntBeValid()
        {
            var attr = new RequiredFlagsEnumAttribute();

            Assert.That(attr.IsValid(default(TestFlagsEnum)), Is.False);
        }

        [Test]
        public void NonDefaultFlagsEnumSingleValueShouldBeValid()
        {
            var attr = new RequiredFlagsEnumAttribute();

            Assert.That(attr.IsValid(TestFlagsEnum.Simplevalue), Is.True);
        }

        [Test]
        public void NonDefaultFlagsEnumMultipleValueShouldBeValid()
        {
            var attr = new RequiredFlagsEnumAttribute();

            Assert.That(attr.IsValid(TestFlagsEnum.Simplevalue | TestFlagsEnum.ValueWithDescriptionAttribute), Is.True);
        }

        [Test]
        public void NullShouldntBeValid()
        {
            var attr = new RequiredFlagsEnumAttribute();

            Assert.That(attr.IsValid(null), Is.False);
        }
    }
}
