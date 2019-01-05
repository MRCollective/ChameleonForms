using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        /// <inheritdoc />
        public override bool CanHandle()
        {
            return GetUnderlyingType(FieldGenerator).IsEnum;
        }

        /// <inheritdoc />
        public override IHtmlContent GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            var selectList = GetSelectList(fieldConfiguration.ExcludedEnums);
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
            return fieldConfiguration.DisplayType == FieldDisplayType.List
                ? FieldDisplayType.List
                : FieldDisplayType.DropDown;
        }

        private IEnumerable<SelectListItem> GetSelectList(Enum[] excludeEnums)
        {
            var enumValues = Enum.GetValues(GetUnderlyingType(FieldGenerator));
            foreach (var i in enumValues)
            {
                if (excludeEnums.Contains(i))
                    continue;

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
