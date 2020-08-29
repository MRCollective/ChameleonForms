using System;
using System.Collections.Generic;
using System.Linq;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using ChameleonForms.Templates.ChameleonFormsBootstrap4Template;
using ChameleonForms.Templates.ChameleonFormsBootstrap4Template.Params;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorRenderer;

namespace ChameleonForms.Templates.Bootstrap4
{
    /// <summary>
    /// The default Chameleon Forms form template renderer.
    /// </summary>
    public class Bootstrap4FormTemplate : Default.DefaultFormTemplate
    {
        private static readonly IEnumerable<string> StyledButtonClasses = Enum.GetNames(typeof(ButtonStyle))
            .Select(x => x.Humanize())
            .ToArray();

        private static readonly FieldDisplayType[] NormalFieldTypes = new[] { FieldDisplayType.DropDown, FieldDisplayType.SingleLineText, FieldDisplayType.MultiLineText };

        /// <inheritdoc />
        public override void PrepareFieldConfiguration<TModel, T>(IFieldGenerator<TModel, T> fieldGenerator, IFieldGeneratorHandler<TModel, T> fieldGeneratorHandler, IFieldConfiguration fieldConfiguration, FieldParent fieldParent)
        {
            if (fieldParent == FieldParent.Form)
                return;

            fieldConfiguration.InlineLabelWrapsElement();

            fieldConfiguration.AddValidationClass("invalid-feedback");

            var displayType = fieldGeneratorHandler.GetDisplayType(fieldConfiguration);
            if (NormalFieldTypes.Contains(displayType))
            {
                fieldConfiguration.Bag.CanBeInputGroup = true;
                fieldConfiguration.AddClass("form-control");
            }

            if (displayType == FieldDisplayType.Checkbox)
            {
                fieldConfiguration.Bag.IsCheckboxControl = true;
                // Hide the parent label otherwise it looks weird
                fieldConfiguration.Label("").WithoutLabelElement();
            }

            if (displayType == FieldDisplayType.List)
                fieldConfiguration.Bag.IsRadioOrCheckboxList = true;
        }

        /// <inheritdoc />
        public override IHtmlContent BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype, bool formSubmitted)
        {
            if (formSubmitted)
                htmlAttributes.AddClass("was-validated");
            return HtmlCreator.BuildFormTag(action, method, htmlAttributes, enctype);
        }

        /// <inheritdoc />
        public override IHtmlContent EndForm()
        {
            return new EndForm().Render();
        }

        /// <inheritdoc />
        public override IHtmlContent BeginSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new BeginSection().Render(new BeginSectionParams {Heading = heading, LeadingHtml = leadingHtml, HtmlAttributes = htmlAttributes ?? new HtmlAttributes() });
        }

        /// <inheritdoc />
        public override IHtmlContent EndSection()
        {
            return new EndSection().Render();
        }

        /// <inheritdoc />
        public override IHtmlContent BeginNestedSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new BeginNestedSection().Render(new BeginSectionParams { Heading = heading, LeadingHtml = leadingHtml, HtmlAttributes = htmlAttributes ?? new HtmlAttributes() });
        }

        /// <inheritdoc />
        public override IHtmlContent EndNestedSection()
        {
            return new EndNestedSection().Render();
        }

        /// <inheritdoc />
        public override IHtmlContent Field(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return new Field().Render(new FieldParams
            {
                RenderMode = FieldRenderMode.Field, LabelHtml = labelHtml, ElementHtml = elementHtml,
                ValidationHtml = validationHtml, FieldMetadata = fieldMetadata, FieldConfiguration = fieldConfiguration,
                IsValid = isValid, RequiredDesignator = RequiredDesignator(fieldMetadata, fieldConfiguration, isValid)
            });
        }

        /// <inheritdoc />
        public override IHtmlContent BeginField(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return new Field().Render(new FieldParams
            {
                RenderMode = FieldRenderMode.BeginField,
                LabelHtml = labelHtml,
                ElementHtml = elementHtml,
                ValidationHtml = validationHtml,
                FieldMetadata = fieldMetadata,
                FieldConfiguration = fieldConfiguration,
                IsValid = isValid,
                RequiredDesignator = RequiredDesignator(fieldMetadata, fieldConfiguration, isValid)
            });
        }

        /// <inheritdoc />
        protected override IHtmlContent RequiredDesignator(ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return new RequiredDesignator().Render();
        }

        /// <inheritdoc />
        public override IHtmlContent EndField()
        {
            return new EndField().Render();
        }

        /// <inheritdoc />
        public override IHtmlContent BeginMessage(MessageType messageType, IHtmlContent heading)
        {
            string alertType;
            switch (messageType)
            {
                case MessageType.Warning:
                    alertType = "warning";
                    break;
                case MessageType.Action:
                    alertType = "primary";
                    break;
                case MessageType.Failure:
                    alertType = "danger";
                    break;
                case MessageType.Success:
                    alertType = "success";
                    break;
                default:
                    alertType = "info";
                    break;
            }

            return new BeginAlert().Render(new AlertParams {AlertType = alertType, Heading = heading });
        }

        /// <inheritdoc />
        public override IHtmlContent EndMessage()
        {
            return new EndAlert().Render();
        }

        /// <inheritdoc />
        public override IHtmlContent BeginNavigation()
        {
            return new BeginNavigation().Render();
        }

        /// <inheritdoc />
        public override IHtmlContent EndNavigation()
        {
            return new EndNavigation().Render();
        }

        /// <inheritdoc />
        public override IHtmlContent Button(IHtmlContent content, string type, string id, string value, HtmlAttributes htmlAttributes)
        {
            htmlAttributes = htmlAttributes ?? new HtmlAttributes();
            htmlAttributes.AddClass("btn");
            if (!StyledButtonClasses.Any(c => htmlAttributes.Attributes["class"].Contains(c)))
                htmlAttributes.AddClass("btn-light");

            return base.Button(content, type, id, value, htmlAttributes);
        }

        /// <inheritdoc />
        public override IHtmlContent RadioOrCheckboxList(IEnumerable<IHtmlContent> list, bool isCheckbox)
        {
            return new RadioOrCheckboxList().Render(new ListParams {Items = list, IsCheckbox = isCheckbox});
        }
    }
}