using System.Collections.Generic;
using System.Web;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.Templates;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        /// <inheritdoc />
        public override bool CanHandle()
        {
            return GetUnderlyingType(FieldGenerator) == typeof(bool)
                && !HasEnumerableValues(FieldGenerator);
        }

        /// <inheritdoc />
        public override IHtmlContent GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            if (GetDisplayType(fieldConfiguration) == FieldDisplayType.Checkbox)
                return GetSingleCheckboxHtml(fieldConfiguration);

            var selectList = GetBooleanSelectList(fieldConfiguration);
            return GetSelectListHtml(selectList, FieldGenerator, fieldConfiguration);
        }

        /// <inheritdoc />
        public override void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            // If a list is being displayed there is no element for the label to point to so drop it
            if (fieldConfiguration.DisplayType == FieldDisplayType.List)
                fieldConfiguration.WithoutLabelElement();
        }

        /// <inheritdoc />
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

        private IHtmlContent GetSingleCheckboxHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            var attrs = new HtmlAttributes(fieldConfiguration.HtmlAttributes);
            AdjustHtmlForModelState(attrs, FieldGenerator);
            var fieldhtml = HtmlCreator.BuildSingleCheckbox(GetFieldName(FieldGenerator), GetValue() ?? false, attrs);

            if (fieldConfiguration.HasInlineLabel)
            {
                if (fieldConfiguration.ShouldInlineLabelWrapElement)
                {
                    var inlineLabelText = fieldConfiguration.InlineLabelText;

                    var contentBuilder = new HtmlContentBuilder();
                    contentBuilder.AppendHtml(fieldhtml);
                    contentBuilder.Append(" ");
                    if(inlineLabelText != null)
                    {
                        contentBuilder.AppendHtml(inlineLabelText);
                    }
                    else
                    {
                        contentBuilder.Append(FieldGenerator.GetFieldDisplayName());
                    }
                    
                    return HtmlCreator.BuildLabel(null, contentBuilder, null);
                }
                else
                {
                    HtmlContentBuilder bld = new HtmlContentBuilder();
                    bld.AppendHtml(fieldhtml)
                        .Append(" ")
                        .AppendHtml(HtmlCreator.BuildLabel(
                        GetFieldName(FieldGenerator),
                        fieldConfiguration.InlineLabelText ?? FieldGenerator.GetFieldDisplayName().ToHtml(),
                        null
                        ));
                    return bld;
                }
            }
            else
            {
                return fieldhtml;
            }
        }

        private IEnumerable<SelectListItem> GetBooleanSelectList(IReadonlyFieldConfiguration fieldConfiguration)
        {
            var value = GetValue();
            yield return new SelectListItem { Value = "true", Text = fieldConfiguration.TrueString, Selected = value == true };
            yield return new SelectListItem { Value = "false", Text = fieldConfiguration.FalseString, Selected = value == false };
        }
    }
}
