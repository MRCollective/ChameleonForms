using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ChameleonForms
{
    public class ModelMetadataFieldMetadata : IFieldMetadata
    {
        private readonly ModelMetadata _metadata;
        private bool _isValid;

        public ModelMetadataFieldMetadata(ModelMetadata metadata, bool isFieldValid)
        {
            _metadata = metadata;
            _isValid = isFieldValid;
        }

        public bool IsValid
        {
            get { return _isValid; }
        }

        public Dictionary<string, object> AdditionalValues
        {
            get { return _metadata.AdditionalValues; }
        }

        public bool IsRequired
        {
            get { return _metadata.IsRequired; }
        }

        public string DataTypeName
        {
            get { return _metadata.DataTypeName; }
        }

        public string DisplayFormatString
        {
            get { return _metadata.DisplayFormatString; }
        }

        public string EditFormatString
        {
            get { return _metadata.EditFormatString; }
        }

        public string NullDisplayText
        {
            get { return _metadata.NullDisplayText; }
        }

        public bool IsReadOnly
        {
            get { return _metadata.IsReadOnly; }
        }

        public Type ModelType
        {
            get { return _metadata.ModelType; }
        }

        public string DisplayName
        {
            get { return _metadata.DisplayName; }
        }

        public string PropertyName
        {
            get { return _metadata.PropertyName; }
        }
    }
}