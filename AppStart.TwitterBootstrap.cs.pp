using ChameleonForms;
using ChameleonForms.Templates.TwitterBootstrap3;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.ConfigureTwitterBootstrap), "Start")]
 
namespace $rootnamespace$.App_Start
{
    public static class ConfigureTwitterBootstrap
    {
        public static void Start()
        {
            FormTemplate.Default = new TwitterBootstrapFormTemplate();
        }
    }
}
