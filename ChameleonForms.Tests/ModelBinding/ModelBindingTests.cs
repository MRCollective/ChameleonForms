using ChameleonForms.Tests.Helpers;
using ChameleonForms.Tests.ModelBinding.Pages;
using NUnit.Framework;
using TestStack.Seleno.Configuration;

namespace ChameleonForms.Tests.ModelBinding
{
    [TestFixture]
    [Ignore("Breaking the build at the moment due to Seleno problem")]
    class ModelBindingShould
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            SelenoApplicationRunner.Run("ChameleonForms.Example", 19456);
        }

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
