using System.Web;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class FileHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public FileHandler(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override bool CanHandle()
        {
            return typeof(HttpPostedFileBase).IsAssignableFrom(FieldGenerator.Metadata.ModelType);
        }

        public override IHtmlString GenerateFieldHtml()
        {
            return GetInputHtml(TextInputType.File);
        }
    }
}
