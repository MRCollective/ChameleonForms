using ChameleonForms.Generated.Default;
using ChameleonForms.Templates;
using RazorLight;

namespace ChameleonForms.Generated
{
    public class ChameleonFormsTemplate
    {
        public ChameleonFormsTemplate()
        {
            var engine = new RazorLightEngineBuilder()
                .UseMemoryCachingProvider()
                .UseEmbeddedResourcesProject(GetType())
                .Build();

            var x = GetType().Assembly.GetManifestResourceNames();
            var y = GetType().Namespace;

            DefaultFormTemplate = new DefaultTemplate(engine);
        }

        public IFormTemplate DefaultFormTemplate { get; }
    }
}
