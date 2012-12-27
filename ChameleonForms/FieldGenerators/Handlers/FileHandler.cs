using System.Web;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    internal class FileHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        public FileHandler(IFieldGenerator<TModel, T> fieldGenerator, IFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override HandleAction Handle()
        {
            if (!typeof(HttpPostedFileBase).IsAssignableFrom(FieldGenerator.Metadata.ModelType))
                return HandleAction.Continue;

            var html = GetInputHtml(TextInputType.File);
            return HandleAction.Return(html);
        }
    }
}
