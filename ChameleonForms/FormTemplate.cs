using ChameleonForms.Templates;

namespace ChameleonForms
{
    public static class FormTemplate
    {
        static FormTemplate()
        {
            Default = new DefaultFormTemplate();
        }

        public static IFormTemplate Default { get; set; }
    }
}
