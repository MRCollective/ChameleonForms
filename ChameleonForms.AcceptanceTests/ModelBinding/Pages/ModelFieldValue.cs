using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages
{
    internal interface IModelFieldValue
    {
        bool HasMultipleValues { get; }
        IEnumerable<string> Values { get; }
        string Value { get; }
        bool IsTrue { get; }
    }

    internal class ModelFieldValue : IModelFieldValue
    {
        private readonly object _value;
        private readonly string _format;

        public ModelFieldValue(object value, string format)
        {
            _value = value;
            _format = format;
        }

        public bool HasMultipleValues { get { return _value as IEnumerable != null && _value.GetType() != typeof(string); } }

        public IEnumerable<string> Values
        {
            get
            {
                if (!HasMultipleValues)
                    throw new InvalidOperationException("Field does not have multiple values!");
                return (_value as IEnumerable).Cast<object>()
                    .Select(o => new ModelFieldValue(o, _format))
                    .Select(v => v.Value);
            }
        }

        public string Value
        {
            get
            {
                var val = string.Empty;
                if (HasMultipleValues)
                    val = string.Join(",", Values);
                else if (_value != null)
                    val = _value is bool ? _value.ToString().ToLower() : string.Format(_format, _value);
                return val;
            }
        }

        public bool IsTrue
        {
            get { return _value as bool? == true; }
        }
    }
}
