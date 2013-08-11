using System.ComponentModel.DataAnnotations;
using System.Web;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class PasswordHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public PasswordHandler(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override bool CanHandle()
        {
            return FieldGenerator.Metadata.DataTypeName == DataType.Password.ToString();
        }

        public override IHtmlString GenerateFieldHtml()
        {
            return GetInputHtml(TextInputType.Password);
        }
    }
}
