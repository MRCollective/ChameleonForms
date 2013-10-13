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
        /// <param name="fieldConfiguration">The field configuration to use when outputting the field</param>
        public DateTimeHandler(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        { }

        public override bool CanHandle()
        {
            return GetUnderlyingType(FieldGenerator) == typeof (DateTime);
        }

        public override IHtmlString GenerateFieldHtml()
        {
            return GetInputHtml(TextInputType.Text, FieldGenerator, FieldConfiguration);
        }

        public override void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            if (!string.IsNullOrEmpty(FieldGenerator.Metadata.DisplayFormatString))
                fieldConfiguration.Attr("data-val-format", FieldGenerator.Metadata.DisplayFormatString.Replace("{0:", "").Replace("}", ""));
        }
    }
}
