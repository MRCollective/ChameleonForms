using ChameleonForms.AcceptanceTests.Helpers;
using ChameleonForms.AcceptanceTests.ModelBinding.Pages;
using NUnit.Framework;
using TestStack.Seleno.Configuration;

namespace ChameleonForms.AcceptanceTests.ModelBinding
{
    [TestFixture]
    class ChangingContextShould
    {
        [Test]
        public void Post_different_view_model_and_bind_on_postback()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.DifferentViewModel;

            var page = Host.Instance.NavigateToInitialPage<HomePage>()
                .GoToChangingContextPage2()
                .PostDifferentModel(enteredViewModel);

            Assert.That(page.ReadDifferentModel(), IsSame.ViewModelAs(enteredViewModel));
            Assert.That(page.HasValidationErrors(), Is.False, "There are validation errors on the page");
        }

        [Test]
        public void Post_child_view_model_and_bind_on_postback()
        {
            var enteredViewModel = ObjectMother.ChangingContextViewModels.ChildViewModel;

            var page = Host.Instance.NavigateToInitialPage<HomePage>()
                .GoToChangingContextPage2()
                .PostChildModel(enteredViewModel);

            Assert.That(page.ReadChildModel(), IsSame.ViewModelAs(enteredViewModel));
            Assert.That(page.HasValidationErrors(), Is.False, "There are validation errors on the page");
        }
    }
}
