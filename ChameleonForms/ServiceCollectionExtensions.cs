using ChameleonForms.Attributes;
using ChameleonForms.Metadata;
using ChameleonForms.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using ChameleonForms.Templates;
using ChameleonForms.Templates.Default;
using ChameleonForms.Validators;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ChameleonForms
{
    // todo: needed for beta release in priority order
    // switch datetimemodelbindershould and flagsenummodelbindershould to modelbindertestbase
    // doco: int/datetime client validation support, email and url fields, client side file copying, how to set up twitter bootstrap 3, view namespace getting started
    // test Uri binding works?
    // Add ability to switch unobtrusive validation on/off and html5 validation on/off (<form novalidate="novalidate">) - global default with per-form override? reference ValidationHtmlAttributeProvider in documentation
    // Twitter bootstrap: template, service collection extension, doco, remove WebViewPage
    // Update all dependencies to latest versions
    // Review the datetime "g" and current culture things - remove? client side validation for non / separators?
    // Test adding package to a fresh web project
    // Add public api surface area approval test, compare to old library to identify gaps, update breakingchanges file (include textarea cols and rows)
    // End-to-end documentation review and update - @helper, IHtmlString, web.config, update Message, Navigation etc.
    // todo: 4.0 non-beta release
    // blog posts: razorgenerator, mvctestcontext, modelbindertestbase, ilmerge, client validation in aspnetcore, end-to-end test in-memory, disposablehtmlhelper
    // Generate nuget from .csproj rather than nuspec like https://github.com/LazZiya/ExpressLocalization/blob/master/LazZiya.ExpressLocalization/LazZiya.ExpressLocalization.csproj
    // [Range] client validation support
    // Add support for non jquery unobtrusive validation
    // Add bootstrap4
    // Add tag helper equivalent to the raw mvc comparison example
    // Add tag helpers for chameleon forms
    // ExistsInAttribute.EnableValidation <- DI rather than static
    // support IList<nullable<value type>> similar to IList<nullable enum type> support
    // Support Blazor, razor pages and carter

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
        /// <param name="registerIntegralClientModelValidator">Whether or not to register a client model validator for integral numeric types</param>
        /// <param name="registerDateTimeClientModelValidator">Whether or not to register a client model validator for DateTime types</param>
        public static void AddChameleonForms(this IServiceCollection services,
            bool humanizeLabels = true,
            bool registerDefaultTemplate = true,
            bool registerFlagsEnumBinding = true,
            bool registerFlagsEnumRequiredValidation = true,
            bool registerDateTimeBinding = true,
            bool registerEnumListBinding = true,
            bool registerIntegralClientModelValidator = true,
            bool registerDateTimeClientModelValidator = true
        )
        {
            services.AddChameleonForms<DefaultFormTemplate>(
                humanizeLabels: humanizeLabels,
                registerDefaultTemplate: registerDefaultTemplate,
                registerFlagsEnumBinding: registerFlagsEnumBinding,
                registerFlagsEnumRequiredValidation: registerFlagsEnumRequiredValidation,
                registerDateTimeBinding: registerDateTimeBinding,
                registerEnumListBinding: registerEnumListBinding,
                registerIntegralClientModelValidator: registerIntegralClientModelValidator,
                registerDateTimeClientModelValidator: registerDateTimeClientModelValidator
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
        /// <param name="registerIntegralClientModelValidator">Whether or not to register a client model validator for integral numeric types</param>
        /// <param name="registerDateTimeClientModelValidator">Whether or not to register a client model validator for DateTime types</param>
        public static void AddChameleonForms<TFormTemplate>(this IServiceCollection services,
            bool humanizeLabels = true,
            bool registerDefaultTemplate = true,
            bool registerFlagsEnumBinding = true,
            bool registerFlagsEnumRequiredValidation = true,
            bool registerDateTimeBinding = true,
            bool registerEnumListBinding = true,
            bool registerIntegralClientModelValidator = true,
            bool registerDateTimeClientModelValidator = true
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

            services.Configure<MvcViewOptions>(x =>
            {
                if (registerIntegralClientModelValidator)
                    x.ClientModelValidatorProviders.Add(new IntegralNumericClientModelValidatorProvider());
                if (registerDateTimeClientModelValidator)
                    x.ClientModelValidatorProviders.Add(new DateTimeClientModelValidatorProvider());
            });

            services.Configure<HtmlHelperOptions>(o =>
            {
                o.ClientValidationEnabled = true;
            });

            if (registerFlagsEnumRequiredValidation)
                services.AddSingleton<IValidationAttributeAdapterProvider, RequiredFlagsEnumAttributeAdapterProvider>();
            if (registerDefaultTemplate)
                services.AddSingleton<IFormTemplate, TFormTemplate>();
        }
    }
}
