using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using ChameleonForms.Generated;
using ChameleonForms.Templates.Default;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChameleonForms.Templates.TwitterBootstrap3
{
    /// <summary>
    /// The default Chameleon Forms form template renderer.
    /// </summary>
    public class TwitterBootstrapFormTemplate : DefaultFormTemplate
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
            return TwitterBootstrapHtmlHelpers.EndForm();
        }

        /// <inheritdoc />
        public override IHtmlContent BeginSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return TwitterBootstrapHtmlHelpers.BeginSection(heading, leadingHtml, htmlAttributes ?? new HtmlAttributes());
        }

        /// <inheritdoc />
        public override IHtmlContent EndSection()
        {
            return TwitterBootstrapHtmlHelpers.EndSection();
        }

        /// <inheritdoc />
        public override IHtmlContent BeginNestedSection(IHtmlContent heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return TwitterBootstrapHtmlHelpers.BeginNestedSection(heading, leadingHtml, htmlAttributes ?? new HtmlAttributes());
        }

        /// <inheritdoc />
        public override IHtmlContent EndNestedSection()
        {
            return TwitterBootstrapHtmlHelpers.EndNestedSection();
        }

        /// <inheritdoc />
        public override IHtmlContent Field(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return TwitterBootstrapHtmlHelpers.Field(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration, isValid, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid));
        }

        /// <inheritdoc />
        public override IHtmlContent BeginField(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return TwitterBootstrapHtmlHelpers.BeginField(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration, isValid, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid));
        }

        /// <inheritdoc />
        protected override IHtmlContent RequiredDesignator(ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return TwitterBootstrapHtmlHelpers.RequiredDesignator(fieldConfiguration);
        }

        /// <inheritdoc />
        public override IHtmlContent EndField()
        {
            return TwitterBootstrapHtmlHelpers.EndField();
        }

        /// <inheritdoc />
        public override IHtmlContent BeginMessage(MessageType messageType, IHtmlContent heading, bool emptyHeading)
        {
            return TwitterBootstrapHtmlHelpers.BeginMessage(messageType.ToTwitterEmphasisStyle(), heading, emptyHeading);
        }

        /// <inheritdoc />
        public override IHtmlContent EndMessage()
        {
            return TwitterBootstrapHtmlHelpers.EndMessage();
        }

        /// <inheritdoc />
        public override IHtmlContent BeginNavigation()
        {
            return TwitterBootstrapHtmlHelpers.BeginNavigation();
        }

        /// <inheritdoc />
        public override IHtmlContent EndNavigation()
        {
            return TwitterBootstrapHtmlHelpers.EndNavigation();
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
            return TwitterBootstrapHtmlHelpers.RadioOrCheckboxList(list, isCheckbox);
        }
    }
}