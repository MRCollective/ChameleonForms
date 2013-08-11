using System;
using System.Web;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class DateTimeHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public DateTimeHandler(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        { }

        public override bool CanHandle()
        {
            return GetUnderlyingType() == typeof (DateTime);
        }

        public override IHtmlString GenerateFieldHtml()
        {
            return GetInputHtml(TextInputType.Text);
        }

        public override void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            if (!string.IsNullOrEmpty(FieldGenerator.Metadata.DisplayFormatString))
                fieldConfiguration.Attr("data-val-format", FieldGenerator.Metadata.DisplayFormatString.Replace("{0:", "").Replace("}", ""));
        }
    }
}
