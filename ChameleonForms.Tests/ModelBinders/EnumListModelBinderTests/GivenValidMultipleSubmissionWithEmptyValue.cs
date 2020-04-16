using System.Threading.Tasks;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders.EnumListModelBinderTests
{
    class GivenValidMultipleSubmissionWithEmptyValue : ModelBinderTestBase<TestViewModel>
    {
        [Test]
        public async Task WhenBindingOptionalNullableEnumList_ThenBindListWithoutEmptyItemAndWithoutErrors()
        {
            var (state, model) = await BindAsync(m => m.OptionalNullableEnumList, TestViewModel.ValidMultipleSubmissionWithEmptyValue);

            Assert.That(model, Is.EquivalentTo(TestViewModel.NullableListFromValidMultipleSubmission));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingRequiredNullableEnumList_ThenBindListWithoutEmptyItemAndWithoutErrors()
        {
            var (state, model) = await BindAsync(m => m.RequiredNullableEnumList, TestViewModel.ValidMultipleSubmissionWithEmptyValue);

            Assert.That(model, Is.EquivalentTo(TestViewModel.NullableListFromValidMultipleSubmission));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingOptionalEnumList_ThenBindListWithoutEmptyItemAndWithoutErrors()
        {
            var (state, model) = await BindAsync(m => m.OptionalEnumList, TestViewModel.ValidMultipleSubmissionWithEmptyValue);

            Assert.That(model, Is.EquivalentTo(TestViewModel.ListFromValidMultipleSubmission));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingRequiredEnumList_ThenBindListWithoutEmptyItemAndWithError()
        {
            var (state, model) = await BindAsync(m => m.RequiredEnumList, TestViewModel.ValidMultipleSubmissionWithEmptyValue);

            Assert.That(model, Is.EquivalentTo(TestViewModel.ListFromValidMultipleSubmission));
            Assert.That(state.IsValid, Is.False);
            AssertPropertyError(state, m => m.RequiredEnumList, "The value '' is invalid.");
        }
    }
}
