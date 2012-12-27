using System.ComponentModel.DataAnnotations;
using System.Web.Mvc.Html;
using ChameleonForms.Component.Config;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class TextAreaHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public TextAreaHandler(IFieldGenerator<TModel, T> fieldGenerator, IFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override HandleAction Handle()
        {
            if (FieldGenerator.Metadata.DataTypeName != DataType.MultilineText.ToString())
                return HandleAction.Continue;

            var html = FieldGenerator.HtmlHelper.TextAreaFor(FieldGenerator.FieldProperty, FieldConfiguration.Attributes.ToDictionary());
            return HandleAction.Return(html);
        }
    }
}
