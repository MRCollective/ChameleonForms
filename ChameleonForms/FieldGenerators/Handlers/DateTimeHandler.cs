using System;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class DateTimeHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public DateTimeHandler(IFieldGenerator<TModel, T> fieldGenerator, IFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        { }

        public override HandleAction Handle()
        {
            if (typeof(T) != typeof(DateTime) && typeof(T) != typeof(DateTime?))
                return HandleAction.Continue;

            if (!string.IsNullOrEmpty(FieldGenerator.Metadata.DisplayFormatString))
                FieldConfiguration.Attr("data-val-format", FieldGenerator.Metadata.DisplayFormatString.Replace("{0:", "").Replace("}", ""));

            var html = GetInputHtml(TextInputType.Text);
            return HandleAction.Return(html);
        }
    }
}
