using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public override IHtml BeginForm(string action, FormSubmitMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            return HtmlCreator.BuildFormTag(action, method, htmlAttributes, enctype);
        }

        /// <inheritdoc />
        public override IHtml EndForm()
        {
            return TwitterBootstrapHtmlHelpers.EndForm().ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml BeginSection(IHtml heading = null, IHtml leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return TwitterBootstrapHtmlHelpers.BeginSection(heading.ToIHtmlString(), leadingHtml.ToIHtmlString(), htmlAttributes ?? new HtmlAttributes()).ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml EndSection()
        {
            return TwitterBootstrapHtmlHelpers.EndSection().ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml BeginNestedSection(IHtml heading = null, IHtml leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return TwitterBootstrapHtmlHelpers.BeginNestedSection(heading.ToIHtmlString(), leadingHtml.ToIHtmlString(), htmlAttributes ?? new HtmlAttributes()).ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml EndNestedSection()
        {
            return TwitterBootstrapHtmlHelpers.EndNestedSection().ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml Field(IHtml labelHtml, IHtml elementHtml, IHtml validationHtml, IFieldMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return TwitterBootstrapHtmlHelpers.Field(labelHtml.ToIHtmlString(), elementHtml.ToIHtmlString(), validationHtml.ToIHtmlString(), fieldMetadata, fieldConfiguration, isValid, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid).ToIHtmlString()).ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml BeginField(IHtml labelHtml, IHtml elementHtml, IHtml validationHtml, IFieldMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return TwitterBootstrapHtmlHelpers.BeginField(labelHtml.ToIHtmlString(), elementHtml.ToIHtmlString(), validationHtml.ToIHtmlString(), fieldMetadata, fieldConfiguration, isValid, RequiredDesignator(fieldMetadata, fieldConfiguration, isValid).ToIHtmlString()).ToIHtml();
        }

        /// <inheritdoc />
        protected override IHtml RequiredDesignator(IFieldMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return TwitterBootstrapHtmlHelpers.RequiredDesignator(fieldConfiguration).ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml EndField()
        {
            return TwitterBootstrapHtmlHelpers.EndField().ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml BeginMessage(MessageType messageType, IHtml heading)
        {
            return TwitterBootstrapHtmlHelpers.BeginMessage(messageType.ToTwitterEmphasisStyle(), heading.ToIHtmlString()).ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml EndMessage()
        {
            return TwitterBootstrapHtmlHelpers.EndMessage().ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml BeginNavigation()
        {
            return TwitterBootstrapHtmlHelpers.BeginNavigation().ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml EndNavigation()
        {
            return TwitterBootstrapHtmlHelpers.EndNavigation().ToIHtml();
        }

        /// <inheritdoc />
        public override IHtml Button(IHtml content, string type, string id, string value, HtmlAttributes htmlAttributes)
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
                    ? new Html(iconHtml + value.ToHtml())
                    : new Html(iconHtml + content.ToHtmlString());
                htmlAttributes.Attributes.Remove(IconAttrKey);
            }

            return base.Button(content, type, id, value, htmlAttributes);
        }

        /// <inheritdoc />
        public override IHtml RadioOrCheckboxList(IEnumerable<IHtml> list, bool isCheckbox)
        {
            return TwitterBootstrapHtmlHelpers.RadioOrCheckboxList(list, isCheckbox).ToIHtml();
        }
    }
}