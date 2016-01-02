# Automatically sentence case form labels

By default MVC will use the property name that you specify in your model as the label when using

    @Html.LabelFor(m => m.MyField)

This is great when you have property names like `Email` and `Name`, but doesn't work so great when you have multiple word property names like `EmailAddress` and `FirstName`.

Of course, MVC provides the ability to override the label for each model using attributes, e.g.:

    [DisplayName("Email address")]
    public string EmailAddress { get; set; }
    
    [DisplayName("First name")]
    public string FirstName { get; set; }

If you have a convention across all of your forms to use sentence case, like the above example, then situations where the camel casing of your property names can be transformed to your desired label text can be utilised to output label names by convention without having to specify redundant `DisplayName` attributes every where.

ChameleonForms provides a class that can do this for you and all you need to do is include a single line in your `Application_Start` method in `global.asax.cs`:

    HumanizedLabels.Register();

This just requires that you have a using statement for the `ChameleonForms` namespace.

If you want a different convention (e.g. Title Case) then simply pass in one of the four LetterCasing enumeration values:

    HumanizedLabels.Register(LetterCasing.AllCaps) => "EMAIL ADDRESS"
    HumanizedLabels.Register(LetterCasing.LowerCase) => "email address"
    HumanizedLabels.Register(LetterCasing.Sentence) => "Email address"
    HumanizedLabels.Register(LetterCasing.Title) => "Email Address"

If you want more flexibility to change the label of properties where the text can't be done by convention (e.g. `LicenseAgreementAcceptance` becoming `I accept the license agreement`) then you can still use the `[DisplayName]` attribute on those fields and that will be used in preference.