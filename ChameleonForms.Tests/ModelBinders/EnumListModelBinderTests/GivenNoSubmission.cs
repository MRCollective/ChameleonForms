using System.Threading.Tasks;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders.EnumListModelBinderTests
{
    class GivenNoSubmission : ModelBinderTestBase<TestViewModel>
    {
        [Test]
        public async Task WhenBindingOptionalNullableEnumList_ThenBindEmptyListWithNoErrors()
        {
            var (state, model) = await BindAsync(m => m.OptionalNullableEnumList);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingRequiredNullableEnumList_ThenBindEmptyListWithError()
        {
            var (state, model) = await BindAsync(m => m.RequiredNullableEnumList);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.RequiredNullableEnumList, "The Required nullable enum list field is required.");
        }

        [Test]
        public async Task WhenBindingOptionalEnumList_ThenBindEmptyListWithNoErrors()
        {
            var (state, model) = await BindAsync(m => m.OptionalEnumList);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingRequiredEnumList_ThenBindEmptyListWithError()
        {
            var (state, model) = await BindAsync(m => m.RequiredEnumList);

            Assert.That(model, Is.Empty);
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.RequiredEnumList, "The Required enum list field is required.");
        }
    }
}
