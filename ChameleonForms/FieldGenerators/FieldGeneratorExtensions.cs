using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChameleonForms.FieldGenerators
{
    public static class FieldGeneratorExtensions
    {
        public static readonly HashSet<Type> IntTypes = new HashSet<Type>(new Type[]
        {
            typeof(byte), typeof(sbyte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong)
        });

        public static readonly HashSet<Type> FloatingTypes = new HashSet<Type>(new Type[]
        {
            typeof(float), typeof(double), typeof(decimal)
        });

        public static readonly HashSet<Type> NumericTypes = IntTypes.Union(FloatingTypes).ToHashSet();

        /// <summary>
        /// Whether or not the field represented by the field generator allows the user to enter multiple values.
        /// </summary>
        /// <returns>Whether or not the user can enter multiple values</returns>
        public static bool HasMultipleValues<TModel, T>(this IFieldGenerator<TModel, T> fieldGenerator)
        {
            return fieldGenerator.HasMultipleEnumValues() || fieldGenerator.HasEnumerableValues();
        }

        /// <summary>
        /// Whether or not the field represented by the field generator is an enum that can represent multiple values.
        /// i.e. whether or not the field is a flags enum.
        /// </summary>
        /// <returns>Whether or not the field is a flags enum</returns>
        public static bool HasMultipleEnumValues<TModel, T>(this IFieldGenerator<TModel, T> fieldGenerator)
        {
            return !fieldGenerator.HasEnumerableValues()
                && fieldGenerator.GetUnderlyingType().IsEnum
                && fieldGenerator.GetUnderlyingType().GetCustomAttributes(typeof(FlagsAttribute), false).Any();
        }

        /// <summary>
        /// Whether or not the field represented by the field generator is an enumerable list that allows multiple values.
        /// i.e. whether or not the field is an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <returns>Whether or not the field is an <see cref="IEnumerable{T}"/></returns>
        public static bool HasEnumerableValues<TModel, T>(this IFieldGenerator<TModel, T> fieldGenerator)
        {
            return typeof(IEnumerable).IsAssignableFrom(fieldGenerator.Metadata.ModelType)
                && fieldGenerator.Metadata.ModelType.IsGenericType;
        }

        /// <summary>
        /// Returns the enumerated values of a field that is an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>The enumerated values of the field</returns>
        public static IEnumerable<object> GetEnumerableValues<TModel, T>(this IFieldGenerator<TModel, T> fieldGenerator)
        {
            return (((IEnumerable)fieldGenerator.GetValue()) ?? new object[] { }).Cast<object>();
        }

        /// <summary>
        /// Whether or not the given value is present for the field represented by the field generator.
        /// </summary>
        /// <param name="value">The value to check is selected</param>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>Whether or not the value is selected</returns>
        public static bool IsSelected<TModel, T>(this IFieldGenerator<TModel, T> fieldGenerator, object value)
        {
            if (HasEnumerableValues(fieldGenerator))
                return GetEnumerableValues(fieldGenerator).Contains(value);

            var val = fieldGenerator.GetValue();
            if (val == null)
                return value == null;

            if (HasMultipleEnumValues(fieldGenerator))
                return (Convert.ToInt64(fieldGenerator.GetValue()) & Convert.ToInt64(value)) != 0;

            return val.Equals(value);
        }

        /// <summary>
        /// Returns the underlying type of the field - unwrapping <see cref="Nullable{T}"/> and <see cref="IEnumerable{T}"/> and IEnumerable&lt;Nullable&lt;T&gt;&gt;.
        /// </summary>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>The underlying type of the field</returns>
        public static Type GetUnderlyingType<TModel, T>(this IFieldGenerator<TModel, T> fieldGenerator)
        {
            var type = fieldGenerator.Metadata.ModelType;

            if (HasEnumerableValues(fieldGenerator))
                type = type.GetGenericArguments()[0];

            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// Whether or not the field involves collection of numeric values.
        /// </summary>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>Whether or not the field involves collection of numeric values</returns>
        public static bool IsNumeric<TModel, T>(this IFieldGenerator<TModel, T> fieldGenerator)
        {
            return NumericTypes.Contains(fieldGenerator.GetUnderlyingType());
        }

        /// <summary>
        /// Whether or not the field involves collection of integral number values.
        /// </summary>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>Whether or not the field involves collection of integral number values</returns>
        public static bool IsIntegralNumber<TModel, T>(this IFieldGenerator<TModel, T> fieldGenerator)
        {
            return IntTypes.Contains(fieldGenerator.GetUnderlyingType());
        }

        /// <summary>
        /// Whether or not the field involves collection of floating-point number values.
        /// </summary>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>Whether or not the field involves collection of floating-point number values</returns>
        public static bool IsFloatingNumber<TModel, T>(this IFieldGenerator<TModel, T> fieldGenerator)
        {
            return NumericTypes.Contains(fieldGenerator.GetUnderlyingType());
        }
    }
}
