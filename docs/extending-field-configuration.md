Extending Field Configuration
=============================

The `Bag` property, which is `dynamic` provides a way to store arbitrary data. The [`FieldConfiguration`](field-configuration) class, which is the default implementation of `IFieldConfiguration`, instantiates this property as an `ExpandoObject`.

To extend the Field Configuration you can create an extension method against `IFieldConfiguration`, which adds data to the `Bag` property (or alternatively, you can modify the `Attributes` property if you want your extension method to add HTML attributes).

If you are using the `Bag` property then you will likely need to create your own [custom template](custom-template) to then pull that data out of the `Bag` property on the `IReadonlyFieldConfiguration` (which will be referenced from the `IFieldConfiguration` when `.ToReadonly()` is called).

Example
-------

For an example of this in action see the extension we added to the [Twitter Bootstrap 3 template](bootstrap-template) to allow you to [specify an input group](field#input-groups).

Firstly, the [definition of the extension method](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/Templates/TwitterBootstrap3/FieldConfigurationExtensions.cs) is:

```csharp
        /// <summary>
        /// Outputs the field in an input group using prepended and appended HTML.
        /// </summary>
        /// <example>
        /// @n.Field(labelHtml, elementHtml, validationHtml, metadata, new FieldConfiguration().Prepend(beforeHtml).Append(afterHtml).AsInputGroup(), false)
        /// </example>
        /// <param name="fc">The configuration for a field</param>
        /// <returns>The field configuration object to allow for method chaining</returns>
        public static IFieldConfiguration AsInputGroup(this IFieldConfiguration fc)
        {
            fc.Bag.DisplayAsInputGroup = true;
            return fc;
        }
```

Then the corresponding code in the [template](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/Templates/TwitterBootstrap3/TwitterBootstrapHtmlHelpers.cshtml#L110) that gets the value as a local variable to switch on is:

```csharp
var isInputGroup = canBeInputGroup && (isRequired || fieldConfiguration.GetBagData<bool>("DisplayAsInputGroup"));
```

Note in particular the `fieldConfiguration.GetBagData<bool>("DisplayAsInputGroup")`.

Namespaces
----------

In order to be able to swap out the extension method usage across your application easily if you change your form template we recommend that rather than adding a using statement to the namespace that contains your extension meethod for each view that you instead [add the namespace to your `Views\Web.config` file](getting-started#namespaces-in-viewswebconfig).