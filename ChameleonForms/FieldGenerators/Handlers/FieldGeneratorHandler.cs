using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.Templates;
using System.Linq;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal abstract class FieldGeneratorHandler<TModel, T>
    {
        private static readonly List<Type> NumericTypes = new List<Type>
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

        protected readonly IFieldGenerator<TModel, T> FieldGenerator;
        protected readonly IReadonlyFieldConfiguration FieldConfiguration;

        protected FieldGeneratorHandler(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
        {
            FieldGenerator = fieldGenerator;
            FieldConfiguration = fieldConfiguration;
        }

        public abstract bool CanHandle();
        public abstract IHtmlString GenerateFieldHtml();
        public virtual void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration) {}

        protected bool HasMultipleValues()
        {
            return FieldGenerator.Metadata.ModelType.IsGenericType
                && typeof(IEnumerable).IsAssignableFrom(FieldGenerator.Metadata.ModelType);
        }

        protected IEnumerable<object> GetValues()
        {
            return (((IEnumerable)FieldGenerator.GetValue()) ?? new object[]{}).Cast<object>();
        }

        protected bool IsSelected(object value)
        {
            if (HasMultipleValues())
                return GetValues().Contains(value);

            var val = FieldGenerator.GetValue();
            if (val != null)
                return val.Equals(value);

            return value == null;
        }

        public Type GetUnderlyingType()
        {
            var type = FieldGenerator.Metadata.ModelType;

            if (HasMultipleValues())
                type = type.GetGenericArguments()[0];

            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public bool IsNumeric()
        {
            return NumericTypes.Contains(GetUnderlyingType());
        }

        protected IHtmlString GetInputHtml(TextInputType inputType)
        {
            if (inputType == TextInputType.Password)
                return FieldGenerator.HtmlHelper.PasswordFor(FieldGenerator.FieldProperty, FieldConfiguration.HtmlAttributes);

            var attrs = new HtmlAttributes(FieldConfiguration.HtmlAttributes);
            if (!attrs.Attributes.ContainsKey("type"))
                attrs.Attr(type => inputType.ToString().ToLower());
            return !string.IsNullOrEmpty(FieldConfiguration.FormatString)
                ? FieldGenerator.HtmlHelper.TextBoxFor(FieldGenerator.FieldProperty, FieldConfiguration.FormatString, attrs.ToDictionary())
                : FieldGenerator.HtmlHelper.TextBoxFor(FieldGenerator.FieldProperty, attrs.ToDictionary());
        }

        private bool HasEmptySelectListItem()
        {
            if (FieldConfiguration.EmptyItemHidden)
                return false;

            // If it's a checkbox list then it
            //  shouldn't since you can uncheck everything
            if (FieldConfiguration.DisplayType == FieldDisplayType.List && HasMultipleValues())
                return false;

            // If it's a radio list for a required field then it
            //  shouldn't since no value is not a valid value and
            //  an initial null value translates to none of the radio
            //  boxes being selected
            if (FieldConfiguration.DisplayType == FieldDisplayType.List)
                return !FieldGenerator.Metadata.IsRequired;

            // If it's a multi-select dropdown and required then
            //  there shouldn't be an empty item
            if (HasMultipleValues())
                return !FieldGenerator.Metadata.IsRequired;

            // Dropdown lists for nullable types should have an empty item
            if (!FieldGenerator.Metadata.ModelType.IsValueType
                    || Nullable.GetUnderlyingType(FieldGenerator.Metadata.ModelType) != null)
                return true;

            return false;
        }

        protected IHtmlString GetSelectListHtml(IEnumerable<SelectListItem> selectList)
        {
            if (HasEmptySelectListItem())
                selectList = new []{GetEmptySelectListItem()}.Union(selectList);

            switch (FieldConfiguration.DisplayType)
            {
                case FieldDisplayType.List:
                    var list = SelectListToRadioList(selectList);
                    return HtmlHelpers.List(list);
                case FieldDisplayType.DropDown:
                case FieldDisplayType.Default:
                    return HasMultipleValues()
                        ? FieldGenerator.HtmlHelper.ListBoxFor(
                            FieldGenerator.FieldProperty, selectList,
                            FieldConfiguration.HtmlAttributes)
                        : FieldGenerator.HtmlHelper.DropDownListFor(
                            FieldGenerator.FieldProperty, selectList,
                            FieldConfiguration.HtmlAttributes);
            }

            return null;
        }

        private string GetEmptySelectListItemText()
        {
            if (!string.IsNullOrEmpty(FieldConfiguration.NoneString))
                return FieldConfiguration.NoneString;

            if (GetUnderlyingType() == typeof (bool) && !FieldGenerator.Metadata.IsRequired)
                return "Neither";

            if (FieldConfiguration.DisplayType == FieldDisplayType.List)
                return "None";

            if (HasMultipleValues())
                return "None";

            return string.Empty;
        }

        private SelectListItem GetEmptySelectListItem()
        {
            var selected = FieldGenerator.GetValue() == null;
            if (typeof (T) == typeof (string))
                selected = string.IsNullOrEmpty(FieldGenerator.GetValue() as string);
            return new SelectListItem
            {
                Selected = selected,
                Value = "",
                Text = GetEmptySelectListItemText()
            };
        }

        private IEnumerable<IHtmlString> SelectListToRadioList(IEnumerable<SelectListItem> selectList)
        {
            var count = 0;
            foreach (var item in selectList)
            {
                var id = string.Format("{0}_{1}", GetFieldName(), ++count);
                var attrs = new HtmlAttributes(FieldConfiguration.HtmlAttributes);
                if (item.Selected)
                    attrs.Attr("checked", "checked");
                attrs.Attr("id", id);
                if (HasMultipleValues())
                    AdjustHtmlForModelState(attrs);
                yield return new HtmlString(string.Format("{0} {1}",
                    HasMultipleValues()
                        ? HtmlCreator.BuildSingleCheckbox(GetFieldName(), item.Selected, attrs, item.Value)
                        : FieldGenerator.HtmlHelper.RadioButtonFor(FieldGenerator.FieldProperty, item.Value, attrs.ToDictionary()),
                    FieldGenerator.HtmlHelper.Label(id, item.Text)
                ));
            }
        }
        
        protected string GetFieldName()
        {
            var name = ExpressionHelper.GetExpressionText(FieldGenerator.FieldProperty);
            return FieldGenerator.HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
        }
        
        protected void AdjustHtmlForModelState(HtmlAttributes attrs)
        {
            var name = ExpressionHelper.GetExpressionText(FieldGenerator.FieldProperty);
            var fullName = FieldGenerator.HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            ModelState modelState;
            if (FieldGenerator.HtmlHelper.ViewContext.ViewData.ModelState.TryGetValue(fullName, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    attrs.AddClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            attrs.Attrs(FieldGenerator.HtmlHelper.GetUnobtrusiveValidationAttributes(name, ModelMetadata.FromLambdaExpression(FieldGenerator.FieldProperty, FieldGenerator.HtmlHelper.ViewData)));
        }
    }

}
