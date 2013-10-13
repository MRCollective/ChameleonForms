using System.Web;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;

namespace ChameleonForms.FieldGenerators.Handlers
{
    /// <summary>
    /// Generates the HTML for the Field Element of file upload fields.
    /// </summary>
    /// <typeparam name="TModel">The type of the model the form is being output for</typeparam>
    /// <typeparam name="T">The type of the property in the model that the specific field is being output for</typeparam>
    public class FileHandler<TModel, T> : FieldGeneratorHandler<TModel, T>
    {
        /// <summary>
        /// Constructor for the File Field Generator Handler.
        /// </summary>
        /// <param name="fieldGenerator">The field generator for the field</param>
        /// <param name="fieldConfiguration">The field configuration to use when outputting the field</param>
        public FileHandler(IFieldGenerator<TModel, T> fieldGenerator, IReadonlyFieldConfiguration fieldConfiguration)
            : base(fieldGenerator, fieldConfiguration)
        {}

        public override bool CanHandle()
        {
            return typeof(HttpPostedFileBase).IsAssignableFrom(FieldGenerator.Metadata.ModelType);
        }

        public override IHtmlString GenerateFieldHtml()
        {
            return GetInputHtml(TextInputType.File, FieldGenerator, FieldConfiguration);
        }
    }
}
