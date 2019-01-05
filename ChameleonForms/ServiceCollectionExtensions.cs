using ChameleonForms.Attributes;
using ChameleonForms.Metadata;
using ChameleonForms.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChameleonForms
{
    public static class ServiceCollectionExtensions
    {
        public static void AddChameleonForms(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }

            services.Configure<MvcOptions>(x =>
            {
                x.ModelMetadataDetailsProviders.Add(new HumanizedLabelsDisplayMetadataProvider());
                x.ModelMetadataDetailsProviders.Add(new ModelMetadataAwareDisplayMetadataProvider());

                x.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                x.ModelBinderProviders.Insert(0, new FlagsEnumModelBinderProvider());
            });

            services.AddSingleton<IValidationAttributeAdapterProvider, RequiredFlagsEnumAttributeAdapterProvider>();
        }
    }
}
