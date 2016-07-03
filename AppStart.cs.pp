using ChameleonForms.Templates;
using ChameleonForms.Templates.Default;
using ChameleonForms.ModelBinders;
using System;
using System.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.RegisterChameleonFormsComponents), "Start")]
 
namespace $rootnamespace$.App_Start
{
    public static class RegisterChameleonFormsComponents
    {
        public static void Start()
        {
            FormTemplate.Default = new DefaultFormTemplate();
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
        }
    }
}
