using System;
using System.Web.Mvc;

namespace ChameleonForms.Attributes
{
    /// <summary>
    /// Indicates that the attributed property value should exist within the list property referenced by the attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExistsInAttribute : Attribute, IMetadataAware
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
    }
}
