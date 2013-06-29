using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages
{
    internal interface IModelFieldType
    {
        string Format { get; }
        object GetValueFromString(string stringValue);
        object GetValueFromStrings(IEnumerable<string> stringValues);
        object Cast(IEnumerable<object> values);
        object DefaultValue { get; }
        Type UnderlyingType { get; }
        bool HasMultipleValues { get; }
        bool IsBoolean { get; }
        Type BaseType { get; }
    }

    internal class ModelFieldType : IModelFieldType
    {
        private readonly Type _fieldType;
        public string Format { get; private set; }

        public ModelFieldType(Type fieldType, string format)
        {
            _fieldType = fieldType;
            Format = format.Replace("{0:", "").Replace("}", "");
        }

        public object GetValueFromString(string stringValue)
        {
            var value = GetUnderlyingValueFromString(stringValue);
            if (!HasMultipleValues)
                return value;

            return Cast(new[] {value});
        }

        public object GetValueFromStrings(IEnumerable<string> stringValues)
        {
            if (stringValues == null)
                return GetValueFromString(null);

            if (!HasMultipleValues)
                return GetUnderlyingValueFromString(string.Join(",", stringValues.Where(s => !string.IsNullOrEmpty(s))));

            return Cast(stringValues.Where(s => !string.IsNullOrEmpty(s)).Select(GetUnderlyingValueFromString));
        }

        public object Cast(IEnumerable<object> values)
        {
            var valuesList = values.ToList();

            if (!valuesList.Any())
                return null;

            var castMethod = typeof(Enumerable).GetMethod("Cast").MakeGenericMethod(BaseType);
            var toListMethod = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(BaseType);
            return toListMethod.Invoke(null, new[] { castMethod.Invoke(null, new[] { valuesList }) });
        }

        private object GetUnderlyingValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return DefaultValue;
            var underlyingType = UnderlyingType;

            if (underlyingType == typeof (DateTime) && !string.IsNullOrEmpty(Format))
                return DateTime.ParseExact(value, Format, new DateTimeFormatInfo(), DateTimeStyles.None);

            return underlyingType.IsEnum
                ? Enum.Parse(underlyingType, value)
                : Convert.ChangeType(value, underlyingType);
        }

        public object DefaultValue
        {
            get
            {
                return _fieldType.IsValueType ? Activator.CreateInstance(_fieldType) : null;
            }
        }

        public Type UnderlyingType
        {
            get
            {
                return Nullable.GetUnderlyingType(BaseType) ?? BaseType;
            }
        }

        public bool HasMultipleValues
        {
            get
            {
                return _fieldType.IsGenericType &&
                    typeof (IEnumerable<>).IsAssignableFrom(_fieldType.GetGenericTypeDefinition());
            }
        }

        public bool IsBoolean
        {
            get { return UnderlyingType == typeof (bool); }
        }

        public Type BaseType
        {
            get
            {
                return HasMultipleValues ? _fieldType.GetGenericArguments()[0] : _fieldType;
            }
        }
    }
}
