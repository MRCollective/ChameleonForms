Section
=======

The Section is a grouping of a set of fields; you create a Section by instantiating a `Section<TModel>` within a `using` block. The start and end of the `using` block will output the start and end HTML for the Section and the inside of the `using` block will contain the Section fields.

The `Section<TModel>` class looks like this and is in the `ChameleonForms.Component` namespace:

```csharp
    /// <summary>
    /// Wraps the output of a form section.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    public class Section<TModel> : FormComponent<TModel>
    {
        /// <summary>
        /// Creates a form section
        /// </summary>
        /// <param name="form">The form the message is being created in</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="nested">Whether the section is nested within another section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        public Section(IForm<TModel> form, IHtmlContent heading, bool nested, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null) : base(form, false) {...}

        /// <summary>
        /// Outputs a field with passed in HTML.
        /// </summary>
        /// <param name="labelHtml">The HTML for the label part of the field</param>
        /// <param name="elementHtml">The HTML for the field element part of the field</param>
        /// <param name="validationHtml">The HTML for the validation markup part of the field</param>
        /// <param name="metadata">Any field metadata</param>
        /// <param name="isValid">Whether or not the field is valid</param>
        /// <returns>A field configuration that can be used to output the field as well as configure it fluently</returns>
        public IFieldConfiguration Field(IHtmlContent labelHtml, IHtmlContent elementHtml, IHtmlContent validationHtml = null, ModelMetadata metadata = null, bool isValid = true) {...}
    }
```

The start and end HTML of the Section are generated via the `BeginSection` and `EndSection` methods in the template (or `BeginNestedSection` and `EndNestedSection` if the Section is a child of another Section). The field HTML is generated via the `Field` method in the template.

Default usage
-------------

In order to get an instance of a `Section<TModel>` you can use the `BeginSection` method on the Form, e.g.

```csharp
using (var s = f.BeginSection("Heading")) {
    @* Section fields go here *@
}
```

The `BeginSection` extension method looks like this:

```csharp
        /// <summary>
        /// Creates a top-level form section.
        /// </summary>
        /// <example>
        /// @using (var s = f.BeginSection("Section Heading")) {
        ///     @s.FieldFor(m => m.FirstName)
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <param name="form">The form the section is being created in</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        /// <returns>The form section</returns>
        public static Section<TModel> BeginSection<TModel>(this IForm<TModel> form, string heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(form, heading.ToHtml(), false, leadingHtml, htmlAttributes);
        }
```

From within a Section you can create [Fields](the-field) and you can also create nested sections using the `BeginSection` extension method off the Section:

```csharp
using (var s = f.BeginSection("Heading")) {
    @* Fields... *@
    using (var ss = s.BeginSection("Inner Heading")) {
        @* Fields... *@
    }
    @* Fields... *@
}
```

The `BeginSection` extension method on Section looks like this:

```csharp
        /// <summary>
        /// Creates a nested form section.
        /// </summary>
        /// <example>
        /// @using (var s = f.BeginSection("Section heading")) {
        ///     using (var ss = s.BeginSection("Nested section heading")) {
        ///         @ss.FieldFor(m => m.FirstName)
        ///     }
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <param name="section">The section the section is being created under</param>
        /// <param name="heading">The heading for the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes to apply to the section container</param>
        /// <returns>The nested form section</returns>
        public static Section<TModel> BeginSection<TModel>(this Section<TModel> section, string heading = null, IHtmlContent leadingHtml = null, HtmlAttributes htmlAttributes = null)
        {
            return new Section<TModel>(section.Form, heading.ToHtml(), true, leadingHtml, htmlAttributes);
        }
```

Default HTML
------------

### Begin HTML

```html
    <fieldset %htmlAttributes%>
        %if heading%<legend>%heading%</legend>%endif%
        %leadingHtml%
        <dl>
```

### End HTML

```html
        </dl>
    </fieldset>
```

### Begin HTML (nested)

```html
            %if heading%<dt>%heading%</dt>%endif%
            <dd>
                %leadingHtml%
                <dl %htmlAttributes%>
```

### End HTML (nested)

```html
                </dl>
            </dd>
```

Twitter Bootstrap 3 HTML
------------------------

### Begin HTML

```html
    <fieldset %htmlAttributes%>
        %if heading%<legend>%heading%</legend>%endif%
        %leadingHtml%
```

### End HTML

```html
    </fieldset>
```

### Begin HTML (nested)

```html
    <div class="panel panel-default" %htmlAttributes%>
        %if heading%<div class="panel-heading">%heading%</div>%endif%
        <div class="panel-body">
        %leadingHtml%
```

### End HTML (nested)

```html
        </div>
    </div>
```