using System.Threading.Tasks;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders.EnumListModelBinderTests
{
    class GivenEmptySubmission : ModelBinderTestBase<TestViewModel>
    {
        [Test]
        public async Task WhenBindingOptionalNullableEnumList_ThenBindEmptyListWithNoErrors()
        {
            var (state, model) = await BindAsync(m => m.OptionalNullableEnumList, TestViewModel.EmptySubmission);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingRequiredNullableEnumList_ThenBindEmptyListWithError()
        {
            var (state, model) = await BindAsync(m => m.RequiredNullableEnumList, TestViewModel.EmptySubmission);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.RequiredNullableEnumList, "The RequiredNullableEnumList field is required.");
        }

        [Test]
        public async Task WhenBindingOptionalEnumList_ThenBindEmptyListWithNoErrors()
        {
            var (state, model) = await BindAsync(m => m.OptionalEnumList, TestViewModel.EmptySubmission);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingRequiredEnumList_ThenBindEmptyListWithError()
        {
            var (state, model) = await BindAsync(m => m.RequiredEnumList, TestViewModel.EmptySubmission);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.RequiredEnumList, "The value '' is invalid.");
        }
    }
}
