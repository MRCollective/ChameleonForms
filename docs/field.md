The Field is a single data collection unit; you create a Field by either calling the `Field` method on a Section or instantiating and outputting to the page a `Field<TModel>`.

You can also create a parent field that can have child fields nested within it by instantiating a `Field<TModel>` within a `using` block (the start and end of the `using` block will output the start and end HTML for the Field and the contents of the `using` block will output the child Fields).

The `Field<TModel>` class is defined as follows in the `ChameleonForms.Component` namespace:

```csharp
    /// <summary>
    /// Wraps the output of a single form field.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    public class Field<TModel> : FormComponent<TModel>
    {
        /// <summary>
        /// Creates a form field.
        /// </summary>
        /// <param name="form">The form the field is being created in</param>
        /// <param name="isParent">Whether or not the field has other fields nested within it</param>
        /// <param name="fieldGenerator">A field HTML generator class</param>
        /// <param name="config"> </param>
        public Field(IForm<TModel> form, bool isParent, IFieldGenerator fieldGenerator, IFieldConfiguration config)
            : base(form, !isParent) {...}
    }
```

The HTML for a Field is generated via the `Field` method in the template (or `BeginField` and `EndField` for the start and end HTML for a parent field).

A Field consists of 4 main sub-components:

* [Field Element](field-element) - The HTML that makes up a control that accepts data from the user
* [Field Label](field-label) - Text that describes a Field Element to a user (and is linked to that Field Element)
* [Field Validation HTML](field-validation-html) - Markup that acts as a placeholder to display any validation messages for a particular Field Element
* [Field Configuration](field-configuration) - The configuration for a particular Field, Field Element and/or Field Label

Default usage
-------------

### Manually specify HTML

If you want to define your own HTML for the Field Element, Field Label and Field Validation HTML then you can do so by using the `Field` method on the Section, e.g.:

```csharp
using (var s = f.BeginSection("Title")) {
    @s.Field(new HtmlString("label"), new HtmlString("element")).ChainFieldConfigurationMethodsHere()
    @* Or, if you want to specify all the possible values: *@
    @s.Field(new HtmlString("label"), new HtmlString("element"), new HtmlString("validation"), new ModelMetadata(...), isValid: false).ChainFieldConfigurationMethodsHere()
}
```

The `Field` method on the Section looks like this:

```csharp
        /// <summary>
        /// Outputs a field with passed in HTML.
        /// </summary>
        /// <param name="labelHtml">The HTML for the label part of the field</param>
        /// <param name="elementHtml">The HTML for the field element part of the field</param>
        /// <param name="validationHtml">The HTML for the validation markup part of the field</param>
        /// <param name="metadata">Any field metadata</param>
        /// <param name="isValid">Whether or not the field is valid</param>
        /// <returns>A field configuration that can be used to output the field as well as configure it fluently</returns>
        public IFieldConfiguration Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml = null, ModelMetadata metadata = null, bool isValid = true) {...}
```

### Use a Field Generator to output a single field in a Section

If you would like ChameleonForms to use a Field Generator to generate the HTML for the Field Element, Field Label and Field Validation HTML from a field on the model then you can use the `FieldFor` extension method on the Section, e.g.:

```csharp
using (var s = f.BeginSection("Title")) {
    @s.FieldFor(m => m.FieldOnTheModel).ChainFieldConfigurationMethodsHere()
}
```

The `FieldFor` extension method looks like this:

```csharp
        /// <summary>
        /// Creates a single form field as a child of a form section.
        /// </summary>
        /// <example>
        /// @s.FieldFor(m => m.FirstName)
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="section">The section the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <returns>A field configuration object that allows you to configure the field</returns>
        public static IFieldConfiguration FieldFor<TModel, T>(this Section<TModel> section, Expression<Func<TModel, T>> property)
        {
            var fc = new FieldConfiguration();
            new Field<TModel>(section.Form, false, section.Form.GetFieldGenerator(property), fc);
            return fc;
        }
```

### Use a Field Generator to output a parent field in a Section

If you want to use a Field Generator and want to nest child Fields under a Field then you can use the `BeginFieldFor` extension method on the Section (optionally with a Field Configuration), e.g.:

```csharp
using (var s = f.BeginSection("Title")) {
    using (var ff = s.BeginFieldFor(m => m.FieldOnTheModel, Field.Configure().ChainFieldConfigurationMethodsHere())) {
        @* Child Fields *@
    }
}
```

The `BeginFieldFor` extension method looks like this:

```csharp
        /// <summary>
        /// Creates a single form field as a child of a form section that can have other form fields nested within it.
        /// </summary>
        /// <example>
        /// @using (var f = s.BeginFieldFor(m => m.Company)) {
        ///     @f.FieldFor(m => m.PositionTitle)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="section">The section the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <param name="config">Any configuration information for the field</param>
        /// <returns>The form field</returns>
        public static Field<TModel> BeginFieldFor<TModel, T>(this Section<TModel> section, Expression<Func<TModel, T>> property, IFieldConfiguration config = null)
        {
            return new Field<TModel>(section.Form, true, section.Form.GetFieldGenerator(property), config);
        }
```

### Use a Field Generator to output a single field in a parent Field

If you want to use a Field Generator to create nested Fields under a parent Field then you can use the `BeginFieldFor` extension method on the Field (with an optional Field Configuration), e.g.:

```csharp
using (var ff = s.BeginFieldFor(m => m.FieldOnTheModel)) {
    @ff.FieldFor(m => m.ChildField).ChainFieldConfigurationMethodsHere()
}
```

The `FieldFor` extension method looks like this:

```csharp
        /// <summary>
        /// Creates a single form field as a child of another form field.
        /// </summary>
        /// <example>
        /// @using (var f = s.BeginFieldFor(m => m.Company)) {
        ///     @f.FieldFor(m => m.PositionTitle)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <typeparam name="T">The type of the field being generated</typeparam>
        /// <param name="field">The parent field the field is being created in</param>
        /// <param name="property">A lamdba expression to identify the field to render the field for</param>
        /// <returns>A field configuration object that allows you to configure the field</returns>
        public static IFieldConfiguration FieldFor<TModel, T>(this Field<TModel> field, Expression<Func<TModel, T>> property)
        {
            var config = new FieldConfiguration();
            new Field<TModel>(field.Form, false, field.Form.GetFieldGenerator(property), config);
            return config;
        }
```

Default HTML
------------

### Field

```html
            <dt>%labelHtml% %requiredDesignator%</dt>
            <dd class="%fieldContainerClasses%">
                %prependedHtml% %fieldElement% %appendedHtml% %hint% %validationHtml%
            </dd>
```

The `%requiredDesignator%` is shown if the field is required:

```html
<em class="required">*</em>
```

The `%hint%` is shown if a hint is specified in the Field Configuration:

```html
<div class="hint">%hint%</div>
```

### Begin HTML (parent)

```html
            <dt>%labelHtml% %requiredDesignator%</dt>
            <dd class="%fieldContainerClasses%">
                %prependedHtml% %fieldElement% %appendedHtml% %hint% %validationHtml%
                <dl>
```

### End HTML (parent)

```html
                </dl>
            </dd>
```

Twitter Bootstrap 3 HTML
------------------------

### Field: Input (except checkbox and file upload), textarea or select control

```html
        <div class="form-group%if !isValid% has-error%endif%%fieldContainerClasses%">
            %if withoutLabel%<span class="control-label">%endif%
                %labelHtml class="control-label"%
            %if withoutLabel%</span>%endif%
            %if isInputGroup or isRequired%
                <div class="input-group">
            %endif%
                %prependedHtml%
                %fieldElement class="form-control"%
                %appendedHtml%
                %if isRequired%
                    <div class="input-group-addon required">%requiredDesignator%</div>
                %endif%
            %if isInputGroup or isRequired%
                </div>
            %endif%
            %hint%
            %validationHtml class="help-block"%
        </div>
```

### Field: Single checkbox

```html
        <div class="checkbox%if !isValid% has-error%endif%%fieldContainerClasses%">
            %prependedHtml%
            %fieldElement%
            %requiredDesignator%
            %appendedHtml%
            %hint%
            %validationHtml class="help-block"%
        </div>
```

### Field: Radio/Checkbox list

```html
        <div class="form-group%if !isValid% has-error%endif%%fieldContainerClasses%">
            <span class="control-label">
                %labelHtml% %requiredDesignator%
            </span>
            %prependedHtml%
            %fieldElement%
            %appendedHtml%
            %hint%
            %validationHtml class="help-block"%
        </div>
```

### Field: Other (e.g. file upload)

```html
        <div class="form-group%if !isValid% has-error%endif%%fieldContainerClasses%">
            %if withoutLabel%<span class="control-label">%endif%
                %labelHtml% %requiredDesignator%
            %if withoutLabel%</span>%endif%
            %prependedHtml%
            %fieldElement%
            %appendedHtml%
            %hint%
            %validationHtml class="help-block"%
        </div>
```

### Common elements

The `%requiredDesignator%` is shown if the field is required:

```html
<em class="required" title="Required">&lowast;</em>
```

The `%hint%` is shown if a hint is specified in the Field Configuration:

```html
<div class="help-block form-hint">%hint</div>
```

### Input Groups

If the Field Element is within an [input group](http://getbootstrap.com/components/#input-groups) then the prepended and appended HTML will be wrapped in the following:

```html
<div class="input-group-addon">%html%</div>
```

A field is in an input group if:

* The field is an input (except checkbox and file upload), textarea or select control and:
    * The field is required (since the Form Template appends the required designator as an input group add-on); or
    * You use the `AsInputGroup` extension method from the `ChameleonForms.Templates.TwitterBootstrap3` namespace on the `IFieldConfiguration`

In all other situations you will manually need to add wrapping HTML with the relevant classes (e.g. using `Append` and `Prepend` on the Field Configuration).

As an example of what you can do with the input group consider the following:

```csharp
@s.FieldFor(m => m.Int).AsInputGroup().Append(".00").Prepend("$")
```

This will render like this:

![Example of int field with input group](int-field-with-bootstrap-input-group.png)

In order to be able to swap out the extension method usage across your application easily if you change your form template we recommend that rather than adding a using statement to `ChameleonForms.Templates.TwitterBootstrap3` for each view that has a form using the extension method you [add the namespace to your `Views\Web.config` file](getting-started#namespaces-in-viewswebconfig).

### Parent field

**Begin HTML**

The HTML is the same as the Field HTML specified above, but the last `<div>` is replaced with:

```html
            <div class="row nested-fields">
                <div class="col-xs-1"></div>
                <div class="col-xs-11">
```

**End HTML**

```html
                </div>
            </div>
        </div>
```