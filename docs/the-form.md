Form
====

The Form is the root element of a ChameleonForms form; you create a Form by instantiating an `IForm<TModel>` within a `using` block. The start and end of the `using` block will output the start and end HTML for the form and the inside of the `using` block will contain the Form content.

The `IForm<TModel>` interface looks like this and is in the `ChameleonForms` namespace:

```c#
    /// <summary>
    /// Interface for a Chameleon Form.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    public interface IForm<TModel> : IDisposable
    {
        /// <summary>
        /// The HTML helper for the current view.
        /// </summary>
        HtmlHelper<TModel> HtmlHelper { get; }
        /// <summary>
        /// The template renderer for the current view.
        /// </summary>
        IFormTemplate Template { get; }
        /// <summary>
        /// Writes a HTML String directly to the view's output.
        /// </summary>
        /// <param name="htmlString">The HTML to write to the view's output</param>
        void Write(IHtmlString htmlString);

        /// <summary>
        /// The field generator for the given field.
        /// </summary>
        /// <param name="property">The property to return the field generator for</param>
        IFieldGenerator GetFieldGenerator<T>(Expression<Func<TModel, T>> property);
    }
```

ChameleonForms comes with a standard implementation of the `IForm<TModel>` interface that uses the `BeginForm` and `EndForm` methods in the given form template and returns an instance of the `DefaultFieldGenerator` class when asked for a Field Generator.

Default usage
-------------

In order to get an instance of an `IForm<TModel>` using the default form template (by default the default form template uses definition lists, [but you you can adjust it](#configuring-the-default-form-template)) you can use the `BeginChameleonForm` extension method on the `HtmlHelper` that appears in MVC views, e.g.:

```c#
@using (var f = Html.BeginChameleonForm()) {
    @* Form content goes here *@
}
```

The `BeginChameleonForm` extension method looks like this:

```c#
        /// <summary>
        /// Constructs a <see cref="Form{TModel}"/> object with the default Chameleon form template renderer.
        /// </summary>
        /// <example>
        /// @using (var f = Html.BeginChameleonForm(...)) {
        ///     ...
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>
        /// <param name="helper">The HTML Helper for the current view</param>
        /// <param name="action">The action the form should submit to</param>
        /// <param name="method">The HTTP method the form submission should use</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use</param>
        /// <param name="enctype">The encoding type the form submission should use</param>
        /// <returns>A <see cref="Form{TModel}"/> object with an instance of the default form template renderer.</returns>
        public static IForm<TModel> BeginChameleonForm<TModel>(this HtmlHelper<TModel> helper, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
        {
            return new Form<TModel>(helper, FormTemplate.Default, action, method, htmlAttributes, enctype);
        }
```

By default a self-submitting form that performs a HTTP post with the browser's default enctype is outputted, but you can change the submit location, HTTP verb, enctype and add any [HTML attributes you like](html-attributes) using the appropriate parameters.

### Configuring the Default Form Template

As you can see above, when using the `BeginChameleonForm` extension method it uses `FormTemplate.Default` to determine what form template to use. By default this is set to an instance of the `DefaultFormTemplate` class from the `ChameleonForms.Templates.Default` namespace.

If you would like to change the form template that is used then simply set an instance of `IFormTemplate` to the `FormTemplate.Default` property. This is a public static property setter so it's not thread-safe and thus you should do this inside of the `Application_Start` method (or a method it calls) inside of `Global.asax.cs`.

If you want to use Twitter Bootstrap 3 then this is how you can use it - [see the relevant documentation for more information](bootstrap-template).

If you want more control over constructing the Form Template or you want to use multiple Form Templates across your application you can [create your own extension methods](custom-template).

Default HTML
------------

### Begin HTML

```html
<form action="%action%" method="%method%" (enctype="%enctype%") (%htmlAttributes%)>
```

### End HTML

```html
</form>
```

Twitter Bootstrap 3 HTML
------------------------

Same as default.