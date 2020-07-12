# Field Validation HTML

The Field Validation HTML is markup that acts as both a placeholder to display any client-side validation messages for a particular Field as well as displaying any server-side validation messages for that Field. The Field Validation HTML can be:

* [Specified manually](field.md#manually-specify-html)
* Created by a [Field Generator](index.md#field-types) based on the metadata of the model property being displayed and the Field Configuration specified when it's:
    * Displayed as part of a [Field](field.md)
    * [Output directly from the Form](#outputting-directly-from-the-form)

## Outputting directly from the Form

To use a Field Generator to output the HTML for a standalone Field Validation HTML you can use the `ValidationMessageFor` extension method on the Form, e.g.:

```cshtml
@f.ValidationMessageFor(m => m.SomeField).ChainedFieldConfigurationMethodsHereAreIgnored()
```

The `ValidationMessageFor` extension method looks like this:

```csharp
        /// <summary>
        /// Creates a standalone validation message to be output in a form for a field.
        /// </summary>
        /// <example>
        /// @using (var f = Html.BeginChameleonForm()) {
        ///     @f.ValidationMessageFor(m => m.PositionTitle)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="form">The form the label is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the validation message for</param>
        /// <returns>The HTML for the validation message</returns>
        public static IFieldConfiguration ValidationMessageFor<TModel, T>(this IForm<TModel> form, Expression<Func<TModel, T>> property)
        {
            var config = new FieldConfiguration();
            config.SetField(() => form.GetFieldGenerator(property).GetValidationHtml(config));
            return config;
        }
```

## Default HTML

The HTML for the Field Validation HTML is the same as calling:

```cshtml
@Html.ValidationMessageFor(m => m.SomeField, new { @class = %validationClasses% })
```

or

```html
<span asp-validation-for="SomeField" class="%validationClasses%"></span>
```

The default Field Generator ignores all properties on the Field Configuration when generating the Field Validation HTML apart from the `ValidationClasses` property, which you can set using the `AddValidationClass` method.
