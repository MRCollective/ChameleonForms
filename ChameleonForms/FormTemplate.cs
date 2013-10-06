using ChameleonForms.Templates;

namespace ChameleonForms
{
    /// <summary>
    /// Holds an instance to the default form template that will be used to render forms.
    /// </summary>
    public static class FormTemplate
    {
        static FormTemplate()
        {
            Default = new DefaultFormTemplate();
        }

        /// <summary>
        /// The default form template instance to render forms.
        /// </summary>
        public static IFormTemplate Default { get; set; }
    }
}
