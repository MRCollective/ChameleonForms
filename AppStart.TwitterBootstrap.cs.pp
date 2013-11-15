using ChameleonForms;
using ChameleonForms.Templates.TwitterBootstrap3;
using System.Web.Optimization;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.ConfigureTwitterBootstrap), "Start")]
 
namespace $rootnamespace$.App_Start
{
    public static class ConfigureChameleonTwitterBootstrap
    {
        public static void Start()
        {
            FormTemplate.Default = new TwitterBootstrapFormTemplate();
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/chameleon-bootstrapjs").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.validate.unobtrusive.twitterbootstrap.js"
            ));
            BundleTable.Bundles.Add(new StyleBundle("~/bundles/chameleon-bootstrapcss").Include(
                "~/Content/bootstrap.css",
                "~/Content/chameleonforms-twitterbootstrap.css"
            ));
        }
    }
}
