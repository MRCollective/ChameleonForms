# Custom Field Generator

Part of the core of ChameleonForms is the concept of a Field Generator. A Field Generator is responsible for generating the HTML for the various parts of a [Field](field.md). A Field Generator will implement the `IFieldGenerator<TModel, T>` interface, the definition for which is shown below:

```cs
    /// <summary>
    /// Generates the HTML for a single form field.
    /// </summary>
    public interface IFieldGenerator<TModel, T> : IFieldGenerator
    {
        /// <summary>
        /// A HTML helper for the model.
        /// </summary>
        IHtmlHelper<TModel> HtmlHelper { get; }

        /// <summary>
        /// The expression that identifies the property in the model being output.
        /// </summary>
        Expression<Func<TModel, T>> FieldProperty { get; }

        /// <summary>
        /// Returns the current value of the field.
        /// </summary>
        /// <returns>The current field value</returns>
        T GetValue();

        /// <summary>
        /// Returns a model with the current values for the form.
        /// </summary>
        /// <returns>The current model</returns>
        TModel GetModel();

        /// <summary>
        /// Returns the displayable name of the field being generated.
        /// </summary>
        /// <returns>The id</returns>
        string GetFieldDisplayName();

        /// <summary>
        /// Returns any custom attributes against the field being generated.
        /// </summary>
        /// <returns>The attributes</returns>
        IEnumerable<Attribute> GetCustomAttributes();
    }

    /// <summary>
    /// Generates the HTML for a single form field.
    /// </summary>
    public interface IFieldGenerator
    {
        /// <summary>
        /// The metadata for the form field.
        /// </summary>
        ModelMetadata Metadata { get; }

        /// <summary>
        /// The form template that will be used to render the form.
        /// </summary>
        IFormTemplate Template { get; }

        /// <summary>
        /// Turns the given <see cref="IFieldConfiguration"/> into a <see cref="FieldConfiguration"/> ready to use for generating the form field.
        /// </summary>
        /// <param name="fieldConfiguration">The field configuration to modify</param>
        /// <param name="fieldParent">The parent component of the field</param>
        /// <returns>The readonly field configuration; ready for generating the form field</returns>
        IReadonlyFieldConfiguration PrepareFieldConfiguration(IFieldConfiguration fieldConfiguration, FieldParent fieldParent);

        /// <summary>
        /// Creates the HTML for the field control.
        /// </summary>
        /// <returns>The HTML for the field control</returns>
        IHtmlContent GetFieldHtml(IReadonlyFieldConfiguration fieldConfiguration);

        /// <summary>
        /// Creates the HTML for the field label.
        /// </summary>
        /// <returns>The HTML for the field label</returns>
        IHtmlContent GetLabelHtml(IReadonlyFieldConfiguration fieldConfiguration);

        /// <summary>
        /// Creates the HTML for the field's validation messages
        /// </summary>
        /// <returns>The HTML for the field's validation messages</returns>
        IHtmlContent GetValidationHtml(IReadonlyFieldConfiguration fieldConfiguration);

        /// <summary>
        /// Creates the HTML for the field control after preparing the given field configuration.
        /// </summary>
        /// <returns>The HTML for the field control</returns>
        IHtmlContent GetFieldHtml(IFieldConfiguration fieldConfiguration);

        /// <summary>
        /// Creates the HTML for the field label after preparing the given field configuration.
        /// </summary>
        /// <returns>The HTML for the field label</returns>
        IHtmlContent GetLabelHtml(IFieldConfiguration fieldConfiguration);

        /// <summary>
        /// Creates the HTML for the field's validation messages after preparing the given field configuration.
        /// </summary>
        /// <returns>The HTML for the field's validation messages</returns>
        IHtmlContent GetValidationHtml(IFieldConfiguration fieldConfiguration);

        /// <summary>
        /// Returns the id of the field being generated.
        /// </summary>
        /// <returns>The id</returns>
        string GetFieldId();
    }
```

This is implemented by the [Default Field Generator](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/FieldGenerators/DefaultFieldGenerator.cs) by default. This includesstandard rules for preparing a field configuration such as setting up format strings, setting readonly fields, setting `aria-describedby` attribute for hints, etc. as well as determining the HTML to display for the field parts such as the label and field element.

If the Default Field Generator doesn't cut it for you then feel free to create your own field generator and use it instead by creating your own [Form](the-form.md) class and overriding the `GetFieldGenerator` method.

You will need to create your [own extension method in place of `BeginChameleonForm`](form-templates.md#custom-extension-method) and/or your [own tag helper in place of `<chameleon-form>`](the-form.md) to new up your custom `Form` class.
