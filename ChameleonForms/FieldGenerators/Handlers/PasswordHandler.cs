using System.ComponentModel.DataAnnotations;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class PasswordHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public PasswordHandler(IFieldGenerator<TModel, T> fieldGenerator, IFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override HandleAction Handle()
        {
            if (FieldGenerator.Metadata.DataTypeName != DataType.Password.ToString())
                return HandleAction.Continue;
            
            var html = GetInputHtml(TextInputType.Password);
            return HandleAction.Return(html);
        }
    }
}
