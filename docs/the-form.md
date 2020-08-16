# Form

The Form is the root element of a ChameleonForms form; you create a Form by either using the `<chameleon-form>` tag helper, or instantiating an `IForm<TModel>` within a `using` block.

The `IForm<TModel>` interface looks like this and is in the `ChameleonForms` namespace:

```cs
    /// <summary>
    /// Interface for a Chameleon Form.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>    
    public interface IForm<TModel> : IForm, IDisposable
    {
        /// <summary>
        /// The HTML helper for the current view.
        /// </summary>
        IHtmlHelper<TModel> HtmlHelper { get; }
        /// <summary>
        /// The template renderer for the current view.
        /// </summary>
        IFormTemplate Template { get; }
        /// <summary>
        /// Writes a HTML String directly to the view's output.
        /// </summary>
        /// <param name="htmlContent">The HTML to write to the view's output</param>
        void Write(IHtmlContent htmlContent);

        /// <summary>
        /// The field generator for the given field.
        /// </summary>
        /// <param name="property">The property to return the field generator for</param>
        IFieldGenerator GetFieldGenerator<T>(Expression<Func<TModel, T>> property);

        /// <summary>
        /// Returns a wrapped <see cref="PartialViewForm{TModel}"/> for the given partial view helper.
        /// </summary>
        /// <param name="partialViewHelper">The HTML Helper from the partial view</param>
        /// <returns>The PartialViewForm wrapping the original form</returns>
        IForm<TModel> CreatePartialForm(IHtmlHelper<TModel> partialViewHelper);
    }
```

ChameleonForms comes with a standard implementation of the `IForm<TModel>` interface that uses the `BeginForm` and `EndForm` methods in the currently configured [form template](form-templates.md) and returns an instance of the `DefaultFieldGenerator` class when asked for a [Field Generator](custom-field-generator.md).

## Default usage

In order to create a self-submitting form using the [default form template](configuration.md#default-global-config) (see below if [you want to adjust it on a per-form basis](#configuring-the-form-template)):

# [Tag Helpers variant](#tab/default-form-th)

Use the `<chameleon-form>` tag helper:

```cshtml
<chameleon-form>
    @* Form content goes here *@
</chameleon-form>
```

# [HTML Helpers variant](#tab/default-form-hh)

Use the `BeginChameleonForm` extension method on the `HtmlHelper` that appears in MVC views, e.g.:

```cshtml
@using (var f = Html.BeginChameleonForm()) {
    @* Form content goes here *@
}
```

The `BeginChameleonForm` extension method looks like this:

```cs
        /// <summary>
        /// Constructs a <see cref="Form{TModel}"/> object with the default ChameleonForms template renderer.
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
        /// <param name="outputAntiforgeryToken">Whether or not to output an antiforgery token in the form; defaults to null which will output a token if the method isn't GET</param>
        /// <returns>A <see cref="Form{TModel}"/> object with an instance of the default form template renderer.</returns>
        public static IForm<TModel> BeginChameleonForm<TModel>(this IHtmlHelper<TModel> helper, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null, bool? outputAntiforgeryToken = null)
        {
            return new Form<TModel>(helper, helper.GetDefaultFormTemplate(), action, method, htmlAttributes, enctype, outputAntiforgeryToken);
        }
```

***

By default a self-submitting form, against the page model type, that performs a HTTP post with the browser's default `enctype` (usually `application/x-www-form-urlencoded`) is outputted, but you can change the submit location, HTTP verb, `enctype` and add any [HTML attributes you like](html-attributes.md) using the appropriate parameters, e.g.:

# [Tag Helpers variant](#tab/configure-form-th)

```cshtml
<chameleon-form action="@Url.Action("SomeAction")" method="Post" enctype="Multipart" id="my-form" disabled="false" add-class="a-class" fluent-config='c => c.Attr("data-a", "b")' attr-data-whatever="some value" output-antiforgery-token="false">
    @* Form content goes here *@
</chameleon-form>
```

# [HTML Helpers variant](#tab/configure-form-hh)

```cshtml
@using (var f = Html.BeginChameleonForm(action: Url.Action("SomeAction"), method: FormMethod.Post, enctype: EncType.Multipart, htmlAttributes: new HtmlAttributes().Id("my-form").Disabled(false).AddClass("a-class").Attr("data-a", "b").Attr("data-whatever", "some value"), outputAntiforgeryToken: false)) {
    @* Form content goes here *@
}
```

***

You can also [create a form against a model type different from the page model](different-form-models.md).

## Configuring the form template

As you can see above, when using the `BeginChameleonForm` extension method (which is also what the `<chameleon-form>` tag helper uses under the hood) it uses `helper.GetDefaultFormTemplate()` to determine what form template to use. [By default](configuration.md#default-global-config) this is set to an instance of the `DefaultFormTemplate` class from the `ChameleonForms.Templates.Default` namespace.

The way this works is the [global configuration](configuration.md) will register an implementation of `IFormTemplate` with the service collection within your ASP.NET Core web application. The `helper.GetDefaultFormTemplate()` extension will then resolve that default template implementation from the request services:

```cs
        /// <summary>
        /// Gets the registered default form template from RequestServices.
        /// </summary>
        /// <param name="htmlHelper">The HTML Helper</param>
        /// <returns>An instance of the default <see cref="IFormTemplate"/></returns>
        public static IFormTemplate GetDefaultFormTemplate(this IHtmlHelper htmlHelper)
        {
            return htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IFormTemplate>();
        }
```

If you would like to change the form template that is used then simply specify a different type when [registering ChameleonForms with the service collection](configuration.md#addservices-overloads):

```cs
services.AddChameleonForms<MyFormTemplate>();
```

This will new up your form template using a parameterless constructor and then register it as a singleton against `IFormTemplate`.

If you want more control on how your template is instantiated and/or registered then you can opt out of ChameleonForms registering your template and instead register it yourself, e.g.:

```cs
services.AddChameleonForms(b => b.WithoutTemplateTypeRegistration());
services.AddSingleton<IFormTemplate>(new MyFormTemplate(/* constructor parameters */));
```

If you want to use multiple Form Templates across your application you can [create your own extension methods](custom-template.md) or create your own tag helper [based on the `<chameleon-form>` one](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/TagHelpers/ChameleonFormTagHelper.cs) to allow for different form templates to be specified on a per-form basis.

## HTML5 validation

By default, ChameleonForms opts out of HTML5 validation for you via the `novalidate="novalidate"` attribute on the `<form>`. It does this so that you can retain control of client-side validation (e.g. through unobtrusive validation), which is typically able to yield a better user experience than HTML5 validation.

If you want to override this behaviour you can configure your own form template.

## Default HTML

### Begin HTML

```html
<form action="%action%" method="%method%" (enctype="%enctype%") (%htmlAttributes%) novalidate="novalidate">
```

### End HTML

```html
</form>
```

## Twitter Bootstrap 3 HTML

Same as default.
