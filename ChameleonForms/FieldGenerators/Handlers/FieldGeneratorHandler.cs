using System;
using System.Collections;
using System.Collections.Generic;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.Templates;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace ChameleonForms.FieldGenerators.Handlers
{
    /// <summary>
    /// A Field Generator Handler is responsible for generating the HTML for a Field Element of a particular type of field.
    /// </summary>
    /// <typeparam name="TModel">The type of the model the form is being output for</typeparam>
    /// <typeparam name="T">The type of the property in the model that the specific field is being output for</typeparam>
    // ReSharper disable UnusedTypeParameter
    public interface IFieldGeneratorHandler<TModel, T>
    // ReSharper enable UnusedTypeParameter
    {
        /// <summary>
        /// Whether or not the current field can be output using this field generator handler.
        /// </summary>
        bool CanHandle();

        /// <summary>
        /// Generate the HTML for the current field's Field Element using this handler.
        /// </summary>
        /// <param name="fieldConfiguration">The field configuration to use to generate the HTML</param>
        /// <returns>The HTML for the Field Element</returns>
        IHtml GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration);

        /// <summary>
        /// Modify the field configuration for the field using this field generator handler.
        /// </summary>
        /// <param name="fieldConfiguration">The field configuration to modify</param>
        void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration);

        /// <summary>
        /// The type of control the field will be displayed as.
        /// </summary>
        /// <param name="fieldConfiguration">The configuration for the field</param>
        /// <returns>The display type of the field control</returns>
        FieldDisplayType GetDisplayType(IReadonlyFieldConfiguration fieldConfiguration);
    }
    // ReSharper enable UnusedTypeParameter

    internal static class FieldGeneratorHandler
    {
        public static readonly List<Type> NumericTypes = new List<Type>
        {
            typeof (byte),
            typeof (sbyte),
            typeof (short),
            typeof (ushort),
            typeof (int),
            typeof (uint),
            typeof (long),
            typeof (ulong),
            typeof (float),
            typeof (double),
            typeof (decimal)
        };
    }

    /// <summary>
    /// Base class that contains common logic for implementing field generator handlers.
    /// </summary>
    /// <typeparam name="TModel">The type of the model the form is being output for</typeparam>
    /// <typeparam name="T">The type of the property in the model that the specific field is being output for</typeparam>
    public abstract class FieldGeneratorHandler<TModel, T> : IFieldGeneratorHandler<TModel, T>
    {
        /// <summary>
        /// Create a field generator handler.
        /// </summary>
        /// <param name="fieldGenerator">The field generator to use</param>
        protected FieldGeneratorHandler(IFieldGenerator<TModel, T> fieldGenerator)
        {
            FieldGenerator = fieldGenerator;
        }

        /// <inheritdoc />
        protected readonly IFieldGenerator<TModel, T> FieldGenerator;
        /// <inheritdoc />
        public abstract bool CanHandle();
        /// <inheritdoc />
        public abstract IHtml GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration);
        /// <inheritdoc />
        public virtual void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration) {}
        /// <inheritdoc />
        public abstract FieldDisplayType GetDisplayType(IReadonlyFieldConfiguration fieldConfiguration);

        /// <summary>
        /// Whether or not the field represented by the field generator allows the user to enter multiple values.
        /// </summary>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>Whether or not the user can enter multiple values</returns>
        protected static bool HasMultipleValues(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return HasMultipleEnumValues(fieldGenerator) || HasEnumerableValues(fieldGenerator);
        }

        /// <summary>
        /// Whether or not the field represented by the field generator is an enum that can represent multiple values.
        /// i.e. whether or not the field is a flags enum.
        /// </summary>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>Whether or not the field is a flags enum</returns>
        protected static bool HasMultipleEnumValues(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return !HasEnumerableValues(fieldGenerator)
                && GetUnderlyingType(fieldGenerator).IsEnum
                && GetUnderlyingType(fieldGenerator).GetCustomAttributes(typeof(FlagsAttribute), false).Any();
        }

        /// <summary>
        /// Whether or not the field represented by the field generator is an enumerable list that allows multiple values.
        /// i.e. whether or not the field is an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>Whether or not the field is an <see cref="IEnumerable{T}"/></returns>
        protected static bool HasEnumerableValues(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return typeof(IEnumerable).IsAssignableFrom(fieldGenerator.Metadata.ModelType)
                && fieldGenerator.Metadata.ModelType.IsGenericType;
        }

        /// <summary>
        /// Returns the enumerated values of a field that is an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>The enumerated values of the field</returns>
        protected static IEnumerable<object> GetEnumerableValues(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return (((IEnumerable)fieldGenerator.GetValue()) ?? new object[]{}).Cast<object>();
        }

        /// <summary>
        /// Whether or not the given value is present for the field represented by the field generator.
        /// </summary>
        /// <param name="value">The value to check is selected</param>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>Whether or not the value is selected</returns>
        protected static bool IsSelected(object value, IFieldGenerator<TModel, T> fieldGenerator)
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
        protected static Type GetUnderlyingType(IFieldGenerator<TModel, T> fieldGenerator)
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
        protected static bool IsNumeric(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return FieldGeneratorHandler.NumericTypes.Contains(GetUnderlyingType(fieldGenerator));
        }

        /// <summary>
        /// Returns HTML for an &lt;input&gt; HTML element.
        /// </summary>
        /// <param name="inputType">The type of input to produce</param>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <param name="fieldConfiguration">The field configuration to use for attributes and format string</param>
        /// <returns>The HTML of the input element</returns>
        protected static IHtml GetInputHtml(TextInputType inputType, IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
        {
            var attrs = new HtmlAttributes(fieldConfiguration.HtmlAttributes);
            if (inputType == TextInputType.Password)
                attrs.Attr(type => "password");

            if (!attrs.Attributes.ContainsKey("type"))
                attrs = attrs.Attr(type => inputType.ToString().ToLower());
            return fieldGenerator.View.InputFor(fieldGenerator.FieldProperty, attrs, fieldConfiguration.FormatString);
        }

        private static bool HasEmptySelectListItem(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
        {
            if (fieldConfiguration.EmptyItemHidden)
                return false;

            // If it's a checkbox list then it
            //  shouldn't since you can uncheck everything
            if (fieldConfiguration.DisplayType == FieldDisplayType.List && HasMultipleValues(fieldGenerator))
                return false;

            // If it's a radio list for a required field then it
            //  shouldn't since no value is not a valid value and
            //  an initial null value translates to none of the radio
            //  boxes being selected
            if (fieldConfiguration.DisplayType == FieldDisplayType.List)
                return !fieldGenerator.Metadata.IsRequired;

            // If it's a multi-select dropdown and required then
            //  there shouldn't be an empty item
            if (HasMultipleValues(fieldGenerator))
                return !fieldGenerator.Metadata.IsRequired;

            // Dropdown lists for nullable types should have an empty item
            if (!fieldGenerator.Metadata.ModelType.IsValueType
                    || Nullable.GetUnderlyingType(fieldGenerator.Metadata.ModelType) != null)
                return true;

            return false;
        }

        /// <summary>
        /// Returns the HTML of a &lt;select&gt; list element.
        /// Automatically adds an empty item where appropriate.
        /// </summary>
        /// <param name="selectList">The list of items to choose from in the select list</param>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <param name="fieldConfiguration">The field configuration to use for attributes and empty item configuration</param>
        /// <returns></returns>
        protected static IHtml GetSelectListHtml(IEnumerable<SelectListItem> selectList, IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
        {
            if (HasEmptySelectListItem(fieldGenerator, fieldConfiguration))
                selectList = new []{GetEmptySelectListItem(fieldGenerator, fieldConfiguration)}.Union(selectList);

            switch (fieldConfiguration.DisplayType)
            {
                case FieldDisplayType.List:
                    var list = SelectListToRadioList(selectList, fieldGenerator, fieldConfiguration);
                    return fieldGenerator.Template.RadioOrCheckboxList(list, isCheckbox: HasMultipleValues(fieldGenerator));
                case FieldDisplayType.DropDown:
                case FieldDisplayType.Default:
                    var htmlAttrs = fieldConfiguration.HtmlAttributes.ToHtmlAttributes();
                    if (HasMultipleEnumValues(fieldGenerator))
                        AdjustHtmlForModelState(htmlAttrs, fieldGenerator);
                    return fieldGenerator.View.SelectListFor(
                        fieldGenerator.FieldProperty,
                        selectList,
                        HasMultipleValues(fieldGenerator),
                        htmlAttrs);
            }

            return null;
        }

        private static string GetEmptySelectListItemText(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
        {
            if (!string.IsNullOrEmpty(fieldConfiguration.NoneString))
                return fieldConfiguration.NoneString;

            if (GetUnderlyingType(fieldGenerator) == typeof (bool) && !fieldGenerator.Metadata.IsRequired)
                return "Neither";

            if (fieldConfiguration.DisplayType == FieldDisplayType.List)
                return "None";

            if (HasMultipleValues(fieldGenerator))
                return "None";

            return string.Empty;
        }

        private static SelectListItem GetEmptySelectListItem(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
        {
            var selected = fieldGenerator.GetValue() == null;
            if (typeof (T) == typeof (string))
                selected = string.IsNullOrEmpty(fieldGenerator.GetValue() as string);
            return new SelectListItem
            {
                Selected = selected,
                Value = "",
                Text = GetEmptySelectListItemText(fieldGenerator, fieldConfiguration)
            };
        }

        private static IEnumerable<IHtml> SelectListToRadioList(IEnumerable<SelectListItem> selectList, IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
        {
            var count = 0;
            foreach (var item in selectList)
            {
                var id = string.Format("{0}_{1}", fieldGenerator.GetFieldId(), ++count);
                var attrs = new HtmlAttributes(fieldConfiguration.HtmlAttributes);
                if (item.Selected)
                    attrs = attrs.Attr("checked", "checked");
                attrs = attrs.Id(id);
                if (HasMultipleValues(fieldGenerator))
                    AdjustHtmlForModelState(attrs, fieldGenerator);
                var fieldHtml = HasMultipleValues(fieldGenerator)
                        ? HtmlCreator.BuildSingleCheckbox(GetFieldName(fieldGenerator), item.Selected, attrs, item.Value)
                        : fieldGenerator.View.RadioItemFor(fieldGenerator.FieldProperty, item.Value, attrs);
                if (fieldConfiguration.ShouldInlineLabelWrapElement)
                    yield return new Html(string.Format("<label>{0} {1}</label>", fieldHtml.ToHtmlString(), HttpUtility.HtmlEncode(item.Text)));
                else
                    yield return new Html(string.Format("{0} {1}", fieldHtml.ToHtmlString(), fieldGenerator.View.Label(id, item.Text, new HtmlAttributes().AddClass(fieldConfiguration.LabelClasses)).ToHtmlString()));
            }
        }

        /// <summary>
        /// The value to use for the name of a field (e.g. for the name attribute or looking up model state).
        /// </summary>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        /// <returns>The name of the field</returns>
        protected static string GetFieldName(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return fieldGenerator.View.GetFieldName(fieldGenerator.FieldProperty);
        }

        /// <summary>
        /// Adjust the HTML attributes of a field based on the state of the model for that field.
        /// e.g. add validation attributes and error attributes.
        /// </summary>
        /// <param name="attrs">The attributes to modify</param>
        /// <param name="fieldGenerator">The field generator wrapping the field</param>
        protected static void AdjustHtmlForModelState(HtmlAttributes attrs, IFieldGenerator<TModel, T> fieldGenerator)
        {
            // todo: this is specific to MVC we should probably move this method to be called within the MVC view stuff
            var fullName = GetFieldName(fieldGenerator);

            ModelState modelState;
            if (fieldGenerator.View.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                    fieldGenerator.View.AddValidationErrorAttributes(attrs, fieldGenerator.FieldProperty, modelState);
            }

            fieldGenerator.View.AddValidationAttributes(attrs, fieldGenerator.FieldProperty);
        }
    }
}
