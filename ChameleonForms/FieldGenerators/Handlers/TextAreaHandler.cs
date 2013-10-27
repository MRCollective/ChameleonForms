using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc.Html;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    /// <summary>
    /// Generates the HTML for the Field Element of textarea fields.
    /// </summary>
    /// <typeparam name="TModel">The type of the model the form is being output for</typeparam>
    /// <typeparam name="T">The type of the property in the model that the specific field is being output for</typeparam>
    public class TextAreaHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        /// <summary>
        /// Constructor for the TextArea Field Generator Handler.
        /// </summary>
        /// <param name="fieldGenerator">The field generator for the field</param>
        public TextAreaHandler(IFieldGenerator<TModel, T> fieldGenerator)
            : base(fieldGenerator)
        {}

        public override bool CanHandle()
        {
            return FieldGenerator.Metadata.DataTypeName == DataType.MultilineText.ToString();
        }

        public override IHtmlString GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration)
        {
            return FieldGenerator.HtmlHelper.TextAreaFor(
                FieldGenerator.FieldProperty,
                fieldConfiguration.HtmlAttributes
            );
        }

        public override FieldDisplayType GetDisplayType(IReadonlyFieldConfiguration fieldConfiguration)
        {
            return FieldDisplayType.MultiLineText;
        }
    }
}
