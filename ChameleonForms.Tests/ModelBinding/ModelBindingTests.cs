using ChameleonForms.Tests.Helpers;
using ChameleonForms.Tests.ModelBinding.Pages;
using NUnit.Framework;
using TestStack.Seleno.Configuration;

namespace ChameleonForms.Tests.ModelBinding
{
    [TestFixture]
    class ModelBindingShould
    {
        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            SelenoApplicationRunner.Run("ChameleonForms.Example", 19456);
        }

        [Test]
        public void Function_correctly()
        {
            var enteredViewModel = ObjectMother.ModelBindingViewModels.BasicValid;

            var submittedViewModel = new HomePage()
                .GoToModelBindingExamplePage()
                .Submit(enteredViewModel)
                .GetFormValues();

            Assert.That(submittedViewModel, IsSame.ViewModelAs(enteredViewModel));
        }
    }
}
