using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.FieldGenerators.Handlers
{
    /// <summary>
    /// A Field Generator Handler is responsible for generating the HTML for a Field Element of a particular type of field.
    /// </summary>
    /// <typeparam name="TModel">The type of the model the form is being output for</typeparam>
    /// <typeparam name="T">The type of the property in the model that the specific field is being output for</typeparam>
    // ReSharper disable UnusedTypeParameter
    public interface IFieldGeneratorHandler<TModel, T>
        // ReSharper enable UnusedTypeParameter
    {
        /// <summary>
        /// Whether or not the current field can be output using this field generator handler.
        /// </summary>
        bool CanHandle();

        /// <summary>
        /// Generate the HTML for the current field's Field Element using this handler.
        /// </summary>
        /// <param name="fieldConfiguration">The field configuration to use to generate the HTML</param>
        /// <returns>The HTML for the Field Element</returns>
        IHtmlContent GenerateFieldHtml(IReadonlyFieldConfiguration fieldConfiguration);

        /// <summary>
        /// Modify the field configuration for the field using this field generator handler.
        /// </summary>
        /// <param name="fieldConfiguration">The field configuration to modify</param>
        void PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration);

        /// <summary>
        /// The type of control the field will be displayed as.
        /// </summary>
        /// <param name="fieldConfiguration">The configuration for the field</param>
        /// <returns>The display type of the field control</returns>
        FieldDisplayType GetDisplayType(IReadonlyFieldConfiguration fieldConfiguration);
    }
}