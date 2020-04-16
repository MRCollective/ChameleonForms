using System.Threading.Tasks;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders.EnumListModelBinderTests
{
    class GivenInvalidSingleSubmission : ModelBinderTestBase<TestViewModel>
    {
        [Test]
        public async Task WhenBindingOptionalNullableEnumList_ThenBindEmptyListWithError()
        {
            var (state, model) = await BindAsync(m => m.OptionalNullableEnumList, TestViewModel.InvalidSingleSubmission);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.OptionalNullableEnumList, $"The value '{TestViewModel.InvalidSingleSubmission}' is not valid.");
        }

        [Test]
        public async Task WhenBindingRequiredNullableEnumList_ThenBindEmptyListWithError()
        {
            var (state, model) = await BindAsync(m => m.RequiredNullableEnumList, TestViewModel.InvalidSingleSubmission);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.RequiredNullableEnumList, $"The value '{TestViewModel.InvalidSingleSubmission}' is not valid.");
        }

        [Test]
        public async Task WhenBindingOptionalEnumList_ThenBindEmptyListWithError()
        {
            var (state, model) = await BindAsync(m => m.OptionalEnumList, TestViewModel.InvalidSingleSubmission);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.OptionalEnumList, $"The value '{TestViewModel.InvalidSingleSubmission}' is not valid.");
        }

        [Test]
        public async Task WhenBindingRequiredEnumList_ThenBindEmptyListWithError()
        {
            var (state, model) = await BindAsync(m => m.RequiredEnumList, TestViewModel.InvalidSingleSubmission);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.RequiredEnumList, $"The value '{TestViewModel.InvalidSingleSubmission}' is not valid.");
        }
    }
}
