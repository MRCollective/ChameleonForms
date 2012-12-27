using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!FieldGenerator.Metadata.ModelType.IsEnum)
                return HandleAction.Continue;

            var selectList = GetSelectList(FieldGenerator.GetValue());
            var html = GetSelectListHtml(selectList);
            return HandleAction.Return(html);
        }

        private IEnumerable<SelectListItem> GetSelectList(T value)
        {
            return Enum.GetValues(typeof (T))
                .OfType<T>()
                .Select(i => new SelectListItem
                {
                    Text = (i as Enum).Humanize(),
                    Value = i.ToString(),
                    Selected = i.Equals(value)
                }
            );
        } 
    }
}
