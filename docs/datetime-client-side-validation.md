# Client-side validation of DateTime fields in ASP.NET MVC using DisplayFormat

ChameleonForms provides a way for you to hook into either the [jQuery validation unobtrusive library](https://github.com/aspnet/jquery-validation-unobtrusive) that ASP.NET Core MVC ships with or the [aspnet-validation library](https://github.com/ryanelian/aspnet-validation) that provides equivalent functionality without requiring jQuery to validate that the date format the user specifies is OK on the client-side based on the format string you provided.

If you want support added for other validation libraries then please [raise an issue to discuss](https://github.com/MRCollective/ChameleonForms/issues).

## Using the validation

In order for this to work you need to:

1. Ensure the client validation provider is registered, this is [on by default](configuration.md#default-global-config)
2. Ensure you have either jQuery validation unobtrusive or aspnet-validation working
3. Reference the [unobtrusive-date-validation.chameleonforms.js](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/wwwroot/lib/chameleonforms/unobtrusive-date-validation.chameleonforms.js) file
    * By default, when you [install ChameleonForms by NuGet](getting-started.md) this file will be placed into `wwwroot/lib/chameleonforms/unobtrusive-date-validation.chameleonforms.js` for you when you build. If you don't want that to happen check out [configuration](configuration.md#msbuild-configuration).
4. Place the `[DisplayFormat(DataFormatString = "{0:%format%}", ApplyInEditMode = true)]` attribute on the model property
5. Output the field using ChameleonForms

If you aren't using ChameleonForms to output the field then replace step 2 and 3 with:

1. Include the `data-val="true"` attribute to turn on unobtrusive validation for that field
2. Include the `data-val-date="%errorMessageIfDateIsIncorrect%"` attribute to indicate the field is a date and what message should display if the user enters an invalid date
3. Include the `data-val-format="%formatString%"` attribute to indicate the format string that should be validated against

## Client validation provider

The [client validation provider](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/Validators/DateTimeClientModelValidatorProvider.cs) that ensures the validation attributes are added is added [by the default global config](configuration.md#default-global-config). If you want to register it yourself then you can:

```cs
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddChameleonForms(b => b.WithoutDateTimeClientModelValidation());
        services.Configure<MvcViewOptions>(x =>
        {
            x.ClientModelValidatorProviders.Add(new DateTimeClientModelValidatorProvider());
            ...
        });
    }
```

## Supported formats

Only the following date format strings are supported (want more added? feel free to [send a pull request](https://github.com/MRCollective/ChameleonForms/pulls)):

* `d/M/yyyy`
* `d-M-yyyy`
* `d.M.yyyy`
* `d/M/yy`
* `d-M-yy`
* `d.M.yy`
* `dd/MM/yyyy`
* `dd-MM-yyyy`
* `dd.MM.yyyy`
* `dd/MM/yy`
* `dd-MM-yy`
* `dd.MM.yy`
* `M/d/yyyy`
* `M-d-yyyy`
* `M.d.yyyy`
* `M/d/yy`
* `M-d-yy`
* `M.d.yy`
* `MM/dd/yyyy`
* `MM-dd-yyyy`
* `MM.dd.yyyy`
* `MM/dd/yy`
* `MM-dd-yy`
* `MM.dd.yy`
* `yyyy/MM/dd`
* `yyyy-MM-dd`
* `yyyy.MM.dd`
* `yyyy/M/d`
* `yyyy-M-d`
* `yyyy.M.d`

Only the following time format strings are supported:

* `h:mmtt`
* `h:mm:sstt`
* `hh:mmtt`
* `hh:mm:sstt`
* `H:mm`
* `H:mm:ss`
* `HH:mm`
* `HH:mm:ss`

You can also combine one of the supported date formats with one of the supported time formats if the date format is first, followed by whitespace and then the time format (want to support more scenarios? feel free to [send a pull request](https://github.com/MRCollective/ChameleonForms/pulls)).

To see what each of the format identifiers means, please consult the [relevant Microsoft documentation](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings).

## Want another format?

If you want support for another format string, please [lodge an issue](https://github.com/MRCollective/ChameleonForms/issues) or [send a pull request](https://github.com/MRCollective/ChameleonForms/pulls).
