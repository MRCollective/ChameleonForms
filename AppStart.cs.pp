using ChameleonForms.Attributes;
using ChameleonForms.Templates;
using ChameleonForms.Templates.Default;
using ChameleonForms.ModelBinders;
using System;
using System.Linq;
using System.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.RegisterChameleonFormsComponents), "Start")]
 
namespace $rootnamespace$
{
    public static class RegisterChameleonFormsComponents
    {
        public static void Start()
        {
            FormTemplate.Default = new DefaultFormTemplate();
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredFlagsEnumAttribute), typeof(RequiredAttributeAdapter));
            typeof(RegisterChameleonFormsComponents).Assembly.GetTypes().Where(t => t.IsEnum && t.GetCustomAttributes(typeof(FlagsAttribute), false).Any())
                .ToList().ForEach(t =>
                {
                    ModelBinders.Binders.Add(t, new FlagsEnumModelBinder());
                    ModelBinders.Binders.Add(typeof(Nullable<>).MakeGenericType(t), new FlagsEnumModelBinder());
                });
        }
    }
}
