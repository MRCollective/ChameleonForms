using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ChameleonForms.Attributes
{
    /// <summary>
    /// Indicates that the attributed property value should exist within the list property referenced by the attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExistsInAttribute : ValidationAttribute, IMetadataAware
    {
        /// <summary>
        /// Additional Values metadata key for whether this attribute has been applied to the property.
        /// </summary>
        public const string ExistsKey = "ExistsInList";
        /// <summary>
        /// Additional Values metadata key for the name of the list property.
        /// </summary>
        public const string PropertyKey = "ExistsInProperty";
        /// <summary>
        /// Additional Values metadata key for the name of the value property for the list items.
        /// </summary>
        public const string ValueKey = "ExistsInValueProperty";
        /// <summary>
        /// Additional Values metadata key for the name of the name/label property for the list items.
        /// </summary>
        public const string NameKey = "ExistsInNameProperty";

        private readonly string _listProperty;
        private readonly string _valueProperty;
        private readonly string _nameProperty;

        /// <summary>
        /// Instantiates an <see cref="ExistsInAttribute"/>.
        /// </summary>
        /// <param name="listProperty">The name of the property containing the list this property should reference.</param>
        /// <param name="valueProperty">The name of the property of the list items to use for the value</param>
        /// <param name="nameProperty">The name of the property of the list items to use for the name/label</param>
        public ExistsInAttribute(string listProperty, string valueProperty, string nameProperty)
        {
            _listProperty = listProperty;
            _valueProperty = valueProperty;
            _nameProperty = nameProperty;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[ExistsKey] = true;
            metadata.AdditionalValues[PropertyKey] = _listProperty;
            metadata.AdditionalValues[ValueKey] = _valueProperty;
            metadata.AdditionalValues[NameKey] = _nameProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            var collection = GetCollectionIfValid(context);
            var possibleValues = collection.Select(item => item.GetType().GetProperty(_valueProperty).GetValue(item, null).ToString());
            if (value is IEnumerable)
            {
                if ((value as IEnumerable).Cast<object>().All(v => possibleValues.Any(item => item == v.ToString())))
                {
                    return ValidationResult.Success;
                }
            }
            else if (possibleValues.Any(item => item == value.ToString()))
            {
                return ValidationResult.Success;
            }

            var attempted = value is IEnumerable
                ? string.Join(", ", (value as IEnumerable).Cast<object>().Select(t => t.ToString()))
                : value.ToString();
            var choices = string.Join(", ", collection.Select(o => o.GetType().GetProperty(_nameProperty).GetValue(o, null)));
            ErrorMessage = string.Format("The {0} field was {1}, but must be one of {2}", "{0}", attempted, choices);
            return new ValidationResult(FormatErrorMessage(context.DisplayName ?? context.MemberName), new List<string> { _listProperty });
        }

        private IList<object> GetCollectionIfValid(ValidationContext context)
        {
            if (string.IsNullOrEmpty(_nameProperty) || string.IsNullOrEmpty(_valueProperty))
            {
                throw new ArgumentException("You must pass valid properties for Name and Value to ExistsIn.");
            }
            var collectionProperty = context.ObjectInstance.GetType().GetProperty(_listProperty);
            if (collectionProperty == null)
            {
                throw new Exception(string.Format("No property Model.{0} exists for ExistsIn validation.", _listProperty));
            }
            var collectionValue = collectionProperty.GetValue(context.ObjectInstance, null);
            if (collectionValue == null)
            {
                throw new Exception(string.Format("Model.{0} is null. Unable to make list for Model.{1}", _listProperty, context.MemberName));
            }
            var collection = collectionValue as IEnumerable;
            if (collection == null)
            {
                throw new Exception(string.Format("Model.{0} is not an IEnumerable. ExistsIn cannot be used to validate against this property.", _listProperty));
            }

            return collection.Cast<object>().ToList();
        }
    }
}
