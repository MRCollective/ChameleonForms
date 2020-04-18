using ChameleonForms.Attributes;
using ChameleonForms.Metadata;
using ChameleonForms.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using ChameleonForms.Templates;
using ChameleonForms.Templates.Default;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ChameleonForms
{
    // todo: Add int/datetime client side validation support. Context: https://github.com/jbogard/aspnetwebstack/blob/730c683da2458430d36e3e360aba68932ba69fa4/src/System.Web.Mvc/ClientDataTypeModelValidatorProvider.cs, https://github.com/aspnet/Mvc/pull/2950, https://github.com/aspnet/Mvc/pull/2812, https://github.com/aspnet/Mvc/issues/4005, https://github.com/jquery-validation/jquery-validation/issues/626
    // content files in nuget
    // todo: Add support for non jquery unobtrusive validation
    // Add this extension equivalent for twitter bootstrap
    // Add bootstrap4
    // Add source files for unobtrusive validation - add to CDN instead?
    // documentation update
    // Add public api approval test
    // review the datetime "g" thing
    // Add tag helper equivalent to the comparison example
    // Update all dependencies to latest versions
    // Update breaking changes
    // Add ability to switch unobtrusive validation on/off and html5 validation on/off (<form novalidate="novalidate">) - global default with per-form override? ValidationHtmlAttributeProvider
    // doco for email and url fields, test Uri
    // Generate nuget from .csproj rather than nuspec

    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds ChameleonForms configuration with the <see cref="DefaultFormTemplate"/>.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="humanizeLabels">Whether or not to automatically humanize labels</param>
        /// <param name="registerDefaultTemplate">Whether or not to register a default <see cref="IFormTemplate"/></param>
        /// <param name="registerFlagsEnumBinding">Whether or not to register flag enum model binding; true by default</param>
        /// <param name="registerFlagsEnumRequiredValidation">Whether or not to register flag enum [Required] validation; true by default</param>
        /// <param name="registerDateTimeBinding">Whether or not to register format aware date time model binding; true by default</param>
        /// <param name="registerEnumListBinding">Whether or not to register enum list model binding; true by default</param>
        public static void AddChameleonForms(this IServiceCollection services,
            bool humanizeLabels = true,
            bool registerDefaultTemplate = true,
            bool registerFlagsEnumBinding = true,
            bool registerFlagsEnumRequiredValidation = true,
            bool registerDateTimeBinding = true,
            bool registerEnumListBinding = true
        )
        {
            services.AddChameleonForms<DefaultFormTemplate>(
                humanizeLabels: humanizeLabels,
                registerDefaultTemplate: registerDefaultTemplate,
                registerFlagsEnumBinding: registerFlagsEnumBinding,
                registerFlagsEnumRequiredValidation: registerFlagsEnumRequiredValidation,
                registerDateTimeBinding: registerDateTimeBinding,
                registerEnumListBinding: registerEnumListBinding
            );
        }

        /// <summary>
        /// Adds ChameleonForms configuration with the <see cref="DefaultFormTemplate"/>.
        /// </summary>
        /// <typeparam name="TFormTemplate">The form template type to register as the default template</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="humanizeLabels">Whether or not to automatically humanize labels; true by default</param>
        /// <param name="registerDefaultTemplate">Whether or not to register a default <see cref="IFormTemplate"/></param>
        /// <param name="registerFlagsEnumBinding">Whether or not to register flag enum model binding; true by default</param>
        /// <param name="registerFlagsEnumRequiredValidation">Whether or not to register flag enum [Required] validation; true by default</param>
        /// <param name="registerDateTimeBinding">Whether or not to register format aware date time model binding; true by default</param>
        /// <param name="registerEnumListBinding">Whether or not to register enum list model binding; true by default</param>
        public static void AddChameleonForms<TFormTemplate>(this IServiceCollection services,
            bool humanizeLabels = true,
            bool registerDefaultTemplate = true,
            bool registerFlagsEnumBinding = true,
            bool registerFlagsEnumRequiredValidation = true,
            bool registerDateTimeBinding = true,
            bool registerEnumListBinding = true
        )
            where TFormTemplate : class, IFormTemplate
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.Configure<MvcOptions>(x =>
            {
                if (humanizeLabels)
                    x.ModelMetadataDetailsProviders.Add(new HumanizedLabelsDisplayMetadataProvider());
                
                x.ModelMetadataDetailsProviders.Add(new ModelMetadataAwareDisplayMetadataProvider<ExistsInAttribute>());

                if (registerDateTimeBinding)
                    x.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                if (registerFlagsEnumBinding)
                    x.ModelBinderProviders.Insert(0, new FlagsEnumModelBinderProvider());
                if (registerEnumListBinding)
                    x.ModelBinderProviders.Insert(0, new EnumListModelBinderProvider());
            });

            services.Configure<HtmlHelperOptions>(o => o.ClientValidationEnabled = false);

            if (registerFlagsEnumRequiredValidation)
                services.AddSingleton<IValidationAttributeAdapterProvider, RequiredFlagsEnumAttributeAdapterProvider>();
            if (registerDefaultTemplate)
                services.AddSingleton<IFormTemplate, TFormTemplate>();
        }
    }
}
