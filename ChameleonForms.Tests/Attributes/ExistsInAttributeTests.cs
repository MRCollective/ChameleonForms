using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ChameleonForms.Attributes;
using ChameleonForms.Example.Controllers;
using ChameleonForms.FieldGenerators.Handlers;
using NUnit.Framework;

namespace ChameleonForms.Tests.Attributes
{
    [TestFixture]
    public class ExistsInAttributeShould
    {
        [SetUp]
        public void Setup()
        {
            ExistsInAttribute.EnableValidation = true;
        }

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
            var validationContext = new ValidationContext(vm, null, null) { MemberName = listProperty };
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);
            var collectionType = vm.GetType().GetProperty(listProperty).PropertyType.GetGenericArguments().FirstOrDefault();

            var ex = Assert.Throws<ArgumentException>(() => attribute.GetValidationResult(vm.ListId, validationContext));
            Assert.That(ex.Message,
                Is.EqualTo(string.Format("ExistsIn: No property {0} exists for type {1} to look up possible values for property Model.{2}.", valueProperty, collectionType.Name, listProperty)
            ));
        }

        [Test]
        public void Throw_exception_if_list_to_validate_against_does_not_contain_specified_name_property()
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name2";
            const string listProperty = "List";
            var vm = new ViewModelExample();
            var validationContext = new ValidationContext(vm, null, null) { MemberName = listProperty };
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);
            var collectionType = vm.GetType().GetProperty(listProperty).PropertyType.GetGenericArguments().FirstOrDefault();

            var ex = Assert.Throws<ArgumentException>(() => attribute.GetValidationResult(vm.ListId, validationContext));
            Assert.That(ex.Message,
                Is.EqualTo(string.Format("ExistsIn: No property {0} exists for type {1} to look up possible values for property Model.{2}.", nameProperty, collectionType.Name, listProperty)
            ));
        }

        [Test]
        public void Throw_exception_if_list_to_validate_against_does_not_exist()
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List2";
            const string memberName = "MyListInput";
            var vm = new ViewModelExample();
            var validationContext = new ValidationContext(vm, null, null) { MemberName = memberName };
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var ex = Assert.Throws<ArgumentException>(() => attribute.GetValidationResult(vm.ListId, validationContext));
            Assert.That(ex.Message, Is.EqualTo(string.Format("ExistsIn: No property Model.{0} exists for looking up values for property Model.{1}.", listProperty, memberName)));
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
            const string memberName = "MyListInput";
            var vm = new ViewModelExample
            {
                List = null
            };

            var validationContext = new ValidationContext(vm, null, null) { MemberName = memberName };
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var ex = Assert.Throws<ListPropertyNullException>(() => attribute.GetValidationResult(vm.ListId, validationContext));
            Assert.That(ex.Message, Is.EqualTo(
                string.Format("The list property ({0}) specified in the [ExistsIn] on {1} is null.", listProperty, memberName)
            ));
        }

        private static readonly IList<int?[]> _nullableListValues = new List<int?[]>()
        {
            new int?[]{null, 0, 0},
            new int?[]{0, null, 0}
        };

        [TestCaseSource("_nullableListValues")]
        public void Allow_validation_against_lists_containing_null_values(int?[] submittedValues)
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ModelBindingViewModel { RequiredNullableListIds = submittedValues };
            var validationContext = new ValidationContext(vm, null, null) { DisplayName = "Required list ids" };
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var result = attribute.GetValidationResult(vm.RequiredNullableListIds, validationContext);

            var expectedError = string.Format("The {{0}} field was {0}, but must be one of A, B", string.Join(", ", submittedValues.Select(s => s == null ? "null" : s.ToString())));
            Assert.That(result.ErrorMessage, Is.EqualTo(string.Format(expectedError, validationContext.DisplayName)));
        }

        [TestCase(new []{-1})]
        [TestCase(new []{0})]
        [TestCase(new []{0,1})]
        [TestCase(new []{3})]
        public void Fail_validation_if_list_property_value_not_in_list(int[] submittedValues)
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ModelBindingViewModel { RequiredListIds = submittedValues };
            var validationContext = new ValidationContext(vm, null, null) { DisplayName = "Required list ids" };
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var result = attribute.GetValidationResult(vm.RequiredListIds, validationContext);

            var expectedError = string.Format("The {{0}} field was {0}, but must be one of A, B", string.Join(", ", submittedValues));
            Assert.That(result.ErrorMessage, Is.EqualTo(string.Format(expectedError, validationContext.DisplayName)));
        }
        
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(3)]
        public void Fail_validation_if_single_property_value_not_in_list(int submittedValue)
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ModelBindingViewModel { RequiredListId = submittedValue };
            var validationContext = new ValidationContext(vm, null, null) { DisplayName = "Required list ids" };
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var result = attribute.GetValidationResult(vm.RequiredListId, validationContext);

            var expectedError = string.Format("The {{0}} field was {0}, but must be one of A, B", string.Join(", ", submittedValue));
            Assert.That(result.ErrorMessage, Is.EqualTo(string.Format(expectedError, validationContext.DisplayName)));
        }

        [Test]
        public void Fail_to_validate_an_invalid_submission_if_validation_disabled_globally_and_enabled_locally()
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ModelBindingViewModel { RequiredListId = 3 };
            var validationContext = new ValidationContext(vm, null, null);
            ExistsInAttribute.EnableValidation = false;
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty, enableValidation: true);

            var result = attribute.GetValidationResult(vm.RequiredListId, validationContext);

            var expectedError = string.Format("The {{0}} field was 3, but must be one of A, B");
            Assert.That(result.ErrorMessage, Is.EqualTo(string.Format(expectedError, validationContext.DisplayName)));
        }

        [Test]
        public void Successfully_validate_an_invalid_submission_if_validation_disabled_locally()
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ModelBindingViewModel { RequiredListId = 3 };
            var validationContext = new ValidationContext(vm, null, null);
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty, enableValidation: false);

            var result = attribute.GetValidationResult(vm.RequiredListId, validationContext);

            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        [Test]
        public void Successfully_validate_an_invalid_submission_if_validation_disabled_globally_and_not_overidden()
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ModelBindingViewModel { RequiredListId = 3 };
            var validationContext = new ValidationContext(vm, null, null);
            ExistsInAttribute.EnableValidation = false;
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var result = attribute.GetValidationResult(vm.RequiredListId, validationContext);

            Assert.That(result, Is.EqualTo(ValidationResult.Success));
        }

        private static readonly ModelBindingViewModel[] _validViewModels =
        {
            new ModelBindingViewModel{ RequiredString = "" },
            new ModelBindingViewModel{ RequiredString = null },
            new ModelBindingViewModel{ RequiredString = "1" }
        };

        [TestCaseSource("_validViewModels")]
        public void Successfully_validate_string_property(ModelBindingViewModel vm)
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var validationContext = new ValidationContext(vm, null, null);
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var result = attribute.GetValidationResult(vm.RequiredString, validationContext);

            Assert.That(result, Is.Null, string.Format("Validation failed with message: {0}", attribute.ErrorMessage));
        }

        [TestCase(1)]
        [TestCase(null)]
        public void Successfully_validate_non_ienumerable_property(int? submittedValue)
        {
            const string valueProperty = "Id";
            const string nameProperty = "Name";
            const string listProperty = "List";
            var vm = new ModelBindingViewModel { OptionalInt = submittedValue };
            var validationContext = new ValidationContext(vm, null, null);
            var attribute = new ExistsInAttribute(listProperty, valueProperty, nameProperty);

            var result = attribute.GetValidationResult(vm.OptionalInt, validationContext);

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