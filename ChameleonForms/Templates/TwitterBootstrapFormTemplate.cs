using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.FieldGenerators.Handlers;

namespace ChameleonForms.Templates
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

        public override void PrepareFieldConfiguration<TModel, T>(IFieldGenerator<TModel, T> fieldGenerator, IFieldGeneratorHandler<TModel, T> fieldGeneratorHandler, IFieldConfiguration fieldConfiguration)
        {
            Action applyFormControl = () => fieldConfiguration.AddClass("form-control").WithLabelClasses(string.IsNullOrEmpty(fieldConfiguration.LabelClasses) ? "control-label" : fieldConfiguration.LabelClasses + " control-label");

            if (
                new[]
                {
                    typeof (DateTimeHandler<TModel, T>), typeof (DefaultHandler<TModel, T>),
                    typeof (PasswordHandler<TModel, T>), typeof (TextAreaHandler<TModel, T>)
                }.Any(t => t.IsInstanceOfType(fieldGeneratorHandler))
            )
                applyFormControl();

            if (new[] {FieldDisplayType.Default, FieldDisplayType.DropDown}.Contains(fieldConfiguration.DisplayType) &&
                new[] {typeof (EnumListHandler<TModel, T>), typeof (ListHandler<TModel, T>)}.Any(t => t.IsInstanceOfType(fieldGeneratorHandler))
            )
                applyFormControl();

            if (typeof(T) == typeof(bool) && fieldConfiguration.DisplayType == FieldDisplayType.Default)
            {
                fieldConfiguration.Bag.IsCheckboxControl = true;
                // Hide the parent label otherwise it looks weird
                fieldConfiguration.Label("").WithoutLabel();
            }
            else if (fieldGeneratorHandler is BooleanHandler<TModel, T> && fieldConfiguration.DisplayType != FieldDisplayType.List)
            {
                applyFormControl();
            }
        }

        public override IHtmlString BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype)
        {
            return HtmlCreator.BuildFormTag(action, method, htmlAttributes, enctype);
        }

        public override IHtmlString EndForm()
        {
            return TwitterBootstrapHtmlHelpers.EndForm();
        }

        public override IHtmlString BeginSection(IHtmlString title, IHtmlString leadingHtml, HtmlAttributes htmlAttributes)
        {
            return TwitterBootstrapHtmlHelpers.BeginSection(title, leadingHtml, htmlAttributes);
        }

        public override IHtmlString EndSection()
        {
            return TwitterBootstrapHtmlHelpers.EndSection();
        }

        public override IHtmlString BeginNestedSection(IHtmlString title, IHtmlString leadingHtml, HtmlAttributes htmlAttributes)
        {
            throw new NotSupportedException("Twitter bootstrap does not support nested form sections.");
        }

        public override IHtmlString EndNestedSection()
        {
            throw new NotSupportedException("Twitter bootstrap does not support nested form sections.");
        }

        public override IHtmlString Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            return TwitterBootstrapHtmlHelpers.Field(labelHtml, elementHtml, validationHtml, fieldMetadata, fieldConfiguration, isValid);
        }

        public override IHtmlString BeginField(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            throw new NotSupportedException("Twitter bootstrap does not support nested form fields.");
        }

        public override IHtmlString RequiredDesignator(ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid)
        {
            throw new NotImplementedException();
        }

        public override IHtmlString EndField()
        {
            throw new NotSupportedException("Twitter bootstrap does not support nested form fields.");
        }

        public override IHtmlString BeginMessage(MessageType messageType, IHtmlString heading)
        {
            return TwitterBootstrapHtmlHelpers.BeginMessage(messageType.ToTwitterAlertType(), heading);
        }

        public override IHtmlString EndMessage()
        {
            return TwitterBootstrapHtmlHelpers.EndMessage();
        }

        public override IHtmlString BeginNavigation()
        {
            return TwitterBootstrapHtmlHelpers.BeginNavigation();
        }

        public override IHtmlString EndNavigation()
        {
            return TwitterBootstrapHtmlHelpers.EndNavigation();
        }

        public override IHtmlString Button(IHtmlString content, string type, string id, string value, HtmlAttributes htmlAttributes)
        {
            htmlAttributes = htmlAttributes ?? new HtmlAttributes();
            htmlAttributes.AddClass("btn");
            if (!htmlAttributes.Attributes["class"].Contains("btn-"))
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
    }
}