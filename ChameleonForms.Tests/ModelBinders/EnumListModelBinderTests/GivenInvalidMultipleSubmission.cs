using System.Threading.Tasks;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders.EnumListModelBinderTests
{
    class GivenInvalidMultipleSubmission : ModelBinderTestBase<TestViewModel>
    {
        [Test]
        public async Task WhenBindingOptionalNullableEnumList_ThenBindPartialListWithError()
        {
            var (state, model) = await BindAndValidateAsync(m => m.OptionalNullableEnumList, TestViewModel.InvalidMultipleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.PartialNullableListFromInvalidMultipleSubmission));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.OptionalNullableEnumList, "The value 'Invalid' is not valid.");
        }

        [Test]
        public async Task WhenBindingRequiredNullableEnumList_ThenBindPartialListWithError()
        {
            var (state, model) = await BindAndValidateAsync(m => m.RequiredNullableEnumList, TestViewModel.InvalidMultipleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.PartialNullableListFromInvalidMultipleSubmission));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.RequiredNullableEnumList, "The value 'Invalid' is not valid.");
        }

        [Test]
        public async Task WhenBindingOptionalEnumList_ThenBindPartialListWithError()
        {
            var (state, model) = await BindAndValidateAsync(m => m.OptionalEnumList, TestViewModel.InvalidMultipleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.PartialListFromInvalidMultipleSubmission));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.OptionalEnumList, "The value 'Invalid' is not valid.");
        }

        [Test]
        public async Task WhenBindingRequiredEnumList_ThenBindPartialListWithError()
        {
            var (state, model) = await BindAndValidateAsync(m => m.RequiredEnumList, TestViewModel.InvalidMultipleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.PartialListFromInvalidMultipleSubmission));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.RequiredEnumList, "The value 'Invalid' is not valid.");
        }
    }
}
