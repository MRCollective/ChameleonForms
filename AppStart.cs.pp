using ChameleonForms.Attributes;
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
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredFlagsEnumAttribute), typeof(RequiredAttributeAdapter));
            GetType().Assembly.GetTypes().Where(t => t.IsEnum && t.GetCustomAttributes(typeof(FlagsAttribute), false).Any())
                .ToList().ForEach(t =>
                {
                    System.Web.Mvc.ModelBinders.Binders.Add(t, new FlagsEnumModelBinder());
                    System.Web.Mvc.ModelBinders.Binders.Add(typeof(Nullable<>).MakeGenericType(t), new FlagsEnumModelBinder());
                });
        }
    }
}
