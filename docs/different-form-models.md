# Create a form against a model type different from the page model

Similar to the examples and reasoning discussed [in the documentation for changing HTML helper model types](html-helper-context.md) it's useful to be able to create forms against an arbitrary model unrelated to the page model or create a form against a subproperty of the parent model.

This can be achieved using the default form template by using these overloads to the `BeginChameleonForms` [extension method](the-form.md):

```csharp
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
        /// <returns>A <see cref="Form{TModel}"/> object with an instance of the default form template renderer.</returns>
        public static IForm<TChildModel> BeginChameleonFormFor<TParentModel, TChildModel>(this IHtmlHelper<TParentModel> helper, Expression<Func<TParentModel, TChildModel>> formFor, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)

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
        /// <returns>A <see cref="Form{TModel}"/> object with an instance of the default form template renderer.</returns>
        public static IForm<TNewModel> BeginChameleonFormFor<TOriginalModel, TNewModel>(this IHtmlHelper<TOriginalModel> helper, TNewModel model, string action = "", FormMethod method = FormMethod.Post, HtmlAttributes htmlAttributes = null, EncType? enctype = null)
```

## Examples

The examples on the [documentation for changing HTML helper model types](html-helper-context.md) are restated below, but in terms of a ChameleonForm instead of using `Html` to output the form:

```html
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

and:

```html
@model SomeViewModel

<h1>Creating new xyz against @Model.Title</h1>
@using (var f = Html.BeginChameleonFormFor(m => m.InputModel)) {
    using (var s = f.BeginSection()) {
        @s.FieldFor(m => m.Property1)
        @s.FieldFor(m => m.Property2)
        @s.FieldFor(m => m.Property3)
        @s.FieldFor(m => m.Property4)
        @s.FieldFor(m => m.Property5)
    }
    using (var n = f.BeginNavigation()) {
        @n.Submit()
    }
}
```

See also the examples in the [source code](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Views/ExampleForms/ChangingContext.cshtml).

## Creating a form for a sub-property that binds back to the parent

This is still easily possible, but it's just a little more verbose since it's likely to be a less common use case:

```html
@using (var f = Html.For(m => m.Child, bindFieldsToParent: true).BeginChameleonForm(...)) { ... }
```
