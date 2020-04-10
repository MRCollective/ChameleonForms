using ChameleonForms.Attributes;
using ChameleonForms.Metadata;
using ChameleonForms.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChameleonForms
{
    public static class ServiceCollectionExtensions
    {
        public static void AddChameleonForms(this IServiceCollection services, bool humanizedLabels = true)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }

            services.Configure<MvcOptions>(x =>
            {
                if (humanizedLabels)
                    x.ModelMetadataDetailsProviders.Add(new HumanizedLabelsDisplayMetadataProvider());
                x.ModelMetadataDetailsProviders.Add(new ModelMetadataAwareDisplayMetadataProvider());

                x.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                x.ModelBinderProviders.Insert(0, new FlagsEnumModelBinderProvider());
            });

            services.AddSingleton<IValidationAttributeAdapterProvider, RequiredFlagsEnumAttributeAdapterProvider>();
        }
    }
}
