using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc.Html;
using ChameleonForms.Component.Config;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class TextAreaHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public TextAreaHandler(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override bool CanHandle()
        {
            return FieldGenerator.Metadata.DataTypeName == DataType.MultilineText.ToString();
        }

        public override IHtmlString GenerateFieldHtml()
        {
            return FieldGenerator.HtmlHelper.TextAreaFor(
                FieldGenerator.FieldProperty,
                FieldConfiguration.HtmlAttributes
            );
        }
    }
}
