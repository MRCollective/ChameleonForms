using System;
using System.Collections.Generic;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.Templates;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace ChameleonForms.FieldGenerators.Handlers
{
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

        /// <summary>
        /// The field generator for the current field.
        /// </summary>
        protected readonly IFieldGenerator<TModel, T> FieldGenerator;
        /// <inheritdoc />
        public abstract bool CanHandle();
        /// <inheritdoc />
        public abstract IHtmlContent GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration);
        /// <inheritdoc />
        public virtual void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration) {}
        /// <inheritdoc />
        public abstract FieldDisplayType GetDisplayType(IReadonlyFieldConfiguration fieldConfiguration);

        /// <summary>
        /// Returns HTML for an &lt;input&gt; HTML element.
        /// </summary>
        /// <param name="inputType">The type of input to produce</param>
        /// <param name="fieldConfiguration">The field configuration to use for attributes and format string</param>
        /// <returns>The HTML of the input element</returns>
        protected IHtmlContent GetInputHtml(TextInputType inputType, IReadonlyFieldConfiguration fieldConfiguration)
        {
            if (inputType == TextInputType.Password)
            {
                return FieldGenerator.HtmlHelper.PasswordFor(FieldGenerator.FieldProperty, fieldConfiguration.HtmlAttributes);
            }

            var attrs = new HtmlAttributes(fieldConfiguration.HtmlAttributes);
            if (!attrs.Attributes.ContainsKey("type"))
            {
                attrs = attrs.Attr(type => inputType.ToString().ToLower());
            }

            IHtmlContent htmlContent;
            if (!string.IsNullOrEmpty(fieldConfiguration.FormatString))
            {
                htmlContent = FieldGenerator.HtmlHelper.TextBoxFor(
                    FieldGenerator.FieldProperty,
                    fieldConfiguration.FormatString,
                    attrs.ToDictionary()
                );
            }
            else
            {
                htmlContent = FieldGenerator.HtmlHelper.TextBoxFor(
                    FieldGenerator.FieldProperty,
                    attrs.ToDictionary()
                );
            }

            return htmlContent;
        }

        private bool HasEmptySelectListItem(IReadonlyFieldConfiguration fieldConfiguration)
        {
            if (fieldConfiguration.EmptyItemHidden)
                return false;

            // If it's a checkbox list then it
            //  shouldn't since you can uncheck everything
            if (fieldConfiguration.DisplayType == FieldDisplayType.List && FieldGenerator.HasMultipleValues())
                return false;

            // If it's a radio list for a required field then it
            //  shouldn't since no value is not a valid value and
            //  an initial null value translates to none of the radio
            //  boxes being selected
            if (fieldConfiguration.DisplayType == FieldDisplayType.List)
                return !FieldGenerator.Metadata.IsRequired;

            // If it's a multi-select dropdown and required then
            //  there shouldn't be an empty item
            if (FieldGenerator.HasMultipleValues())
                return !FieldGenerator.Metadata.IsRequired;

            // Dropdown lists for nullable types should have an empty item
            if (!FieldGenerator.Metadata.ModelType.IsValueType
                    || Nullable.GetUnderlyingType(FieldGenerator.Metadata.ModelType) != null)
                return true;

            return false;
        }

        /// <summary>
        /// Returns the HTML of a &lt;select&gt; list element.
        /// Automatically adds an empty item where appropriate.
        /// </summary>
        /// <param name="selectList">The list of items to choose from in the select list</param>
        /// <param name="fieldConfiguration">The field configuration to use for attributes and empty item configuration</param>
        /// <returns></returns>
        protected IHtmlContent GetSelectListHtml(IEnumerable<SelectListItem> selectList, IReadonlyFieldConfiguration fieldConfiguration)
        {
            if (HasEmptySelectListItem(fieldConfiguration))
                selectList = new []{GetEmptySelectListItem(fieldConfiguration)}.Union(selectList);

            switch (fieldConfiguration.DisplayType)
            {
                case FieldDisplayType.List:
                    var list = SelectListToRadioList(selectList, fieldConfiguration);
                    return FieldGenerator.Template.RadioOrCheckboxList(list, isCheckbox: FieldGenerator.HasMultipleValues());
                case FieldDisplayType.DropDown:
                case FieldDisplayType.Default:
                    if (FieldGenerator.HasMultipleEnumValues())
                    {
                        var attrs = new HtmlAttributes(fieldConfiguration.HtmlAttributes);
                        AdjustHtmlForModelState(attrs);
                        return HtmlCreator.BuildSelect(GetFieldName(), selectList, multiple: true, htmlAttributes: attrs);
                    }

                    return FieldGenerator.HasEnumerableValues()
                        ? FieldGenerator.HtmlHelper.ListBoxFor(
                            FieldGenerator.FieldProperty, selectList,
                            fieldConfiguration.HtmlAttributes)
                        : FieldGenerator.HtmlHelper.DropDownListFor(
                            FieldGenerator.FieldProperty, selectList,
                            fieldConfiguration.HtmlAttributes);
            }

            return null;
        }

        private string GetEmptySelectListItemText(IReadonlyFieldConfiguration fieldConfiguration)
        {
            if (!string.IsNullOrEmpty(fieldConfiguration.NoneString))
                return fieldConfiguration.NoneString;

            if (FieldGenerator.GetUnderlyingType() == typeof (bool) && !FieldGenerator.Metadata.IsRequired)
                return "Neither";

            if (fieldConfiguration.DisplayType == FieldDisplayType.List)
                return "None";

            if (FieldGenerator.HasMultipleValues())
                return "None";

            return string.Empty;
        }

        private SelectListItem GetEmptySelectListItem(IReadonlyFieldConfiguration fieldConfiguration)
        {
            var selected = FieldGenerator.GetValue() == null;
            if (typeof (T) == typeof (string))
                selected = string.IsNullOrEmpty(FieldGenerator.GetValue() as string);
            return new SelectListItem
            {
                Selected = selected,
                Value = "",
                Text = GetEmptySelectListItemText(fieldConfiguration)
            };
        }

        private IEnumerable<IHtmlContent> SelectListToRadioList(IEnumerable<SelectListItem> selectList, IReadonlyFieldConfiguration fieldConfiguration)
        {
            var count = 0;
            foreach (var item in selectList)
            {
                var id = $"{GetFieldName()}_{++count}";
                var attrs = new HtmlAttributes(fieldConfiguration.HtmlAttributes);
                attrs = attrs.Id(id);
                if (FieldGenerator.HasMultipleValues())
                    AdjustHtmlForModelState(attrs);
                var fieldHtml = FieldGenerator.HasMultipleValues()
                    ? HtmlCreator.BuildSingleCheckbox(GetFieldName(), item.Selected, attrs, item.Value)
                    : FieldGenerator.HtmlHelper.RadioButton(FieldGenerator.HtmlHelper.GetExpressionText(FieldGenerator.FieldProperty), item.Value, item.Selected, attrs.ToDictionary());
                if (fieldConfiguration.ShouldInlineLabelWrapElement)
                {
                    yield return new HtmlContentBuilder()
                        .AppendHtml("<label>")
                        .AppendHtml(fieldHtml)
                        .Append(" ")
                        .AppendHtml(item.Text)
                        .AppendHtml("</label>");
                }
                else
                {
                    yield return new HtmlContentBuilder()
                        .AppendHtml(fieldHtml)
                        .Append(" ")
                        .AppendHtml(FieldGenerator.HtmlHelper.Label(id, item.Text));
                }
            }
        }

        /// <summary>
        /// The value to use for the name of a field (e.g. for the name attribute or looking up model state).
        /// </summary>
        /// <returns>The name of the field</returns>
        protected string GetFieldName()
        {
            var name = FieldGenerator.HtmlHelper.GetExpressionText(FieldGenerator.FieldProperty);
            return FieldGenerator.HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
        }

        /// <summary>
        /// Adjust the HTML attributes of a field based on the state of the model for that field.
        /// e.g. add validation attributes and error attributes.
        /// </summary>
        /// <param name="attrs">The attributes to modify</param>
        protected void AdjustHtmlForModelState(HtmlAttributes attrs)
        {
            var name = FieldGenerator.HtmlHelper.GetExpressionText(FieldGenerator.FieldProperty);
            var fullName = FieldGenerator.HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            if (FieldGenerator.HtmlHelper.ViewContext.ViewData.ModelState.TryGetValue(fullName, out var modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    attrs = attrs.AddClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            var validationHtmlAttributeProvider = FieldGenerator.HtmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<ValidationHtmlAttributeProvider>();
            var modelExpressionProvider = FieldGenerator.HtmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<ModelExpressionProvider>();
            validationHtmlAttributeProvider.AddAndTrackValidationAttributes(
                FieldGenerator.HtmlHelper.ViewContext,
                modelExpressionProvider.CreateModelExpression(FieldGenerator.HtmlHelper.ViewData, FieldGenerator.FieldProperty).ModelExplorer,
                name,
                attrs.Attributes);
        }
    }
}
