using System.Threading.Tasks;
using NUnit.Framework;

namespace ChameleonForms.Tests.ModelBinders.EnumListModelBinderTests
{
    class GivenValidSingleSubmission : ModelBinderTestBase<TestViewModel>
    {
        [Test]
        public async Task WhenBindingOptionalNullableEnumList_ThenBindListWithoutErrors()
        {
            var (state, model) = await BindAsync(m => m.OptionalNullableEnumList, TestViewModel.ValidSingleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.NullableListFromValidSingleSubmission));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingRequiredNullableEnumList_ThenBindListWithoutErrors()
        {
            var (state, model) = await BindAsync(m => m.RequiredNullableEnumList, TestViewModel.ValidSingleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.NullableListFromValidSingleSubmission));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingOptionalEnumList_ThenBindListWithoutErrors()
        {
            var (state, model) = await BindAsync(m => m.OptionalEnumList, TestViewModel.ValidSingleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.ListFromValidSingleSubmission));
            Assert.That(state.IsValid, Is.True);
        }

        [Test]
        public async Task WhenBindingRequiredEnumList_ThenBindListWithoutErrors()
        {
            var (state, model) = await BindAsync(m => m.RequiredEnumList, TestViewModel.ValidSingleSubmission);

            Assert.That(model, Is.EquivalentTo(TestViewModel.ListFromValidSingleSubmission));
            Assert.That(state.IsValid, Is.True);
        }
    }
}
