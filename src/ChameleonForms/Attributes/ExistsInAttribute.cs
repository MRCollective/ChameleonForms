using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using ChameleonForms.FieldGenerators.Handlers;
#if NETSTANDARD
using System.Reflection;
#endif

namespace ChameleonForms.Attributes
{
    /// <summary>
    /// Indicates that the attributed property value should exist within the list property referenced by the attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExistsInAttribute : ValidationAttribute
    {
        /// <summary>
        /// Returns whether or not the attribute applies to the given property
        /// </summary>
        /// <typeparam name="TParent">Type of the parent class of the property</typeparam>
        /// <typeparam name="TProperty">Type of the property itself</typeparam>
        /// <param name="property">The property to check</param>
        /// <returns>Whether or not the attribute applies</returns>
        public static bool AppliesToProperty<TParent, TProperty>(Expression<Func<TParent, TProperty>> property)
        {
            return GetAttributeInstanceForProperty(property) != null;
        }

        /// <summary>
        /// Returns the ExistsInAttribute instance that applies to the given property (or null if it doesn't).
        /// </summary>
        /// <typeparam name="TParent">Type of the parent class of the property</typeparam>
        /// <typeparam name="TProperty">Type of the property itself</typeparam>
        /// <param name="property">The property to check</param>
        /// <returns>The attribute that applies</returns>
        public static ExistsInAttribute GetAttributeInstanceForProperty<TParent, TProperty>(Expression<Func<TParent, TProperty>> property)
        {
            return property.Body.NodeType == ExpressionType.MemberAccess
                ? ((MemberExpression)property.Body).Member.GetCustomAttributes(typeof(ExistsInAttribute), false).Cast<ExistsInAttribute>().FirstOrDefault()
                : null;
        }
        
        /// <summary>
        /// Application-wide configuration for whether or not to enable ExistsIn validation.
        /// </summary>
        public static bool EnableValidation = true;

        /// <summary>
        /// The name of the list property on the parent model to get the value from.
        /// </summary>
        public string ListProperty { get; private set; }

        /// <summary>
        /// The name of the name property of the list item type to get the value from.
        /// </summary>
        public string ValueProperty { get; private set; }

        /// <summary>
        /// The name of the name property of the list item type to get the description from.
        /// </summary>
        public string NameProperty { get; private set; }

        /// <summary>
        /// Whether or not server-side validation is enabled for this instance.
        /// </summary>
        public bool? EnableValidationForThisInstance;

        /// <summary>
        /// Instantiates an <see cref="ExistsInAttribute"/>.
        /// </summary>
        /// <param name="listProperty">The name of the property containing the list this property should reference.</param>
        /// <param name="valueProperty">The name of the property of the list items to use for the value</param>
        /// <param name="nameProperty">The name of the property of the list items to use for the name/label</param>
        public ExistsInAttribute(string listProperty, string valueProperty, string nameProperty)
        {
            ListProperty = listProperty;
            ValueProperty = valueProperty;
            NameProperty = nameProperty;
        }

        /// <summary>
        /// Instantiates an <see cref="ExistsInAttribute"/>.
        /// </summary>
        /// <param name="listProperty">The name of the property containing the list this property should reference.</param>
        /// <param name="valueProperty">The name of the property of the list items to use for the value</param>
        /// <param name="nameProperty">The name of the property of the list items to use for the name/label</param>
        /// <param name="enableValidation">Optional override for ExistsIn server-side validation configuration (if not specified, static configuration setting ExistsInAttribute.EnableValidation is used)</param>
        public ExistsInAttribute(string listProperty, string valueProperty, string nameProperty, bool enableValidation)
        {
            ListProperty = listProperty;
            ValueProperty = valueProperty;
            NameProperty = nameProperty;
            EnableValidationForThisInstance = enableValidation;
        }

        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var enableValidation = EnableValidationForThisInstance.HasValue
                ? EnableValidationForThisInstance.Value
                : EnableValidation;
            if (!enableValidation || value == null || value.ToString() == string.Empty)
            {
                return ValidationResult.Success;
            }
            ValidateListConfiguration(context.ObjectInstance, ListProperty, ValueProperty, NameProperty, context.MemberName);

            var collectionProperty = context.ObjectInstance.GetType().GetProperty(ListProperty)
                .GetValue(context.ObjectInstance, null) as IEnumerable;
            var collection = collectionProperty.Cast<object>().ToList();
            var possibleValues = collection.Select(item => item.GetType().GetProperty(ValueProperty).GetValue(item, null))
                .Select(i => i is Enum ? (int)i : i);
            if (value is IEnumerable && !(value is string))
            {
                if ((value as IEnumerable).Cast<object>().All(v => v == null || possibleValues.Contains(v)))
                {
                    return ValidationResult.Success;
                }
            }
            else if (possibleValues.Any(item => item == null || item.ToString() == value.ToString()))
            {
                return ValidationResult.Success;
            }

            var attempted = value is IEnumerable
                ? string.Join(", ", (value as IEnumerable).Cast<object>().Select(t => t == null ? "null" : t.ToString()))
                : value.ToString();
            var choices = string.Join(", ", collection.Select(o => o.GetType().GetProperty(NameProperty).GetValue(o, null)));
            ErrorMessage = string.Format("The {0} field was {1}, but must be one of {2}", "{0}", attempted, choices);
            return new ValidationResult(FormatErrorMessage(context.DisplayName ?? context.MemberName), new List<string> { ListProperty });
        }

        /// <summary>
        /// Given a model, ensures the ExistsIn attribute has a valid configuration for generating and validating a list.
        /// </summary>
        /// <param name="model">The model being validated</param>
        /// <param name="listProperty">The name of the property containing the list this property should reference.</param>
        /// <param name="valueProperty">The name of the property of the list items to use for the value</param>
        /// <param name="nameProperty">The name of the property of the list items to use for the name/label</param>
        /// <param name="memberName">The name of the property that the ExistsIn attribute is applied do</param>
        public static void ValidateListConfiguration(object model, string listProperty, string valueProperty, string nameProperty, string memberName)
        {
            if (model == null)
            {
                throw new ModelNullException(memberName);
            }

            if (string.IsNullOrEmpty(nameProperty) || string.IsNullOrEmpty(valueProperty))
            {
                throw new ArgumentException("ExistsIn: You must pass valid properties for Name and Value.");
            }
            var collectionProperty = model.GetType().GetProperty(listProperty);
            if (collectionProperty == null)
            {
                throw new ArgumentException(string.Format("ExistsIn: No property Model.{0} exists for looking up values for property Model.{1}.", listProperty, memberName));
            }
            var collectionPropertyType = collectionProperty.PropertyType;
            var collectionType = collectionPropertyType.IsArray ? collectionPropertyType.GetElementType() : collectionPropertyType.GetGenericArguments().FirstOrDefault();
            if (collectionType != null && collectionType.GetProperty(valueProperty) == null)
            {
                throw new ArgumentException(string.Format("ExistsIn: No property {0} exists for type {1} to look up possible values for property Model.{2}.", valueProperty, collectionType.Name, memberName));
            }
            if (collectionType != null && collectionType.GetProperty(nameProperty) == null)
            {
                throw new ArgumentException(string.Format("ExistsIn: No property {0} exists for type {1} to look up possible values for property Model.{2}.", nameProperty, collectionType.Name, memberName));
            }
            var collectionValue = collectionProperty.GetValue(model, null);
            if (collectionValue == null)
            {
                throw new ListPropertyNullException(listProperty, memberName);
            }
            var collection = collectionValue as IEnumerable;
            if (collection == null)
            {
                throw new ArgumentException(string.Format("ExistsIn: Model.{0} is not an IEnumerable. ExistsIn cannot be used to validate against this property.", listProperty));
            }
        }
    }
}
