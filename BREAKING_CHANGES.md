ChameleonForms Breaking Changes
-------------------------------

Version 2.0.0
=============

Removed the `TTemplate` type parameter from all of the form component classes. This will only affect you if you created custom form components e.g. you extended `Component`, `Form`, `Section`, `Naviation`, etc.

### Reason

It made extensibility harder to have to pass that type around everywhere and it wasn't really being used. Using polymorphism with a property of type `IFormTemplate` made more sense.

### Workaround

You need to change any custom components to simply remove the template type. If you custom component relied on that template type for strong typing then you can follow the example of the `RandomComponent` example in the source: https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Forms/Components/RandomComponent.cs.

Version 0.9.116
===============

The `AsList` method on the `IFieldConfiguration` interface has been removed in favour of `AsRadioList` and `AsCheckboxList`.

### Reason
We feel this is more discoverable and intention revealing than `AsList`.

### Workaround
Change any instances of using `@s.FieldFor(m => m.SomeField).AsList()` to `@s.FieldFor(m => m.SomeField).AsRadioList()` or `@s.FieldFor(m => m.SomeField).AsCheckboxList()`.

Version 0.9.89
==============

The `FieldFor` extension method on the Form has been deprecated in favour of a `FieldElementFor` method.

### Reason
This fits in more consistently with the nomenclature of ChameleonForms given that the method outputs the Field Element rather than the Field.

### Workaround
Change any instances of using `@f.FieldFor(m => m.SomeField)` to `@f.FieldElementFor(m => m.SomeField)`.

Version 0.9.81
==============

The `DefaultFormTemplate` class has been moved to the `ChameleonForms.Templates.Default` namespace.

### Reason
ChameleonForms now has more than one built-in template so it made sense to move them into their own namespaces.

### Workaround
If you referenced the `DefaultFormTemplate` at all then you will need to change the using statement from `ChameleonForms.Templates` to `ChameleonForms.Templates.Default`.

Version 0.9.39
==============

The `[ExistsIn]` attribute now performs server-side validation by default to ensure the selected value is in the list.

You can opt-out of this validation globally by setting ExistsInAttribute.EnableValidation or per-usage by passing a boolean enableValidation parameter to the ExistsIn attribute.

If you do use validation, the attribute requires that the list is populated at validation time in the ASP.NET MVC pipeline. This requires you to either fill the list in the constructor of your view model or to create a model binder to populate the list.

If you don't populate the list before the validator runs then ExistsIn will throw an exception on trying to validate - you should either populate the list or disable validation to resolve this.

### Reason
We wanted to provide in-built server-side validation of fields using the `[ExistsIn]` attribute and the easiest way to do that is to hook into the validation pipeline in ASP.NET MVC.

### Workaround
If for some reason you don't want server-side validation to run then don't populate your list until after the validation pipeline runs. If you need to be able to opt out of validation, but still populate the list before then or to tweak the validation in some other way then [raise an issue](https://github.com/MRCollective/ChameleonForms/issues) so we can add that functionality.

Version 0.9.20
==============

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

