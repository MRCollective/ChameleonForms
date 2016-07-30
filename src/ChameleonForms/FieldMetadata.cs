using System;
using System.Collections.Generic;

namespace ChameleonForms
{
    /// <summary>
    /// Represents metdata about an instance of a form field.
    /// </summary>
    public interface IFieldMetadata
    {
        /// <summary>
        /// Whether or not the field input is valid.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Additional metadata values represented as name=>value pairs.
        /// </summary>
        Dictionary<string, object> AdditionalValues { get; }

        /// <summary>
        /// Whether or not entry of the field data is required.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        /// Data type name.
        /// </summary>
        string DataTypeName { get; }

        /// <summary>
        /// Format string to use when displaying.
        /// </summary>
        string DisplayFormatString { get; }

        /// <summary>
        /// Format string to use when editing.
        /// </summary>
        string EditFormatString { get; }

        /// <summary>
        /// Text to show when displaying this value as null.
        /// </summary>
        string NullDisplayText { get; }

        /// <summary>
        /// Whether or not the field is readonly.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// The type of data expected for the field.
        /// </summary>
        Type ModelType { get; }

        /// <summary>
        /// The name to display to an end user to describe the field.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The name of the property on the parent model that represents this field.
        /// </summary>
        string PropertyName { get; }
    }
}
