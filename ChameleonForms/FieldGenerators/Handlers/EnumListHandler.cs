using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using Humanizer;

namespace ChameleonForms.FieldGenerators.Handlers
{
    /// <summary>
    /// Generates the HTML for the Field Element of enum fields as either a select list or a list of radio buttons.
    /// </summary>
    /// <typeparam name="TModel">The type of the model the form is being output for</typeparam>
    /// <typeparam name="T">The type of the property in the model that the specific field is being output for</typeparam>
    public class EnumListHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        /// <summary>
        /// Constructor for the Enum Field Generator Handler.
        /// </summary>
        /// <param name="fieldGenerator">The field generator for the field</param>
        public EnumListHandler(IFieldGenerator<TModel, T> fieldGenerator)
            : base(fieldGenerator)
        {}

        public override bool CanHandle()
        {
            return GetUnderlyingType(FieldGenerator).IsEnum;
        }

        public override IHtmlString GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            var selectList = GetSelectList();
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
            return fieldConfiguration.DisplayType == FieldDisplayType.List
                ? FieldDisplayType.List
                : FieldDisplayType.DropDown;
        }

        private IEnumerable<SelectListItem> GetSelectList()
        {
            var enumValues = Enum.GetValues(GetUnderlyingType(FieldGenerator));
            foreach (var i in enumValues)
            {
                yield return new SelectListItem
                {
                    Text = (i as Enum).Humanize(),
                    Value = i.ToString(),
                    Selected = IsSelected(i, FieldGenerator)
                };
            }
        }

    }
}
