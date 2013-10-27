using System;
using System.Web;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    /// <summary>
    /// Generates the HTML for the Field Element of datetime fields.
    /// </summary>
    /// <typeparam name="TModel">The type of the model the form is being output for</typeparam>
    /// <typeparam name="T">The type of the property in the model that the specific field is being output for</typeparam>
    public class DateTimeHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        /// <summary>
        /// Constructor for the DateTime Field Generator Handler.
        /// </summary>
        /// <param name="fieldGenerator">The field generator for the field</param>
        public DateTimeHandler(IFieldGenerator<TModel, T> fieldGenerator)
            : base(fieldGenerator)
        { }

        public override bool CanHandle()
        {
            return GetUnderlyingType(FieldGenerator) == typeof (DateTime);
        }

        public override IHtmlString GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            return GetInputHtml(TextInputType.Text, FieldGenerator, fieldConfiguration);
        }

        public override void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            base.PrepareFieldConfiguration(fieldConfiguration);

            if (!string.IsNullOrEmpty(FieldGenerator.Metadata.DisplayFormatString))
                fieldConfiguration.Attr("data-val-format", FieldGenerator.Metadata.DisplayFormatString.Replace("{0:", "").Replace("}", ""));
        }

        public override FieldDisplayType GetDisplayType(IReadonlyFieldConfiguration fieldConfiguration)
        {
            return FieldDisplayType.SingleLineText;
        }
    }
}
