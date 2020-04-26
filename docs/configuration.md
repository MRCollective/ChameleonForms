# Configuration

There are a number of different configuration options to tweak the out-of-the-box functionality in ChameleonForms.

## `AddServices` overloads

ChameleonForms [gets registered](getting-started.md) in `Startup.cs` within the `AddServices` method, e.g.:

```cs
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddChameleonForms();
    }
```

There are actually 3 different overloads of the `AddChameleonForms` method that you can use:

```cs
        /// <summary>
        /// Adds ChameleonForms configuration with the <see cref="DefaultFormTemplate"/>.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configModifier">Lambda expression to alter configuration</param>
        public static void AddChameleonForms(this IServiceCollection services,
            Func<ChameleonFormsConfigBuilder<DefaultFormTemplate>, ChameleonFormsConfigBuilder<DefaultFormTemplate>> configModifier = null
        ) {...}

        /// <summary>
        /// Adds ChameleonForms configuration with a specified form template and a builder modification delegate.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configModifier">Lambda expression to alter configuration</param>
        public static void AddChameleonForms<TFormTemplate>(this IServiceCollection services,
            Func<ChameleonFormsConfigBuilder<TFormTemplate>, ChameleonFormsConfigBuilder<TFormTemplate>> configModifier = null
        )
            where TFormTemplate : class, IFormTemplate {...}

        /// <summary>
        /// Adds ChameleonForms configuration with a specified form template and a builder instance.
        /// </summary>
        /// <typeparam name="TFormTemplate">The form template type to register as the default template</typeparam>
        /// <param name="services">The service collection</param>
        /// <param name="configBuilder">The configuration builder to use to specify the Chameleon Forms configuration</param>
        public static void AddChameleonForms<TFormTemplate>(this IServiceCollection services,
            ChameleonFormsConfigBuilder<TFormTemplate> configBuilder
        )
            where TFormTemplate : class, IFormTemplate {...}
```

The first one (which is used in the above example) adds ChameleonForms with the `DefaultFormTemplate` as the default template and has an optional property to allow you to tweak the global configuration of ChameleonForms using the configuration builder class. For example that might look like:

```cs
services.AddChameleonForms(b => b.WithoutHumanizedLabels());
```

The other overloads of `AddChameleonForms` allow you to specify the form template type, which allows you to specify a different default template to use. For example that might look like one of these:

```cs
// Just specify the default template type
services.AddChameleonForms<TwitterBootstrap3FormTemplate>();

// 
services.AddChameleonForms<TwitterBootstrap3FormTemplate>(b => b.WithoutHumanizedLabels());

// 
var configBuilder = new ChameleonFormsConfigBuilder<TwitterBootstrap3FormTemplate>();
// Do stuff with configBuilder
services.AddChameleonForms<TwitterBootstrap3FormTemplate>(configBuilder);
```

For more information on form templates see:

* [Using different form templates](form-templates.md)
* [Creating custom form templates](custom-template.md)

## Default global config

By default, the global config will set up the following for you:

* **Humanized labels**: The label text for fields will automatically be "[humanized](https://github.com/Humanizr/Humanizer)" from the property name using [sentence case](https://github.com/Humanizr/Humanizer#transform-string) (e.g. `public string FirstName { get; set; }` will automatically have a label of `First name`)
    * If any of the following have been applied to a field then the humanization will be skipped: `[DisplayName(Name = "Label text)]`, `[Display(Name = "Label text")]` or you have an `IDisplayMetadataProvider` registered that either sets `context.DisplayMetadata.SimpleDisplayProperty` to a non-empty/non-null string or sets `context.DisplayMetadata.DisplayName` to a lambda that returns a non-empty/non-null string. For examples see the [relevant test](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Tests/HumanizedLabelsTests.cs).
* **Default form template type**: The given `IFormTemplate` type will be registered as a Singleton with the service collection and will be resolved by default when creating a ChameleonForm.
* **Flags enum support**: Any non-nullable `[Flags]` enums will automatically be validated to be `[Required]`. The default behaviour is MVC is that their `ModelMetadata` is marked `IsRequired`, but they aren't actually validated as required; ChameleonForms patches that by default.

## Configuration builder

The configuration builder allows you to tweak the default global config using the following fluent methods:

```cs
        /// <summary>
        /// Turn off humanized labels.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutHumanizedLabels();

        /// <summary>
        /// Humanize labels with the given transformer. Use <see cref="To"/> to access the default Humanizer ones.
        /// </summary>
        /// <example>
        /// builder.WithHumanizedLabelTransformer(To.TitleCase)
        /// </example>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithHumanizedLabelTransformer(IStringTransformer transformer);

        /// <summary>
        /// Don't register the template type with the <see cref="ServiceCollection"/>.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutTemplateTypeRegistration();

        /// <summary>
        /// Turn off model binding of flag enums.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutFlagsEnumBinding();

        /// <summary>
        /// Turn off validation of implicit [Required] on non-nullable flag enums.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutFlagsEnumRequiredValidation();

        /// <summary>
        /// Turn off model binding of <see cref="System.DateTime"/>s.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutDateTimeBinding();

        /// <summary>
        /// Turn off model binding of enum lists.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutEnumListBinding();

        /// <summary>
        /// Turn off model binding of <see cref="System.Uri"/>s.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutUriBinding();

        /// <summary>
        /// Turn off client model validation of integral numerics.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutIntegralClientModelValidation();

        /// <summary>
        /// Turn off client model validation of <see cref="System.DateTime"/>s.
        /// </summary>
        /// <returns>The builder to allow fluent method chaining</returns>
        public ChameleonFormsConfigBuilder<TFormTemplate> WithoutDateTimeClientModelValidation();
```


## MSBuild configuration

## Advanced usage configuration
* [Using different form templates](form-templates.md)
* [Creating custom form templates](custom-template.md)
* [Extending the field configuration](extending-field-configuration.md)
* [Extending the form components](extending-form-components.md)
* [Creating and using a custom field generator](custom-field-generator.md)
* [Creating and using custom field generator handlers](custom-field-generator-handlers.md)
