# Controlling labels

By default ASP.NET Core MVC will use the property name that you specify in your model as the label when using

    @Html.LabelFor(m => m.MyField)
    <label asp-for="MyField"></label>

This is great when you have property names like `Email` and `Name`, but doesn't work so great when you have multiple word property names like `EmailAddress` and `FirstName`.

Of course, MVC provides the ability to override the label for each model using attributes, e.g.:

```cs
    [DisplayName("Email address")]
    public string EmailAddress { get; set; }
    
    [Display(Name = "First name")]
    public string FirstName { get; set; }
```

If you have a convention across all of your forms to use sentence case, like the above example, then it makes sense to automatically translate the camel casing of your property names to your desired label text by convention without having to specify redundant `DisplayName` attributes everywhere (adding noise and maintenance overhead).

ChameleonForms provides this functionality to you [by default](configuration.md#default-global-config).

By default your label names will be "Sentence cased" (similar to the above example of EmailAddress and FirstName). You can configure different options for label name transformation using:

```cs
services.AddChameleonForms(b => b.WithHumanizedLabelTransformer(Transform.UpperCase));
// or ...
services.AddChameleonForms(b => b.WithHumanizedLabelTransformer(Transform.LowerCase));
// or ...
services.AddChameleonForms(b => b.WithHumanizedLabelTransformer(Transform.TitleCase));
// or ...
services.AddChameleonForms(b => b.WithHumanizedLabelTransformer(/* Custom class that inherits from Humanizer.IStringTransformer */));
```

You can also disable it using:

```cs
services.AddChameleonForms(b => b.WithoutHumanizedLabels());
```

If you want more flexibility to change the label of properties where the text can't be done by convention (e.g. `LicenseAgreementAcceptance` becoming `I accept the license agreement`) then you can still use the `[DisplayName]` or `[Display]` attribute on those properties and that will always be used in preference.
