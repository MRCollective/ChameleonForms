using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ChameleonForms.Component.Config;
using Humanizer;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class EnumListHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public EnumListHandler(IFieldGenerator<TModel, T> fieldGenerator, IFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override HandleAction Handle()
        {
            if (!GetUnderlyingType().IsEnum)
                return HandleAction.Continue;

            var selectList = GetSelectList();
            var html = GetSelectListHtml(selectList);
            return HandleAction.Return(html);
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
