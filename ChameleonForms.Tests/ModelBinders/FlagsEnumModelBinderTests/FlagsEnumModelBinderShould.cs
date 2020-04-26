using System.Threading.Tasks;
using ChameleonForms.Tests.FieldGenerator;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders.FlagsEnumModelBinderTests
{
    /// <remarks>
    /// These tests also test <see cref="Validators.RequiredFlagsEnumValidatorProvider" />.
    /// </remarks>
    [TestFixture(TypeArgs = new[]{typeof(TestFlagsEnum)})]
    [TestFixture(TypeArgs = new[]{typeof(TestFlagsEnum?)})]
    class FlagsEnumModelBinderShould<T> : ModelBinderTestBase<TestViewModel<T>>
    {
        [Test]
        public async Task Use_valid_default_value_when_there_is_empty_value([Values("", null)] string value)
        {
            var (state, model) = await BindAndValidateAsync(m => m.FlagEnum, value);
            
            Assert.That(model, Is.EqualTo(default(T)));
            Assert.That(state.IsValid, Is.EqualTo(typeof(T) == typeof(TestFlagsEnum?)));
        }

        [Test]
        public async Task Register_error_when_there_is_empty_value_on_field_with_required_attribute([Values("", null)] string value)
        {
            var (state, model) = await BindAndValidateAsync(m => m.RequiredFlagEnum, value);

            Assert.That(model, Is.EqualTo(default(T)));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.RequiredFlagEnum, "The RequiredFlagEnum field is required.");
        }

        [Test]
        public async Task Register_error_when_there_is_empty_value_on_implicitly_required_field([Values("", null)] string value)
        {
            var (state, model) = await BindAndValidateAsync(m => m.ImplicitlyRequiredFlagEnum, value);

            Assert.That(model, Is.EqualTo(default(TestFlagsEnum)));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.ImplicitlyRequiredFlagEnum, "The ImplicitlyRequiredFlagEnum field is required.");
        }

        [Test]
        public async Task Use_valid_default_value_when_there_is_no_value()
        {
            var (state, model) = await BindAndValidateAsync(m => m.FlagEnum);

            Assert.That(model, Is.EqualTo(default(T)));
            Assert.That(state.IsValid, Is.EqualTo(typeof(T) == typeof(TestFlagsEnum?)));
        }

        [Test]
        public async Task Use_default_value_and_add_error_when_there_is_an_invalid_value()
        {
            var (state, model) = await BindAndValidateAsync(m => m.FlagEnum, "invalid");

            Assert.That(model, Is.EqualTo(default(T)));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.FlagEnum, "The value 'invalid' is not valid for FlagEnum.");
        }

        [Test]
        public async Task Return_and_bind_valid_value_if_there_us_an_ok_single_value()
        {
            var (state, model) = await BindAndValidateAsync(m => m.FlagEnum, TestFlagsEnum.Simplevalue.ToString());

            Assert.That(model, Is.EqualTo(TestFlagsEnum.Simplevalue));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task Return_and_bind_valid_value_if_there_is_an_ok_multiple_value()
        {
            var (state, model) = await BindAndValidateAsync(m => m.FlagEnum, TestFlagsEnum.Simplevalue.ToString(), TestFlagsEnum.ValueWithDescriptionAttribute.ToString());

            Assert.That(model, Is.EqualTo(TestFlagsEnum.Simplevalue | TestFlagsEnum.ValueWithDescriptionAttribute));
            Assert.That(state.IsValid, Is.True);
        }
    }
}
