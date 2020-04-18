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
    // todo: needed for beta release in priority order
    // Collapse Template dll into single dll with no dependencies? https://www.phillipsj.net/posts/using-ilrepack-with-dotnet-core-sdk-and-dotnet-standard/, https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-core-3-0#assembly-linking, https://github.com/gluck/il-repack
    // Add int/datetime client side validation support. Context: https://github.com/jbogard/aspnetwebstack/blob/730c683da2458430d36e3e360aba68932ba69fa4/src/System.Web.Mvc/ClientDataTypeModelValidatorProvider.cs, https://github.com/aspnet/Mvc/pull/2950, https://github.com/aspnet/Mvc/pull/2812, https://github.com/aspnet/Mvc/issues/4005, https://github.com/jquery-validation/jquery-validation/issues/626
    // doco for email and url fields, test Uri binding
    // content files in nuget - testing required (https://github.com/NuGet/Home/issues/6743#issuecomment-378827727), documentation required
    // web config transform view namespace additions equivalent in new world
    // Add ability to switch unobtrusive validation on/off and html5 validation on/off (<form novalidate="novalidate">) - global default with per-form override? reference ValidationHtmlAttributeProvider in documentation
    // Twitter bootstrap: template, service collection extension, nuspec, contentfiles, doco
    // Update all dependencies to latest versions
    // Review the datetime "g" and current culture things - remove?
    // Test adding both packages to a fresh web project
    // Add public api surface area approval test, compare to old library to identify gaps, update breakingchanges file
    // End-to-end documentation review and update
    // todo: 4.0 non-beta release
    // Generate nuget from .csproj rather than nuspec like https://github.com/LazZiya/ExpressLocalization/blob/master/LazZiya.ExpressLocalization/LazZiya.ExpressLocalization.csproj
    // Add support for non jquery unobtrusive validation
    // Add bootstrap4
    // Add tag helper equivalent to the raw mvc comparison example
    // Add tag helpers for chameleon forms

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
