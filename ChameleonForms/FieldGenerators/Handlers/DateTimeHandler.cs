﻿using System;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using System.Globalization;
using Microsoft.AspNetCore.Html;

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

        /// <inheritdoc />
        public override bool CanHandle()
        {
            return FieldGenerator.GetUnderlyingType() == typeof (DateTime);
        }

        /// <inheritdoc />
        public override IHtmlContent GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            return GetInputHtml(TextInputType.Text, fieldConfiguration);
        }

        /// <inheritdoc />
        public override void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            if (!string.IsNullOrEmpty(FieldGenerator.Metadata.EditFormatString))
            {
                var format = FieldGenerator.Metadata.EditFormatString.Replace("{0:", "").Replace("}", "");
                if (format == "g")
                    format = string.Join(" ", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

                fieldConfiguration.Attr("data-val-format", format);
            }
        }

        /// <inheritdoc />
        public override FieldDisplayType GetDisplayType(IReadonlyFieldConfiguration fieldConfiguration)
        {
            return FieldDisplayType.SingleLineText;
        }
    }
}
