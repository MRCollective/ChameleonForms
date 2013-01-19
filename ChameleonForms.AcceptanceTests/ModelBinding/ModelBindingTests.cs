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

            var page = new HomePage()
                .GoToModelBindingExamplePage()
                .Submit(enteredViewModel);

            Assert.That(page.GetFormValues(), IsSame.ViewModelAs(enteredViewModel));
            Assert.That(page.HasValidationErrors(), Is.False, "There are validation errors on the page");
        }

        [Test]
        public void Function_correctly_with_checkbox_and_radio_lists()
        {
            var enteredViewModel = ObjectMother.ModelBindingViewModels.BasicValid;

            var page = new HomePage()
                .GoToModelBindingExamplePage2()
                .Submit(enteredViewModel);

            Assert.That(page.GetFormValues(), IsSame.ViewModelAs(enteredViewModel));
            // This next assertion currently fails due to some tricky interaction with the default validation in MVC
            Assert.That(page.HasValidationErrors(), Is.False, "There are validation errors on the page");
        }
    }
}
