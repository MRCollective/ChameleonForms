using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using Humanizer;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class EnumListHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public EnumListHandler(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override bool CanHandle()
        {
            return GetUnderlyingType().IsEnum;
        }

        public override IHtmlString GenerateFieldHtml()
        {
            var selectList = GetSelectList();
            return GetSelectListHtml(selectList);
        }

        public override void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            // If a list is being displayed there is no element for the label to point to so drop it
            if (fieldConfiguration.DisplayType == FieldDisplayType.List)
                fieldConfiguration.WithoutLabel();
        }

        private IEnumerable<SelectListItem> GetSelectList()
        {
            var enumValues = Enum.GetValues(GetUnderlyingType());
            foreach (var i in enumValues)
            {
                yield return new SelectListItem
                {
                    Text = (i as Enum).Humanize(),
                    Value = i.ToString(),
                    Selected = IsSelected(i)
                };
            }
        }

    }
}
