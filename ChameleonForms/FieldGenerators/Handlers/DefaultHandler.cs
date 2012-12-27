using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class DefaultHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public DefaultHandler(IFieldGenerator<TModel, T> fieldGenerator, IFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override HandleAction Handle()
        {
            var html = GetInputHtml(TextInputType.Text);
            return HandleAction.Return(html);
        }
    }
}
