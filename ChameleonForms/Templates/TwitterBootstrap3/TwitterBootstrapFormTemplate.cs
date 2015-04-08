﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;
using ChameleonForms.Templates.Default;

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

        private static readonly FieldDisplayType[] NormalFieldTypes = new[] {FieldDisplayType.DropDown, FieldDisplayType.SingleLineText, FieldDisplayType.MultiLineText};

        /// <inheritdoc />
        public override void PrepareFieldConfiguration<TModel, T>(IFieldGenerator<TModel, T> fieldGenerator, IFieldGeneratorHandler<TModel, T> fieldGeneratorHandler, IFieldConfiguration fieldConfiguration, FieldParent fieldParent)
        {
            if (fieldParent == FieldParent.Form)
                return;

            fieldConfiguration.AddValidationClass("help-block");

            var displayType = fieldGeneratorHandler.GetDisplayType(fieldConfiguration.ToReadonly());
            if (NormalFieldTypes.Contains(displayType))
            {
                fieldConfiguration.Bag.CanBeInputGroup = true;
                fieldConfiguration.AddClass("form-control").AddLabelClass("control-label");
            }

            if (displayType == FieldDisplayType.Checkbox)
            {
                fieldConfiguration.Bag.IsCheckboxControl = true;
            }

            if (displayType == FieldDisplayType.List)
            {
                fieldConfiguration.Bag.IsRadioOrCheckboxList = true;
            }
        }

        /// <inheritdoc />
        public override IHtmlString BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            return HtmlCreator.BuildFormTag(action, method, htmlAttributes, enctype);
        }

        /// <inheritdoc />
        public override IHtmlString EndForm()
        {
            return TwitterBootstrapHtmlHelpers.EndForm();
        }

        /// <inheritdoc />
        public override IHtmlString BeginSection(IHtmlString heading = null, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return TwitterBootstrapHtmlHelpers.BeginSection(heading, leadingHtml, htmlAttributes ?? new HtmlAttributes());
        }

        /// <inheritdoc />
        public override IHtmlString EndSection()
        {
            return TwitterBootstrapHtmlHelpers.EndSection();
        }

        /// <inheritdoc />
        public override IHtmlString BeginNestedSection(IHtmlString heading = null, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return TwitterBootstrapHtmlHelpers.BeginNestedSection(heading, leadingHtml, htmlAttributes ?? new HtmlAttributes());
        }

        /// <inheritdoc />
        public override IHtmlString EndNestedSection()
        {
            return TwitterBootstrapHtmlHelpers.EndNestedSection();
        }

        /// <inheritdoc />
        public override IHtmlString Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return TwitterBootstrapHtmlHelpers.Field(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration, isValid, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid));
        }

        /// <inheritdoc />
        public override IHtmlString BeginField(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return TwitterBootstrapHtmlHelpers.BeginField(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration, isValid, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid));
        }

        /// <inheritdoc />
        protected override IHtmlString RequiredDesignator(ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return TwitterBootstrapHtmlHelpers.RequiredDesignator(fieldConfiguration);
        }

        /// <inheritdoc />
        public override IHtmlString EndField()
        {
            return TwitterBootstrapHtmlHelpers.EndField();
        }

        /// <inheritdoc />
        public override IHtmlString BeginMessage(MessageType messageType, IHtmlString heading)
        {
            return TwitterBootstrapHtmlHelpers.BeginMessage(messageType.ToTwitterEmphasisStyle(), heading);
        }

        /// <inheritdoc />
        public override IHtmlString EndMessage()
        {
            return TwitterBootstrapHtmlHelpers.EndMessage();
        }

        /// <inheritdoc />
        public override IHtmlString BeginNavigation()
        {
            return TwitterBootstrapHtmlHelpers.BeginNavigation();
        }

        /// <inheritdoc />
        public override IHtmlString EndNavigation()
        {
            return TwitterBootstrapHtmlHelpers.EndNavigation();
        }

        /// <inheritdoc />
        public override IHtmlString Button(IHtmlString content, string type, string id, string value, HtmlAttributes htmlAttributes)
        {
            htmlAttributes = htmlAttributes ?? new HtmlAttributes();
            htmlAttributes.AddClass("btn");
            if (!StyledButtonClasses.Any(c => htmlAttributes.Attributes["class"].Contains(c)))
                htmlAttributes.AddClass("btn-default");

            if (htmlAttributes.Attributes.ContainsKey(IconAttrKey))
            {
                var icon = htmlAttributes.Attributes[IconAttrKey];
                var iconHtml = string.Format("<span class=\"glyphicon glyphicon-{0}\"></span> ", icon);
                content = content == null
                    ? new HtmlString(iconHtml + value.ToHtml())
                    : new HtmlString(iconHtml + content.ToHtmlString());
                htmlAttributes.Attributes.Remove(IconAttrKey);
            }

            return base.Button(content, type, id, value, htmlAttributes);
        }

        /// <inheritdoc />
        public override IHtmlString RadioOrCheckboxList(IEnumerable<IHtmlString> list, bool isCheckbox)
        {
            return TwitterBootstrapHtmlHelpers.RadioOrCheckboxList(list, isCheckbox);
        }
    }
}