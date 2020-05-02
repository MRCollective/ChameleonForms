# ChameleonForms Breaking Changes

This file has all breaking changes across ChameleonForms versions.

# Version 4.0.0-beta

## .NET Framework no longer supported

.NET Framework is no longer supported. ChameleonForms now works against .NET Core 3.1 onwards.

### Reason

ChameleonForms has been upgraded to support ASP.NET Core MVC. It's much easier to just support .NET Core 3.1 rather than try and target older versions of .NET Core or .NET Framework.

### Workaround

If you want to support .NET Full Framework with MVC 5 then check out v3.0.3 of the [NuGet package](https://www.nuget.org/packages/ChameleonForms/3.0.3) and [documentation](https://chameleonforms.readthedocs.io/en/3.0.3/). If you want to use a different version of .NET Core then [raise an issue](https://github.com/MRCollective/ChameleonForms/issues) to discuss adding that support.

## DisplayFormatString no longer used

Support in ChameleonForms for detecting format strings for display of value in the form field as well as DateTime support has been modified to use `EditFormatString` rather than `DisplayFormatString`.

### Reason

Given ChameleonForms is used for edit scenarios it made sense to use the edit format string rather than the display format string.

### Workaround

Change all affected instances of `[DisplayFormat(...)]` to use the format for editing via the `ApplyFormatInEditMode = true` property on the attribute.

## Removed `[RequiredFlagsEnum]`

The `[RequiredFlagsEnum]` attribute has been removed in favour of support for using the standard `[Required]` attribute on flags enum fields with server-side validation.

### Reason

It's much more intuitive and discoverable to use `[Required]` than the strange `[RequiredFlagsEnum]` deviation. Upgrading to .NET Core made it possible to implement this support.

### Workaround

Find all instances of `[RequiredFlagsEnum]` and replace with `[Required]`.

## Removed `FormTemplate.Default`

Overriding the default form template is no longer done using `FormTemplate.Default`, which has been removed.

### Reason

It's much nicer to move away from non-threadsafe statics and instead use dependency injection. The migration to .NET Core allowed this to be easily implemented and is a more idiomatic way of configuring the library.

### Workaround

Change any call to `FormTemplate.Default` with an appropriate [ChameleonForms configuration call](https://chameleonforms.readthedocs.io/en/latest/configuration/).

## ChameleonForms requires registration

ChameleonForms now needs to be registered to work, rather than automatic registration via WebActivator.

### Reason

The idiomatic way to register libraries in .NET Core is via the service collection. This also gives much more explicit control over configuration.

### Workaround

Remove any existing `AppStart.cs` or `AppStart.TwitterBootstrap.cs` files added by ChameleonForms and instead call `services.AddChameleonForms()` in your `Startup.cs` [or one of the variants of that call](https://chameleonforms.readthedocs.io/en/latest/configuration/). See also [Getting Started](https://chameleonforms.readthedocs.io/en/latest/getting-started/)

## Client-side CSS and JavaScript no longer automatically registered

Client-side CSS and JavaScript files are no longer automatically registered with a bundle.

### Reason

.NET Core doesn't use bundles anymore.

### Workaround

The client-side files are copied into `wwwroot/libs/chameleonforms` - simply reference the CSS and/or JavaScript files you want to use within the relevant parts of your site e.g. `_Layout.cshtml`, `_ValidationScriptsPartial.cshtml`, etc.

## `IHtmlString` -> `IHtmlContent`

All methods in ChameleonForms that returned or took `IHtmlString` parameters, and all classes that extended `IHtmlString` now return/take/extend `IHtmlContent`.

### Reason

`IHtmlContent` is the .NET Core equivalent of `IHtmlString`.

### Workaround

Change any of your classes / methods that rely on `IHtmlString` to instead use `IHtmlContent`.

## Twitter Bootstrap template

The `TwitterBootstrapFormTemplate` has been renamed to `TwitterBootstrap3FormTemplate` and no longer has a seprate NuGet package.

### Reason

Bootstrap has a version 4 and soon a version 5 now so it made sense to be explicit about the version number. The separate NuGet package mostly was about adding in CSS / JavaScript files, which is taken care of differently now (see above).

### Workaround

Rename any usage of `TwitterBootstrapFormTemplate` to `TwitterBootstrap3FormTemplate`. Manually add in references to relevant CSS and JS files. See [Twitter Bootstrap Template](https://chameleonforms.readthedocs.io/en/latest/bootstrap-template/) for more details.

## HTML5 validation turned off

The `DeFaultFormTemplate` and `TwitterBootstrapFormTemplate` now add `novalidate="novalidate"` to the `<form>` element outputted from `Html.BeginChameleonForm`.

### Reason

We now make use of HTML5 attributes like `required`, `type="number"`, etc. and the user experience for HTML5 validation is poor compared to things like unobtrusive validation. Beause of this we turn off HTML5 validation by default.

### Workaround

If you want to have HTML5 validation you can create your own form template that doesn't add the `novalidate` attribute. We plan on adding configurability to the specification of the client-side validation in the future, so if you are in this situation feel free to [raise an issue](https://github.com/MRCollective/ChameleonForms/issues) to discuss adding that support.

# Version 3.0.0

## Flags enum support

Enums marked with the `[Flags]` attribute will now show as multiple select elements (or checkboxes when displaying as a list).

### Reason

Support has been added for flags enums; these make sense as multiple-select controls since a flags enum can support multiple values.

### Workaround

Create a enum class without the `[Flags]` attribute with the same values and bind to that instead.

# Version 2.0.0

## Deprecated `WithoutLabel`

Deprecated `WithoutLabel` method on `IFieldConfiguration`. It still works (for now), but the method has been marked with the `[Obsolete]` attribute.

### Reason

The method has been renamed to `WithoutLabelElement` since it more closely reflects what the method does.

### Workaround

Change all instances of `WithoutLabel` to `WithoutLabelElement`.

## Removed `ReadOnlyConfiguration` class

Remove the `ReadOnlyConfiguration` public class. 

### Reason

The class was redundant, because it just wrapped calls to `FieldConfiguration` instance. Implementing `IReadOnlyConfiguration` in `FieldCofiguration` class has more sense

### Workaround

The `ReadonlyConfiguration` class expected not to be widely using. In case of using remove reference to he class and/or move possible implementation to custom wrapper around `FieldConfiguration`

## Removed `TTemplate` type parameter from form component classes

Removed the `TTemplate` type parameter from all of the form component classes. This will only affect you if you created custom form components e.g. you extended `Component`, `Form`, `Section`, `Naviation`, etc.

### Reason

It made extensibility harder to have to pass that type around everywhere and it wasn't really being used. Using polymorphism with a property of type `IFormTemplate` made more sense.

### Workaround

You need to change any custom components to simply remove the template type. If you custom component relied on that template type for strong typing then you can follow the example of the `RandomComponent` example in the source: https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Forms/Components/RandomComponent.cs.

# Version 0.9.116

## Removed `AsList`

The `AsList` method on the `IFieldConfiguration` interface has been removed in favour of `AsRadioList` and `AsCheckboxList`.

### Reason
We feel this is more discoverable and intention revealing than `AsList`.

### Workaround
Change any instances of using `@s.FieldFor(m => m.SomeField).AsList()` to `@s.FieldFor(m => m.SomeField).AsRadioList()` or `@s.FieldFor(m => m.SomeField).AsCheckboxList()`.

# Version 0.9.89

## Removed `FieldFor` extension method from `Form`

The `FieldFor` extension method on the `Form` has been deprecated in favour of a `FieldElementFor` method.

### Reason
This fits in more consistently with the nomenclature of ChameleonForms given that the method outputs the Field Element rather than the Field.

### Workaround
Change any instances of using `@f.FieldFor(m => m.SomeField)` to `@f.FieldElementFor(m => m.SomeField)`.

# Version 0.9.81

## Moved `DefaultFormTemplate` to `ChameleonForms.Templates.Default`

The `DefaultFormTemplate` class has been moved to the `ChameleonForms.Templates.Default` namespace.

### Reason
ChameleonForms now has more than one built-in template so it made sense to move them into their own namespaces.

### Workaround
If you referenced the `DefaultFormTemplate` at all then you will need to change the using statement from `ChameleonForms.Templates` to `ChameleonForms.Templates.Default`.

# Version 0.9.39

## `[ExistsIn]` server-side validation

The `[ExistsIn]` attribute now performs server-side validation by default to ensure the selected value is in the list.

You can opt-out of this validation globally by setting ExistsInAttribute.EnableValidation or per-usage by passing a boolean enableValidation parameter to the ExistsIn attribute.

If you do use validation, the attribute requires that the list is populated at validation time in the ASP.NET MVC pipeline. This requires you to either fill the list in the constructor of your view model or to create a model binder to populate the list.

If you don't populate the list before the validator runs then ExistsIn will throw an exception on trying to validate - you should either populate the list or disable validation to resolve this.

### Reason
We wanted to provide in-built server-side validation of fields using the `[ExistsIn]` attribute and the easiest way to do that is to hook into the validation pipeline in ASP.NET MVC.

### Workaround
If for some reason you don't want server-side validation to run then don't populate your list until after the validation pipeline runs. If you need to be able to opt out of validation, but still populate the list before then or to tweak the validation in some other way then [raise an issue](https://github.com/MRCollective/ChameleonForms/issues) so we can add that functionality.

# Version 0.9.20

## Buttons chain HtmlAttributes methods rather than taking as parameter

The submit/reset/button methods in Navigation now chain `HtmlAttributes` methods off the end rather than taking them as a parameter.

### Reason
This provides a much nicer experience when using these methods in your view - you don't have to new up a `HtmlAttributes` object anymore.

### Workaround
If you are using the old methods then change your `HtmlAttributes` object to use the same parameters that were in there, but by chaining method calls off of the end of the method.

For example:

    @n.Submit("Submit", new HtmlAttributes().AddClass("btn"))

Becomes:

    @n.Submit("Submit").AddClass("btn")

If you were passing in the `HtmlAttributes` object to the view rather than generating it inline then use `.Attrs(existingHtmlAttributesObject)`
