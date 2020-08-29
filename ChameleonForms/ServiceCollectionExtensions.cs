﻿using ChameleonForms.Attributes;
using ChameleonForms.Metadata;
using ChameleonForms.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using ChameleonForms.Config;
using ChameleonForms.Templates;
using ChameleonForms.Templates.Default;
using ChameleonForms.Validators;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ChameleonForms
{
    // todo: 4.0 non-beta release
    // Bootstrap4: form-control-lg/sm, inline forms, form grids, horizontal forms
    // Build-in DI support e.g. fieldgeneratorrouter
    // Review the datetime "g" and current culture things - remove? client side validation for non / separators?
    // Update all dependencies to latest versions
    // Tidy up cshtml files
    // Add ability to switch unobtrusive validation on/off and html5 validation on/off (<form novalidate="novalidate">) - global default with per-form override? reference ValidationHtmlAttributeProvider in documentation
    // blog posts: razorgenerator, mvctestcontext, modelbindertestbase, ilmerge, client validation in aspnetcore, end-to-end test in-memory, disposablehtmlhelper, testing metadatadetailsprovider, docfx, tag helpers including nested children
    // Generate nuget from .csproj rather than nuspec like https://github.com/LazZiya/ExpressLocalization/blob/master/LazZiya.ExpressLocalization/LazZiya.ExpressLocalization.csproj
    // [Range] client validation support
    // ExistsInAttribute.EnableValidation <- DI rather than static
    // support IList<nullable<value type>> similar to IList<nullable enum type> support
    // Support Blazor, razor pages and carter
    // Remove ExistsInAttribute.EnableValidation as a static
    // Recreate FormMethod enum

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds ChameleonForms configuration with the <see cref="DefaultFormTemplate"/>.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configModifier">Lambda expression to alter configuration</param>
        public static void AddChameleonForms(this IServiceCollection services,
            Func<ChameleonFormsConfigBuilder<DefaultFormTemplate>, ChameleonFormsConfigBuilder<DefaultFormTemplate>> configModifier = null
        )
        {
            if (configModifier == null)
                configModifier = c => c;
            services.AddChameleonForms<DefaultFormTemplate>(configModifier(new ChameleonFormsConfigBuilder<DefaultFormTemplate>()));
        }

        /// <summary>
        /// Adds ChameleonForms configuration with a specified form template and a builder modification delegate.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configModifier">Lambda expression to alter configuration</param>
        public static void AddChameleonForms<TFormTemplate>(this IServiceCollection services,
            Func<ChameleonFormsConfigBuilder<TFormTemplate>, ChameleonFormsConfigBuilder<TFormTemplate>> configModifier = null
        )
            where TFormTemplate : class, IFormTemplate
        {
            if (configModifier == null)
                configModifier = c => c;
            services.AddChameleonForms<TFormTemplate>(configModifier(new ChameleonFormsConfigBuilder<TFormTemplate>()));
        }

        /// <summary>
        /// Adds ChameleonForms configuration with a specified form template and a builder instance.
        /// </summary>
        /// <typeparam name="TFormTemplate">The form template type to register as the default template</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="configBuilder">The configuration builder to use to specify the Chameleon Forms configuration</param>
        public static void AddChameleonForms<TFormTemplate>(this IServiceCollection services,
            ChameleonFormsConfigBuilder<TFormTemplate> configBuilder
        )
            where TFormTemplate : class, IFormTemplate
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var config = configBuilder.Build();

            services.Configure<MvcOptions>(x =>
            {
                if (config.HumanizeLabels)
                    x.ModelMetadataDetailsProviders.Add(new HumanizedLabelsDisplayMetadataProvider(config.HumanizedLabelsTransformer));
                
                x.ModelMetadataDetailsProviders.Add(new ModelMetadataAwareDisplayMetadataProvider<ExistsInAttribute>());

                if (config.RegisterDateTimeBinding)
                    x.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                if (config.RegisterFlagsEnumBinding)
                    x.ModelBinderProviders.Insert(0, new FlagsEnumModelBinderProvider());
                if (config.RegisterEnumListBinding)
                    x.ModelBinderProviders.Insert(0, new EnumListModelBinderProvider());
                if (config.RegisterUriBinding)
                    x.ModelBinderProviders.Insert(0, new UriModelBinderProvider());
            });

            services.Configure<MvcViewOptions>(x =>
            {
                if (config.RegisterIntegralClientModelValidator)
                    x.ClientModelValidatorProviders.Add(new IntegralNumericClientModelValidatorProvider());
                if (config.RegisterDateTimeClientModelValidator)
                    x.ClientModelValidatorProviders.Add(new DateTimeClientModelValidatorProvider());
            });

            services.Configure<HtmlHelperOptions>(o =>
            {
                o.ClientValidationEnabled = true;
            });

            if (config.RegisterTemplateType)
                services.AddSingleton<IFormTemplate, TFormTemplate>();
        }
    }
}
