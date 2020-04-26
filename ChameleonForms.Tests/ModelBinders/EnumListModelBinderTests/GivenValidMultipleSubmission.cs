using System.Threading.Tasks;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders.EnumListModelBinderTests
{
    class GivenValidMultipleSubmission : ModelBinderTestBase<TestViewModel>
    {
        [Test]
        public async Task WhenBindingOptionalNullableEnumList_ThenBindListWithoutErrors()
        {
            var (state, model) = await BindAndValidateAsync(m => m.OptionalNullableEnumList, TestViewModel.ValidMultipleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.NullableListFromValidMultipleSubmission));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingRequiredNullableEnumList_ThenBindListWithoutErrors()
        {
            var (state, model) = await BindAndValidateAsync(m => m.RequiredNullableEnumList, TestViewModel.ValidMultipleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.NullableListFromValidMultipleSubmission));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingOptionalEnumList_ThenBindListWithoutErrors()
        {
            var (state, model) = await BindAndValidateAsync(m => m.OptionalEnumList, TestViewModel.ValidMultipleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.ListFromValidMultipleSubmission));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingRequiredEnumList_ThenBindListWithoutErrors()
        {
            var (state, model) = await BindAndValidateAsync(m => m.RequiredEnumList, TestViewModel.ValidMultipleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.ListFromValidMultipleSubmission));
            Assert.That(state.IsValid, Is.True);
        }
    }
}
