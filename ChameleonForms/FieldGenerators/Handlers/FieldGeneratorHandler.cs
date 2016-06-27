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

        /// <inheritdoc />
        protected static bool HasMultipleValues(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return fieldGenerator.Metadata.ModelType.IsGenericType
                && typeof(IEnumerable).IsAssignableFrom(fieldGenerator.Metadata.ModelType);
        }

        /// <inheritdoc />
        protected static IEnumerable<object> GetValues(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return (((IEnumerable)fieldGenerator.GetValue()) ?? new object[]{}).Cast<object>();
        }

        /// <inheritdoc />
        protected static bool IsSelected(object value, IFieldGenerator<TModel, T> fieldGenerator)
        {
            if (HasMultipleValues(fieldGenerator))
                return GetValues(fieldGenerator).Contains(value);

            var val = fieldGenerator.GetValue();
            if (val != null)
                return val.Equals(value);

            return value == null;
        }

        /// <inheritdoc />
        protected static Type GetUnderlyingType(IFieldGenerator<TModel, T> fieldGenerator)
        {
            var type = fieldGenerator.Metadata.ModelType;

            if (HasMultipleValues(fieldGenerator))
                type = type.GetGenericArguments()[0];

            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <inheritdoc />
        protected static bool IsNumeric(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return FieldGeneratorHandler.NumericTypes.Contains(GetUnderlyingType(fieldGenerator));
        }

        /// <inheritdoc />
        protected static IHtml GetInputHtml(TextInputType inputType, IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
        {
            var attrs = new HtmlAttributes(fieldConfiguration.HtmlAttributes);
            if (inputType == TextInputType.Password)
                attrs.Attr(type => "password");

            if (!attrs.Attributes.ContainsKey("type"))
                attrs.Attr(type => inputType.ToString().ToLower());
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

        /// <inheritdoc />
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
                    return fieldGenerator.View.SelectListFor(
                        fieldGenerator.FieldProperty,
                        selectList,
                        HasMultipleValues(fieldGenerator),
                        fieldConfiguration.HtmlAttributes.ToHtmlAttributes());
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
                    attrs.Attr("checked", "checked");
                attrs.Id(id);
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

        /// <inheritdoc />
        protected static string GetFieldName(IFieldGenerator<TModel, T> fieldGenerator)
        {
            return fieldGenerator.View.GetFieldName(fieldGenerator.FieldProperty);
        }

        /// <inheritdoc />
        protected static void AdjustHtmlForModelState(HtmlAttributes attrs, IFieldGenerator<TModel, T> fieldGenerator)
        {
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
