using System.Collections.Generic;
using System.Reflection;
using ChameleonForms.Example.Controllers;
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
            var enteredViewModel = GetViewModel();

            var submittedViewModel = new HomePage()
                .GoToModelBindingExamplePage()
                .Submit(enteredViewModel)
                .GetFormValues();

            foreach (var property in typeof(ModelBindingViewModel).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.IsReadonly())
                    continue;
                if (property.Name == "OptionalNullableEnums")
                    Assert.That(property.GetValue(submittedViewModel, null), Is.Null);
                else
                    Assert.That(property.GetValue(submittedViewModel, null), Is.EqualTo(property.GetValue(enteredViewModel, null)), property.Name);
            }
        }

        private ModelBindingViewModel GetViewModel()
        {
            return new ModelBindingViewModel
            {
                RequiredString = "RequiredString",
                RequiredInt = 1,
                OptionalInt = null,
                RequiredBool = true,
                RequiredNullableBool = false,
                OptionalBool = true,
                OptionalBool2 = false,
                OptionalBool3 = null,
                RequiredEnum = SomeEnum.SomeOtherValue,
                RequiredNullableEnum = SomeEnum.Value1,
                OptionalEnum = null,
                RequiredEnums = new List<SomeEnum> { SomeEnum.ValueWithDescription, SomeEnum.SomeOtherValue },
                RequiredNullableEnums = new List<SomeEnum?> { SomeEnum.Value1 },
                OptionalEnums = null,
                OptionalNullableEnums = new List<SomeEnum?>(),
                RequiredListId = 1
            };
        }
    }
}
