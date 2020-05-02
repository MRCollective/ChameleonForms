using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.FieldGenerators.Handlers
{
    /// <summary>
    /// Generates the HTML for the Field Element of text input fields - always returns true when asked if it can handle a field.
    /// </summary>
    /// <typeparam name="TModel">The type of the model the form is being output for</typeparam>
    /// <typeparam name="T">The type of the property in the model that the specific field is being output for</typeparam>
    public class NumberHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        /// <summary>
        /// Constructor for the Default Field Generator Handler.
        /// </summary>
        /// <param name="fieldGenerator">The field generator for the field</param>
        public NumberHandler(IFieldGenerator<TModel, T> fieldGenerator)
            : base(fieldGenerator)
        {}

        /// <inheritdoc />
        public override bool CanHandle()
        {
            return FieldGenerator.IsNumeric();
        }

        /// <inheritdoc />
        public override IHtmlContent GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            return GetInputHtml(TextInputType.Number, fieldConfiguration);
        }

        /// <inheritdoc />
        public override FieldDisplayType GetDisplayType(IReadonlyFieldConfiguration fieldConfiguration)
        {
            return FieldDisplayType.SingleLineText;
        }

        /// <inheritdoc />
        public override void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            if (!fieldConfiguration.Attributes.Has("step"))
            {
                if (FieldGenerator.IsIntegralNumber())
                    fieldConfiguration.Attr("step", 1);
                else if (FieldGenerator.Metadata.DataTypeName == DataType.Currency.ToString())
                    fieldConfiguration.Attr("step", 0.01);
                else
                    fieldConfiguration.Attr("step", "any");
            }

            if (!fieldConfiguration.Attributes.Has("min") || !fieldConfiguration.Attributes.Has("max"))
            {
                object min = null;
                object max = null;

                if (FieldGenerator.GetCustomAttributes().OfType<RangeAttribute>().Any())
                {
                    var converter = TypeDescriptor.GetConverter(FieldGenerator.GetUnderlyingType());
                    var range = FieldGenerator.GetCustomAttributes().OfType<RangeAttribute>().First();
                    min = range.Minimum;
                    max = range.Maximum;
                }
                else
                {
                    var type = FieldGenerator.GetUnderlyingType();

                    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types
                    if (type == typeof(byte))
                    {
                        min = 0;
                        max = 255;
                    }
                    if (type == typeof(sbyte))
                    {
                        min = -128;
                        max = 127;
                    }
                    if (type == typeof(short))
                    {
                        min = -32768;
                        max = 32767;
                    }
                    if (type == typeof(ushort))
                    {
                        min = 0;
                        max = 65535;
                    }
                    if (type == typeof(uint))
                    {
                        min = 0;
                    }
                    if (type == typeof(ulong))
                    {
                        min = 0;
                    }
                }

                if (!fieldConfiguration.Attributes.Has("min") && min != null)
                    fieldConfiguration.Min(min.ToString());
                if (!fieldConfiguration.Attributes.Has("max") && max != null)
                    fieldConfiguration.Max(max.ToString());
            }
        }
    }
}
