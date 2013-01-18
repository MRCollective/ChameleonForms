using ChameleonForms.AcceptanceTests.Helpers;
using ChameleonForms.AcceptanceTests.ModelBinding.Pages;
using NUnit.Framework;

namespace ChameleonForms.AcceptanceTests.ModelBinding
{
    [TestFixture]
    class ModelBindingShould
    {
        [Test]
        public void Function_correctly_with_default_form()
        {
            var enteredViewModel = ObjectMother.ModelBindingViewModels.BasicValid;

            var submittedViewModel = new HomePage()
                .GoToModelBindingExamplePage()
                .Submit(enteredViewModel)
                .GetFormValues();

            Assert.That(submittedViewModel, IsSame.ViewModelAs(enteredViewModel));
        }

        [Test]
        public void Function_correctly_with_checkbox_and_radio_lists()
        {
            var enteredViewModel = ObjectMother.ModelBindingViewModels.BasicValid;

            var submittedViewModel = new HomePage()
                .GoToModelBindingExamplePage2()
                .Submit(enteredViewModel)
                .GetFormValues();

            Assert.That(submittedViewModel, IsSame.ViewModelAs(enteredViewModel));
        }
    }
}
