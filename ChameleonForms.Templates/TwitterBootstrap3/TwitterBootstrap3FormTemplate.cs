using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using ChameleonForms.Templates.ChameleonFormsTwitterBootstrap3Template;
using ChameleonForms.Templates.ChameleonFormsTwitterBootstrap3Template.Params;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorRenderer;

namespace ChameleonForms.Templates.TwitterBootstrap3
{
    /// <summary>
    /// The default Chameleon Forms form template renderer.
    /// </summary>
    public class TwitterBootstrap3FormTemplate : Default.DefaultFormTemplate
    {
        /// <summary>
        /// The attribute name to use for adding an icon class to a Html Attributes object.
        /// </summary>
        public const string IconAttrKey = "data-chameleonforms-twbs-icon";

        private static readonly IEnumerable<string> StyledButtonClasses = Enum.GetNames(typeof(EmphasisStyle))
            .Select(x => string.Format("btn-{0}", x.ToLower()))
            .ToArray();

        private static readonly FieldDisplayType[] NormalFieldTypes = new[] { FieldDisplayType.DropDown, FieldDisplayType.SingleLineText, FieldDisplayType.MultiLineText };

        /// <inheritdoc />
        public override void PrepareFieldConfiguration<TModel, T>(IFieldGenerator<TModel, T> fieldGenerator, IFieldGeneratorHandler<TModel, T> fieldGeneratorHandler, IFieldConfiguration fieldConfiguration, FieldParent fieldParent)
        {
            if (fieldParent == FieldParent.Form)
                return;

            fieldConfiguration.InlineLabelWrapsElement();

            fieldConfiguration.AddValidationClass("help-block");

            var displayType = fieldGeneratorHandler.GetDisplayType(fieldConfiguration);
            if (NormalFieldTypes.Contains(displayType))
            {
                fieldConfiguration.Bag.CanBeInputGroup = true;
                fieldConfiguration.AddClass("form-control").AddLabelClass("control-label");
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
        public override IHtmlContent BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
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
            return new BeginMessage().Render(new MessageParams {MessageType = messageType.ToTwitterEmphasisStyle(), Heading = heading });
        }

        /// <inheritdoc />
        public override IHtmlContent EndMessage()
        {
            return new EndMessage().Render();
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
                htmlAttributes.AddClass("btn-default");

            if (htmlAttributes.Attributes.ContainsKey(IconAttrKey))
            {
                var icon = htmlAttributes.Attributes[IconAttrKey];
                var iconHtml = string.Format("<span class=\"glyphicon glyphicon-{0}\"></span> ", icon);
                if(content == null)
                {
                    content = new HtmlString(iconHtml + value.ToHtml());
                }
                else
                {
                    var bld = new HtmlContentBuilder();
                    bld.AppendHtml(iconHtml)
                        .AppendHtml(content);
                    content = bld;
                }

                htmlAttributes.Attributes.Remove(IconAttrKey);
            }

            return base.Button(content, type, id, value, htmlAttributes);
        }

        /// <inheritdoc />
        public override IHtmlContent RadioOrCheckboxList(IEnumerable<IHtmlContent> list, bool isCheckbox)
        {
            return new RadioOrCheckboxList().Render(new ListParams {Items = list, IsCheckbox = isCheckbox});
        }
    }
}