using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ChameleonForms.Attributes;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.Templates;
using Humanizer;

namespace ChameleonForms.FieldGenerators
{
    /// <summary>
    /// The default field HTML generator.
    /// </summary>
    /// <typeparam name="TModel">The type of the view model for the form</typeparam>
    /// <typeparam name="T">The type of the field being generated</typeparam>
    public class DefaultFieldGenerator<TModel, T> : IFieldGenerator<TModel, T>
    {
        /// <summary>
        /// Constructs the field generator.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper for the current view</param>
        /// <param name="fieldProperty">Expression to identify the property to generate the field for</param>
        public DefaultFieldGenerator(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, T>> fieldProperty)
        {
            HtmlHelper = htmlHelper;
            FieldProperty = fieldProperty;
            Metadata = ModelMetadata.FromLambdaExpression(FieldProperty, HtmlHelper.ViewData);
        }

        public ModelMetadata Metadata { get; private set; }
        public HtmlHelper<TModel> HtmlHelper { get; private set; }
        public Expression<Func<TModel, T>> FieldProperty { get; private set; }

        public IHtmlString GetLabelHtml(IFieldConfiguration fieldConfiguration)
        {
            return HtmlHelper.LabelFor(FieldProperty, fieldConfiguration == null ? null : fieldConfiguration.LabelText);
        }

        public IHtmlString GetValidationHtml(IFieldConfiguration fieldConfiguration)
        {
            return HtmlHelper.ValidationMessageFor(FieldProperty);
        }

        public IHtmlString GetFieldHtml(IFieldConfiguration fieldConfiguration)
        {
            fieldConfiguration = fieldConfiguration ?? new FieldConfiguration();
            
            var typeAttribute = default(string);

            if (Metadata.ModelType.IsEnum)
                return GetEnumHtml(fieldConfiguration);

            if (Metadata.DataTypeName == DataType.Password.ToString())
                return HtmlHelper.PasswordFor(FieldProperty, fieldConfiguration.Attributes.ToDictionary());

            if (Metadata.DataTypeName == DataType.MultilineText.ToString())
                return HtmlHelper.TextAreaFor(FieldProperty, fieldConfiguration.Attributes.ToDictionary());

            if (Metadata.ModelType == typeof(bool))
                return GetSingleCheckboxHtml(fieldConfiguration);

            if (typeof(HttpPostedFileBase).IsAssignableFrom(Metadata.ModelType))
                typeAttribute = "file";

            if (Metadata.AdditionalValues.ContainsKey(ExistsInAttribute.ExistsKey) &&
                    Metadata.AdditionalValues[ExistsInAttribute.ExistsKey] as bool? == true)
                return GetListHtml(fieldConfiguration);

            if (typeAttribute == default(string))
                typeAttribute = "text";

            fieldConfiguration.Attributes.Attr(type => typeAttribute);
            return HtmlHelper.TextBoxFor(FieldProperty, fieldConfiguration.Attributes.ToDictionary());
        }

        #region Helpers

        /// <summary>
        /// Returns the current value of the field.
        /// </summary>
        /// <returns>The current value</returns>
        private T GetValue()
        {
            return FieldProperty.Compile().Invoke(GetModel());
        }

        private TModel GetModel()
        {
            return (TModel) HtmlHelper.ViewData.ModelMetadata.Model;
        }

        /// <summary>
        /// Creates the HTML for a drop down list that wraps an enumeration field.
        /// </summary>
        /// <param name="fieldConfiguration">The field configuration for the field</param>
        /// <returns>The HTML for the drop down list</returns>
        public virtual IHtmlString GetEnumHtml(IFieldConfiguration fieldConfiguration)
        {
            var selectList = Enum.GetValues(typeof(T)).OfType<T>().Select(i => new SelectListItem { Text = (i as Enum).Humanize(), Value = i.ToString(), Selected = i.Equals(GetValue())});
            return GetDropDown(selectList, fieldConfiguration);
        }

        /// <summary>
        /// Creates the HTML for a single checkbox.
        /// </summary>
        /// <param name="fieldConfiguration">The field configuration for the field</param>
        /// <returns>The HTML for the single checkbox</returns>
        public virtual IHtmlString GetSingleCheckboxHtml(IFieldConfiguration fieldConfiguration)
        {
            var value = GetValue() as bool? ?? false;
            var name = ExpressionHelper.GetExpressionText(FieldProperty);
            var fullName = HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var selectList = GetBooleanSelectList(fieldConfiguration, value);
            if (fieldConfiguration.DisplayType == FieldDisplayType.DropDown)
                return GetDropDown(selectList, fieldConfiguration);
            if (fieldConfiguration.DisplayType == FieldDisplayType.List)
                return HtmlHelpers.List(GetList(fullName, selectList, fieldConfiguration));

            AdjustHtmlForModelState(fieldConfiguration.Attributes);

            var fieldhtml = HtmlCreator.BuildSingleCheckbox(fullName, value, fieldConfiguration.Attributes);

            var labelHtml = HtmlHelper.LabelFor(FieldProperty, fieldConfiguration.InlineLabelText);

            return new HtmlString(string.Format("{0} {1}", fieldhtml, labelHtml));
        }

        private IEnumerable<IHtmlString> GetList(string baseId, IEnumerable<SelectListItem> selectList, IFieldConfiguration fieldConfiguration)
        {
            var count = 0;
            foreach (var item in selectList)
            {
                var id = string.Format("{0}_{1}", baseId, ++count);
                var attrs = new HtmlAttributes(fieldConfiguration.Attributes.ToDictionary());
                if (item.Selected)
                    attrs.Attr("checked", "checked");
                attrs.Attr("id", id);
                yield return new HtmlString(string.Format("{0} {1}",
                    HtmlHelper.RadioButtonFor(FieldProperty, item.Value, attrs.ToDictionary()),
                    HtmlHelper.Label(id, item.Text)
                ));
            }
        }

        private IHtmlString GetDropDown(IEnumerable<SelectListItem> selectList, IFieldConfiguration fieldConfiguration)
        {
            return HtmlHelper.DropDownListFor(FieldProperty, selectList, fieldConfiguration.Attributes.ToDictionary());
        }

        private static IEnumerable<SelectListItem> GetBooleanSelectList(IFieldConfiguration fieldConfiguration, bool value)
        {
            yield return new SelectListItem{Value = "true", Text = fieldConfiguration.TrueString, Selected = value};
            yield return new SelectListItem{Value = "false", Text = fieldConfiguration.FalseString, Selected = !value};
        }

        private IHtmlString GetListHtml(IFieldConfiguration fieldConfiguration)
        {
            var name = ExpressionHelper.GetExpressionText(FieldProperty);
            var fullName = HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var model = GetModel();
            var listProperty = model.GetType().GetProperty((string)Metadata.AdditionalValues[ExistsInAttribute.PropertyKey]);
            var listValue = (IEnumerable) listProperty.GetValue(model, null);
            var selectList = GetSelectList(listValue, (string)Metadata.AdditionalValues[ExistsInAttribute.NameKey], (string)Metadata.AdditionalValues[ExistsInAttribute.ValueKey], GetValue());

            if (fieldConfiguration.DisplayType == FieldDisplayType.List)
                return HtmlHelpers.List(GetList(fullName, selectList, fieldConfiguration));

            return GetDropDown(selectList, fieldConfiguration);
        }

        private IEnumerable<SelectListItem> GetSelectList(IEnumerable listValue, string nameProperty, string valueProperty, object selectedValue)
        {
            foreach (var item in listValue)
            {
                var name = item.GetType().GetProperty(nameProperty).GetValue(item, null);
                var value = item.GetType().GetProperty(valueProperty).GetValue(item, null);
                yield return new SelectListItem { Selected = value.Equals(selectedValue), Value = value.ToString(), Text = name.ToString() };
            }
        }

        private void AdjustHtmlForModelState(HtmlAttributes attributes)
        {
            var name = ExpressionHelper.GetExpressionText(FieldProperty);
            var fullName = HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            ModelState modelState;
            if (HtmlHelper.ViewContext.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    attributes.AddClass(System.Web.Mvc.HtmlHelper.ValidationInputCssClassName);
                }
            }

            attributes.Attrs(HtmlHelper.GetUnobtrusiveValidationAttributes(name, ModelMetadata.FromLambdaExpression(FieldProperty, HtmlHelper.ViewData)));
        }
        #endregion
    }
}
