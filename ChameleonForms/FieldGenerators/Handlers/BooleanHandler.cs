using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.Templates;

namespace ChameleonForms.FieldGenerators.Handlers
{
    /// <summary>
    /// Generates the HTML for the Field Element of boolean fields as either a single checkbox, a select list or a list of radio buttons.
    /// </summary>
    /// <typeparam name="TModel">The type of the model the form is being output for</typeparam>
    /// <typeparam name="T">The type of the property in the model that the specific field is being output for</typeparam>
    public class BooleanHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        /// <summary>
        /// Constructor for the Boolean Field Generator Handler.
        /// </summary>
        /// <param name="fieldGenerator">The field generator for the field</param>
        public BooleanHandler(IFieldGenerator<TModel, T> fieldGenerator)
            : base(fieldGenerator)
        {}

        public override bool CanHandle()
        {
            return GetUnderlyingType(FieldGenerator) == typeof(bool);
        }

        public override IHtmlString GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            if (GetDisplayType(fieldConfiguration) == FieldDisplayType.Checkbox)
                return GetSingleCheckboxHtml(fieldConfiguration);

            var selectList = GetBooleanSelectList(fieldConfiguration);
            return GetSelectListHtml(selectList, FieldGenerator, fieldConfiguration);
        }

        public override void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            // If a list is being displayed there is no element for the label to point to so drop it
            if (fieldConfiguration.DisplayType == FieldDisplayType.List)
                fieldConfiguration.WithoutLabel();
        }

        public override FieldDisplayType GetDisplayType(IReadonlyFieldConfiguration fieldConfiguration)
        {
            if (fieldConfiguration.DisplayType == FieldDisplayType.Default && FieldGenerator.Metadata.ModelType == typeof(bool))
                return FieldDisplayType.Checkbox;

            return fieldConfiguration.DisplayType == FieldDisplayType.List
                ? FieldDisplayType.List
                : FieldDisplayType.DropDown;
        }

        private bool? GetValue()
        {
            return FieldGenerator.GetValue() as bool?;
        }

        private IHtmlString GetSingleCheckboxHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            var attrs = new HtmlAttributes(fieldConfiguration.HtmlAttributes);
            AdjustHtmlForModelState(attrs, FieldGenerator);
            var fieldhtml = HtmlCreator.BuildSingleCheckbox(GetFieldName(FieldGenerator), GetValue() ?? false, attrs);
            var labelHtml = HtmlCreator.BuildLabel(
                GetFieldName(FieldGenerator),
                fieldConfiguration.InlineLabelText ?? FieldGenerator.GetFieldDisplayName().ToHtml(),
                null
            );

            return new HtmlString(string.Format("{0} {1}", fieldhtml, labelHtml));
        }

        private IEnumerable<SelectListItem> GetBooleanSelectList(IReadonlyFieldConfiguration fieldConfiguration)
        {
            var value = GetValue();
            yield return new SelectListItem { Value = "true", Text = fieldConfiguration.TrueString, Selected = value == true };
            yield return new SelectListItem { Value = "false", Text = fieldConfiguration.FalseString, Selected = value == false };
        }
    }
}
