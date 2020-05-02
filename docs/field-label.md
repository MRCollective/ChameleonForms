# Field Label

The Field Label is the HTML that makes up a label for a single Field. This Field Label can be:

* [Specified manually](field.md#manually-specify-html)
* Created by a [Field Generator](index.md#field-types) based on the metadata of the model property being displayed and the Field Configuration specified when it's:
    * Displayed as part of a [Field](field.md)
    * [Output directly from the Form](#outputting-directly-from-the-form)

## Outputting directly from the Form

To use a Field Generator to output the HTML for a standalone Field Label you can use the `LabelFor` extension method on the Form, e.g.:

```csharp
@f.LabelFor(m => m.SomeField).ChainFieldConfigurationMethodsHere()
```

The `LabelFor` extension method looks like this:

```csharp
        /// <summary>
        /// Creates a standalone label to be output in a form for a field.
        /// </summary>
        /// <example>
        /// @using (var f = Html.BeginChameleonForm()) {
        ///     @f.LabelFor(m => m.PositionTitle)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="form">The form the label is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the label for</param>
        /// <returns>The HTML for the label</returns>
        public static IFieldConfiguration LabelFor<TModel, T>(this IForm<TModel> form, Expression<Func<TModel, T>> property)
        {
            var config = new FieldConfiguration();
            config.SetField(() => form.GetFieldGenerator(property).GetLabelHtml(config));
            return config;
        }
```

## Default HTML

The HTML for the Field Label will be determined depending on the metadata of the model property being specified and the options in the [Field Configuration](field-configuration.md). The HTML of the Field Label will by default be:

```html
<label for="%field-element-id%" (class="%labelClasses%")>%labelText%</label>
```

The `%id%` will be determined by using the built-in ASP.NET Core MVC methods for generating field ids, unless the `.Id("overriddenId")` method is called on the Field Configuration to override the id.

To add classes to the label then use the `AddLabelClass` method on the Field Configuration.

If the `WithoutLabelElement()` method is called on the Field Configuration then the HTML of the Field Label will be:

```html
%labelText%
```

The `%labelText%` will be determined by using the following (in order of preference from most to least if specified (not-null)):

1. The text specified by calling the `.Label(...)` method on the Field Configuration (**note:** if you use this then any validation messages will use the resulting label from the other points in this list and *not* what you specify in the `.Label(...)` method, it will just impact the display of the label in this one place)
2. The `DisplayName` property in the ModelMetadata for the model property being displayed
3. The `Name` property in the ModelMetadata for the model property being displayed
4. The name of the model property being displayed (all lower-case except for the first character)

By default, ChameleonForms will set sensible defaults in the `DisplayName` property in ModelMetadata by humanizing the property name; [consult the relevant documentation to find out more](labels.md).
