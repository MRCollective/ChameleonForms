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

* **Humanized labels**: The label text for fields will automatically be "[humanized](https://github.com/Humanizr/Humanizer)" from the property name using [sentence case](https://github.com/Humanizr/Humanizer#transform-string) (e.g. `public string FirstName { get; set; }` will automatically have a label of `First name`). See [Controlling labels](labels.md) for more information.
    * If any of the following have been applied to a field then the humanization will be skipped: `[DisplayName(Name = "Label text)]`, `[Display(Name = "Label text")]` or you have an `IDisplayMetadataProvider` registered that either sets `context.DisplayMetadata.SimpleDisplayProperty` to a non-empty/non-null string or sets `context.DisplayMetadata.DisplayName` to a lambda that returns a non-empty/non-null string. For examples see the [relevant test](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Tests/HumanizedLabelsTests.cs).
* **Default form template type**: The given `IFormTemplate` type will be registered as a Singleton with the service collection and will be resolved by default when creating a ChameleonForm. See [Form Templates](form-templates.md) for more information.
* **Flags enum support**: Correctly handle model binding and server-side validation of `[Flags]` enums (including implicit `[Required]` for non-nullable, which is broken in out-of-the-box MVC). It's expected that they will be posted as multiple values and rendered as a multiple select input (`<select multiple>` or `<input type="checkbox">` list). See [Flags Enum Fields](flags-enum.md) for more information.
* **Format-aware DateTime support**: Correctly handle model binding and server-side validation of `DateTime` values with a `[DisplayFormat(DataFormatString = "{0:SOME_FORMAT}", ApplyFormatInEditMode = true)]` attribute. See [DateTime Fields](datetime.md) for more information.
* **Enum list support**: Correctly handle binding and server-side validation of enum lists (e.g. `IEnumerable<EnumType>`, `EnumType[]` etc.). This patches up a range of problems with the out-of-the-box MVC support for enum lists, including poor support for `[Required]` and erroneous binding of `null` values in the lists. See [Multiple-Select Enum Fields](multiple-enum.md) for more information.
* **Uri support**: Correctly handle model binding and server-side validation of `Uri`'s. See [Uri Fields](uri.md) for more information.
* **Integral number client validation support**: Support unobtrusive client-side validation of integral types (`byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`). This existed in ASP.NET MVC, but no longer exists in ASP.NET Core MVC because HTML5 `type="number"` has been added. This is a problem if you don't want to rely on HTML5 validation (which has a sub-par user experience in most cases).
* **Format-aware DateTime client validation support**: Support unobtrusive client-side validation of `DateTime`'s that is format string aware. See [Client-side DateTime Validation](datetime-client-side-validation.md) for more information.

## Configuration builder

The configuration builder allows you to tweak the default global config using the following self-explanatory fluent methods:

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

When you install `ChameleonForms` it will automatically include an MSBuild `.targets` file into your project. This file provides the following functionality:

* Copy ChameleonForms client-side files into `wwwroot/lib/chameleonforms/` on build unless they haven't changed.
    * If you want to disable this simply set the following property in your `.csproj` file:

    ```xml
      <PropertyGroup>
        <ChameleonFormsCopyContentFiles>false</ChameleonFormsCopyContentFiles>
      </PropertyGroup>
    ```

## Advanced configuration

* [Using different form templates](form-templates.md)
* [Creating custom form templates](custom-template.md)
* [Extending the field configuration](extending-field-configuration.md)
* [Extending the form components](extending-form-components.md)
* [Creating and using a custom field generator](custom-field-generator.md)
* [Creating and using custom field generator handlers](custom-field-generator-handlers.md)
