using System;
using System.ComponentModel.DataAnnotations;
using ChameleonForms.Attributes;
using ChameleonForms.Example.Controllers;
using NUnit.Framework;

namespace ChameleonForms.Tests.Attributes
{
    [TestFixture]
    public class ExistsInAttributeShould
    {
        [TestCase("", "Test")]
        [TestCase(null, "Test")]
        [TestCase("Test", "")]
        [TestCase("Test", null)]
        [TestCase("", "")]
        [TestCase(null, null)]
        public void Throw_exception_if_existsin_was_initialised_with_null_name_or_value_properties(string valueProperty, string nameProperty)
        {
            const string listProperty = "List";
            var vm = new ViewModelExample();
            var validationContext = new ValidationContext(vm, null, null);
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            Assert.Throws<ArgumentException>(
                () => attribute.GetValidationResult(vm.ListId, validationContext),
                "ExistsIn: You must pass valid properties for Name and Value to ExistsIn"
            );
        }

        [Test]
        public void Throw_exception_if_list_to_validate_against_does_not_contain_specified_value_property()
        {
            const string valueProperty = "Id2";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ViewModelExample();
            var validationContext = new ValidationContext(vm, null, null);
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            Assert.Throws<ArgumentException>(
                () => attribute.GetValidationResult(vm.ListId, validationContext),
                string.Format("ExistsIn: No property Model.{0} exists for validation.", valueProperty)
            );
        }

        [Test]
        public void Throw_exception_if_list_to_validate_against_does_not_exist()
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List2";
            var vm = new ViewModelExample();
            var validationContext = new ValidationContext(vm, null, null);
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            Assert.Throws<ArgumentException>(
                () => attribute.GetValidationResult(vm.ListId, validationContext),
                string.Format("ExistsIn: No property Model.{0} exists for validation.", valueProperty)
            );
        }

        [Test]
        public void Throw_exception_if_property_to_validate_against_is_not_an_ienumerable()
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "Decimal";
            var vm = new ViewModelExample();
            var validationContext = new ValidationContext(vm, null, null);
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            Assert.Throws<ArgumentException>(
                () => attribute.GetValidationResult(vm.ListId, validationContext),
                string.Format("ExistsIn: No property Model.{0} exists for validation.", valueProperty)
            );
        }

        [Test]
        public void Throw_exception_if_list_to_validate_against_is_null()
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ViewModelExample
            {
                List = null
            };
            var validationContext = new ValidationContext(vm, null, null);
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            Assert.Throws<ArgumentException>(
                () => attribute.GetValidationResult(vm.ListId, validationContext),
                string.Format("ExistsIn: Model.{0} is null. Unable to make list for Model.{1}", valueProperty, validationContext.MemberName)
            );
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(3)]
        public void Fail_validation_if_property_value_not_in_list(int testValue)
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ViewModelExample { ListId = testValue };
            var validationContext = new ValidationContext(vm, null, null) { DisplayName = "List id" };
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var result = attribute.GetValidationResult(vm.ListId, validationContext);

            var expectedError = string.Format("The {{0}} field was {0}, but must be one of A, B", testValue);
            Assert.That(result.ErrorMessage, Is.EqualTo(string.Format(expectedError, validationContext.DisplayName)));
        }

        [Test]
        public void Successfully_validate_non_ienumerable_property()
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ViewModelExample { ListId = 1 };
            var validationContext = new ValidationContext(vm, null, null);
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var result = attribute.GetValidationResult(vm.ListId, validationContext);

            Assert.That(result, Is.Null, string.Format("Validation failed with message: {0}", attribute.ErrorMessage));
        }

        [TestCase(new[]{1})]
        [TestCase(new[]{2})]
        [TestCase(new[]{1,2})]
        public void Successfully_validate_ienumerable_property(int[] submittedValues)
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ModelBindingViewModel { RequiredListIds = submittedValues };
            var validationContext = new ValidationContext(vm, null, null);
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var result = attribute.GetValidationResult(vm.RequiredListIds, validationContext);

            Assert.That(result, Is.Null, string.Format("Validation failed with message: {0}", attribute.ErrorMessage));
        }
    }
}