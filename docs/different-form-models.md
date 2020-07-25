# Create a form against a model type different from the page model

Similar to the examples and reasoning discussed [in the documentation for changing HTML helper model types](html-helper-context.md) it's useful to be able to create forms against an arbitrary model unrelated to the page model or create a form against a subproperty of the parent model.

## Tag Helpers

Tag Helpers don't allow you to change a model within a page, instead you need to use `<partial model="model" name="_PartialName" />` or `<partial for="ChildProperty" name="_PartialName" />` to change the model type in a view. See [Partials](partials.md) for more information or below for examples.

## HTML Helpers

This can be achieved using the default form template by using these overloads to the `BeginChameleonForms` [extension method](the-form.md):

```cs
        /// <summary>
        /// Constructs a <see cref="Form{TModel}"/> object with the default ChameleonForms template renderer using a sub-property of the current model as the model.
        /// Values will bind back to the model type of the sub-property as if that was the model all along.
        /// </summary>
        /// <example>
        /// @using (var f = Html.BeginChameleonFormFor(m => m.Subproperty, ...)) {
        ///     ...
        /// }
        /// </example>
        /// <typeparam name="TParentModel">The model type of the view</typeparam>
        /// <typeparam name="TChildModel">The model type of the sub-property to construct the form for</typeparam>
        /// <param name="helper">The HTML Helper for the current view</param>
        /// <param name="formFor">A lambda expression identifying the sub-property to construct the form for</param>
        /// <param name="action">The action the form should submit to</param>
        /// <param name="method">The HTTP method the form submission should use</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use</param>
        /// <param name="enctype">The encoding type the form submission should use</param>
        /// <param name="outputAntiforgeryToken">Whether or not to output an antiforgery token in the form; defaults to null which will output a token if the method isn't GET</param>
        /// <returns>A <see cref="Form{TModel}"/> object with an instance of the default form template renderer.</returns>
        public static IForm<TChildModel> BeginChameleonFormFor<TParentModel, TChildModel>(this IHtmlHelper<TParentModel> helper, Expression<Func<TParentModel, TChildModel>> formFor, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null, bool? outputAntiforgeryToken = null)

        /// <summary>
        /// Constructs a <see cref="Form{TModel}"/> object with the default ChameleonForms template renderer using the given model type and instance.
        /// Values will bind back to the model type specified as if that was the model all along.
        /// </summary>
        /// <example>
        /// @using (var f = Html.BeginChameleonFormFor(new AnotherModelType(), ...)) {
        ///     ...
        /// }
        /// @using (var f = Html.BeginChameleonFormFor(default(AnotherModelType), ...)) {
        ///     ...
        /// }
        /// </example>
        /// <remarks>
        /// This can also be done using the For() HTML helper extension method and just a type:
        /// @using (var f = Html.For&lt;AnotherModelType&gt;().BeginChameleonForm(...)) {
        ///     ...
        /// }
        /// </remarks>
        /// <typeparam name="TOriginalModel">The model type of the view</typeparam>
        /// <typeparam name="TNewModel">The model type of the sub-property to construct the form for</typeparam>
        /// <param name="helper">The HTML Helper for the current view</param>
        /// <param name="model">The model to use for the form</param>
        /// <param name="action">The action the form should submit to</param>
        /// <param name="method">The HTTP method the form submission should use</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use</param>
        /// <param name="enctype">The encoding type the form submission should use</param>
        /// <param name="outputAntiforgeryToken">Whether or not to output an antiforgery token in the form; defaults to null which will output a token if the method isn't GET</param>
        /// <returns>A <see cref="Form{TModel}"/> object with an instance of the default form template renderer.</returns>
        public static IForm<TNewModel> BeginChameleonFormFor<TOriginalModel, TNewModel>(this IHtmlHelper<TOriginalModel> helper, TNewModel model, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null, bool? outputAntiforgeryToken = null)
```

## Examples

The examples on the [documentation for changing HTML helper model types](html-helper-context.md) are restated below, but using ChameleonForms instead of using `Html` to output the form.

### Using a different view model

# [Tag Helpers variant](#tab/example-th)

Tag Helpers are explicitly tied to the model of the page so it's not possible to have the different model inline. However, you can include another form with a different model by using a partial.

```cshtml
@model LoginViewModel

<h1>Login</h1>
<chameleon-form>
    <form-section>
        <field for="Username" />
        <field for="Password" />
    </form-section>
    <form-navigation>
        <submit-button label="Login" />
    </form-navigation>
</chameleon-form>

<h2>Don't have an account?</h2>
<p>You can easily create a new account in seconds - just start by selecting a username below.</p>
<partial name="_SignupForm" model="new SignupStep1ViewModel()" />
```

*_SignupForm.cshtml*
```cshtml
@model SignupStep1ViewModel

<chameleon-form action='@Url.Action("Step1", "Signup")'>
    <form-section>
        <field for="Username" placeholder="Enter your preferred username">
    </form-section>
    <form-navigation>
        <submit-button>Begin signup - check if my username is available &raquo;</submit-button>
    </form-navigation>
</chameleon-form>
```

# [HTML Helpers variant](#tab/example-hh)

```cshtml
@model LoginViewModel

<h1>Login</h1>
@using (var f = Html.BeginChameleonForm()) {
    using (var s = f.BeginSection()) {
        @s.FieldFor(m => m.Username)
        @s.FieldFor(m => m.Password)
    }
    using (var n = f.BeginNavigation()) {
        @n.Submit("Login")
    }
}

<h2>Don't have an account?</h2>
<p>You can easily create a new account in seconds - just start by selecting a username below.</p>
@using (var f = Html.BeginChameleonFor(default(SignupStep1ViewModel), Url.Action("Step1", "Signup"))) {
    using (var s = f.BeginSection) {
        @s.FieldFor(m => m.Username).Placeholder("Enter your preferred username")
    }
    using (var n = f.BeginNavigation()) {
        @n.Submit("Begin signup - check if my username is available &raquo;".ToHtml())
    }
}
```

***

### Binding against a child property

This is useful when you want to have readonly properties that always get set in the controller before showing a view, but you want a clean view model to bind against in the postback controller action. This technique can make your controller action cleaner and make it less likely that you will forget to set the readonly properties for the view or forget to add `[ReadOnly(true)]` to those properties if they were on the view model being bound to the controller.

As an example of this technique in action, let's say you have this view model and controller:

```cs
public class SomeViewModel
{
    public SomeViewModel(string readonlyProperty, SomeViewModelInput input)
    {
        ReadonlyProperty = readonlyProperty;
        InputModel = input;
    }

    public string ReadonlyProperty { get; set; }
    public SomeViewModelInput InputModel { get; set; }
}

public class SomeViewModelInput
{
    public string Property1 { get; set; }
    public string Property2 { get; set; }
    public string Property3 { get; set; }
    public string Property4 { get; set; }
    public string Property5 { get; set; }
}

public class SomeController : Controller
{
    public ActionResult Index()
    {
        return new View(new SomeViewModel("Value from database or whatever", new SomeViewModelInput()));
    }

    [HttpPost]
    public ActionResult Index(SomeViewModelInput model)
    {
        if (!ModelState.IsValid)
            return new View(new SomeViewModel("Value from database or whatever", model));

        // Do stuff with model...
    }
}
```

# [Tag Helpers variant](#tab/example2-th)

Tag Helpers are explicitly tied to the model of the page so it's not possible to have the different model inline. However, you can include another form with a different model by using a partial.

```cshtml
@model SomeViewModel

<h1>Creating new xyz against @Model.ReadonlyProperty</h1>
<partial name="_SomeViewModelInputForm" model="Model?.InputModel" />
```

*_SomeViewModelInputForm.cshtml*
```cshtml
<chameleon-form>
    <form-section>
        <field for="Property1" />
        <field for="Property2" />
        <field for="Property3" />
        <field for="Property4" />
        <field for="Property5" />
    </form-section>
    <form-navigation>
        <submit-button label="Submit" />
    </form-navigation>
</chameleon-form>
```

# [HTML Helpers variant](#tab/example2-hh)

```cshtml
@model SomeViewModel

<h1>Creating new xyz against @Model.ReadonlyProperty</h1>
@using (var f = Html.BeginChameleonFormFor(m => m.InputModel)) {
    using (var s = f.BeginSection()) {
        @s.FieldFor(m => m.Property1)
        @s.FieldFor(m => m.Property2)
        @s.FieldFor(m => m.Property3)
        @s.FieldFor(m => m.Property4)
        @s.FieldFor(m => m.Property5)
    }
    using (var n = f.BeginNavigation()) {
        @n.Submit("Submit")
    }
}
```

***

### Creating a form for a child property that binds back to the parent view model

# [Tag Helpers variant](#tab/example3-th)

Tag Helpers are explicitly tied to the model of the page so it's not possible to have the different model inline. However, you can include a subset of the form with a different model by using the `<form-partial />` Tag Helper. See [Partials](partials.md) for more information about the other ways you can use partials within forms.

```cshtml
<chameleon-form action='@Url.Action("PostParentViewModel")'>

    <form-partial name="_FormPartialAgainstChildProperty" for="Child" />

    <form-navigation>
        <submit-button label="Submit" add-class="parent-model" />
    </form-navigation>

</chameleon-form>
```

*_FormPartialAgainstChildProperty.cshtml*

```
@model ChildViewModel

<form-section>
    <field for="ChildField" />
    <field for="SomeEnum" />
</form-section>
```

# [HTML Helpers variant](#tab/example3-hh)

This is still easily possible, but it's just a little more verbose since it's likely to be a less common use case:

```cshtml
@using (var f = Html.For(m => m.Child, bindFieldsToParent: true).BeginChameleonForm(...)) { ... }
```

***

## Try working examples

See also the working examples in the [source](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Views/ExampleForms/ChangingContextTH.cshtml) [code](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Views/ExampleForms/ChangingContext.cshtml), which can be run so you can see how it works.
