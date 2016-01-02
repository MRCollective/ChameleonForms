Field Element
=============

The Field Element is the HTML that makes up a control to accept data from the user for a single Field. The Field Element can be:

* [Specified manually](field#manually-specify-html)
* Created by a [Field Generator](./#field-types) based on the metadata of the model property being displayed and the Field Configuration specified when it's:
    * Displayed as part of a [Field](field)
    * Output directly from the Form

Outputting directly from the Form
---------------------------------

To use a Field Generator to output the HTML for a standalone Field Element you can use the `FieldElementFor` extension method on the Form (with optional Field Configuration), e.g.:

```csharp
@f.FieldElementFor(m => m.SomeField).ChainFieldConfigurationMethodsHere()
```

The `FieldElementFor` extension method looks like this:

```csharp
        /// <summary>
        /// Creates a standalone form field to be output in a form.
        /// </summary>
        /// <example>
        /// @using (var f = Html.BeginChameleonForm()) {
        ///     <p>@f.FieldElementFor(m => m.PositionTitle)</p>
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="form">The form the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <returns>A field configuration object that allows you to configure the field</returns>
        public static IFieldConfiguration FieldElementFor<TModel, T>(this IForm<TModel> form, Expression<Func<TModel, T>> property)
        {
            var config = new FieldConfiguration();
            config.SetField(() => form.GetFieldGenerator(property).GetFieldHtml(config));
            return config;
        }
```

Default HTML
------------

The HTML for the Field Element will be determined depending on the metadata of the model property being specified and the options in the [Field Configuration](field-configuration). The HTML of the Field Element will by default simply be:

```html
%fieldElement%
```