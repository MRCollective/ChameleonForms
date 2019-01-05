using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;


using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Templates;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace ChameleonForms.FieldGenerators
{
    /// <summary>
    /// The default field HTML generator.
    /// </summary>
    /// <typeparam name="TModel">The type of the view model for the form</typeparam>
    /// <typeparam name="T">The type of the field being generated</typeparam>
    public class DefaultFieldGenerator<TModel, T> : IFieldGenerator<TModel, T>
    {
        /// <summary>
        /// Constructs the field generator.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper for the current view</param>
        /// <param name="fieldProperty">Expression to identify the property to generate the field for</param>
        /// <param name="template">The template being used to output the form</param>
        public DefaultFieldGenerator(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, T>> fieldProperty, IFormTemplate template)
        {
            HtmlHelper = htmlHelper;
            FieldProperty = fieldProperty;
            Template = template;
            Metadata = ExpressionMetadataProvider.FromLambdaExpression(FieldProperty, HtmlHelper.ViewData, HtmlHelper.MetadataProvider).Metadata;
        }

        /// <inheritdoc />
        public ModelMetadata Metadata { get; private set; }
        /// <inheritdoc />
        public IHtmlHelper<TModel> HtmlHelper { get; private set; }
        /// <inheritdoc />
        public Expression<Func<TModel, T>> FieldProperty { get; private set; }
        /// <inheritdoc />
        public IFormTemplate Template { get; private set; }

        /// <inheritdoc />
        public IHtmlContent GetLabelHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            fieldConfiguration = fieldConfiguration ?? new FieldConfiguration();

            string @for;
            if (fieldConfiguration.HtmlAttributes.ContainsKey("id"))
            {
                @for = fieldConfiguration.HtmlAttributes["id"].ToString();
            }
            else
            {
                @for = HtmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(
                    ExpressionHelper.GetExpressionText(FieldProperty));
            }

            var labelText = fieldConfiguration.LabelText
                ?? GetFieldDisplayName().ToHtml();

            if (!fieldConfiguration.HasLabelElement)
                return labelText;

            var labelAttrs = new HtmlAttributes();
            if (!string.IsNullOrEmpty(fieldConfiguration.LabelClasses))
                labelAttrs.AddClass(fieldConfiguration.LabelClasses);

            return HtmlCreator.BuildLabel(@for, labelText, labelAttrs);
        }

        /// <inheritdoc />
        public string GetFieldDisplayName()
        {
            return Metadata.DisplayName
                ?? Metadata.PropertyName
                ?? ExpressionHelper.GetExpressionText(FieldProperty).Split('.').Last();
        }

        /// <inheritdoc />
        public IHtmlContent GetValidationHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            return HtmlHelper.ValidationMessageFor(FieldProperty, null, fieldConfiguration.ValidationClasses != null ? new {@class = fieldConfiguration.ValidationClasses} : null);
        }

        /// <inheritdoc />
        public IHtmlContent GetFieldHtml(IFieldConfiguration fieldConfiguration)
        {
            return GetFieldHtml(PrepareFieldConfiguration(fieldConfiguration, FieldParent.Form));
        }

        /// <inheritdoc />
        public IHtmlContent GetLabelHtml(IFieldConfiguration fieldConfiguration)
        {
            return GetLabelHtml(PrepareFieldConfiguration(fieldConfiguration, FieldParent.Form));
        }

        /// <inheritdoc />
        public IHtmlContent GetValidationHtml(IFieldConfiguration fieldConfiguration)
        {
            return GetValidationHtml(PrepareFieldConfiguration(fieldConfiguration, FieldParent.Form));
        }

        /// <inheritdoc />
        public IReadonlyFieldConfiguration PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration, FieldParent fieldParent)
        {
            fieldConfiguration = fieldConfiguration ?? new FieldConfiguration();
            if (!string.IsNullOrEmpty(Metadata.EditFormatString) && string.IsNullOrEmpty(fieldConfiguration.FormatString))
                fieldConfiguration.WithFormatString(Metadata.EditFormatString);
            if (!string.IsNullOrEmpty(Metadata.NullDisplayText) && string.IsNullOrEmpty(fieldConfiguration.NoneString))
                fieldConfiguration.WithNoneAs(Metadata.NullDisplayText);
            if (Metadata.IsReadOnly)
                fieldConfiguration.Readonly();

            var handler = FieldGeneratorHandlersRouter<TModel, T>.GetHandler(this);
            handler.PrepareFieldConfiguration(fieldConfiguration);
            Template.PrepareFieldConfiguration(this, handler, fieldConfiguration, fieldParent);

            return fieldConfiguration;
        }

        /// <inheritdoc />
        public IHtmlContent GetFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            fieldConfiguration = fieldConfiguration ?? new FieldConfiguration();
            if (fieldConfiguration.FieldHtml != null)
                return fieldConfiguration.FieldHtml;
            
            return FieldGeneratorHandlersRouter<TModel, T>.GetHandler(this).GenerateFieldHtml(fieldConfiguration);
        }

        /// <inheritdoc />
        public T GetValue()
        {
            var model = GetModel();

            return model == null ? default(T) : FieldProperty.Compile().Invoke(model);
        }

        /// <inheritdoc />
        public string GetFieldId()
        {
            return ((MemberExpression) FieldProperty.Body).Member.Name;
        }

        /// <inheritdoc />
        public TModel GetModel()
        {
            return (TModel) HtmlHelper.ViewData.Model;
        }
    }
}