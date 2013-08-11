using System.Web;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class DefaultHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public DefaultHandler(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override bool CanHandle()
        {
            return true;
        }

        public override IHtmlString GenerateFieldHtml()
        {
            return GetInputHtml(TextInputType.Text);
        }
    }
}
