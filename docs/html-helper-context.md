# Change the model type for HTML Helper for portions of your page

Sometimes it's useful to include multiple forms in a page or use other HTML Helper functionality against a different model type than the parent model of the page. This different model type might be a completely arbitrary model unrelated to the page model type or it may be a sub-property of the parent model.

Consider this example:

```html
@model LoginViewModel

<h1>Login</h1>
@using (Html.BeginForm()) {
    <p>@Html.LabelFor(m => m.Username) @Html.TextBoxFor(m => m.Username) @Html.ValidationMessageFor(m => m.Username)</p>
    <p>@Html.LabelFor(m => m.Password) @Html.PasswordFor(m => m.Password) @Html.ValidationMessageFor(m => m.Password)</p>
    <p><button type="submit">Login</button></p>
}

<h2>Don't have an account?</h2>
<p>You can easily create a new account in seconds - just start by selecting a username below.</p>
@using (Html.BeginForm("Step1", "Signup") {
    <p><input type="text" name="Username" placeholder="Enter your preferred username"></p>
    <p><button type="submit">Begin signup - check if my username is available &raquo;</button></p>
}
```

In the above example the second form actually submits to a different controller and action using a completely different and unrelated view model to the page view model (`LoginViewModel`). In the above example the field name of the field (`Username`) was hardcoded - if that doesn't match up to the property name in the view model that is in `SignupController.Step1()` then it won't be bound and there will be a runtime error. It's impossible to use the view model type to get type safety without dirtying up the `LoginViewModel` to include it and worse still that would requie you to bind to the `LoginViewModel` in the `SignupController` (even though it has nothing to do with logging in) since the binding names wouldn't match otherwise.

Another example - if you decide to make all form fields part of an "input model":

```html
@model SomeViewModel

<h1>Creating new xyz against @Model.Title</h1>
@using (Html.BeginForm()) {
    <p>@Html.LabelFor(m => m.InputModel.Property1) @Html.TextBoxFor(m => m.InputModel.Property1) @Html.ValidationMessageFor(m => m.InputModel.Property1)</p>
    <p>@Html.LabelFor(m => m.InputModel.Property2) @Html.TextBoxFor(m => m.InputModel.Property2) @Html.ValidationMessageFor(m => m.InputModel.Property2)</p>
    <p>@Html.LabelFor(m => m.InputModel.Property3) @Html.TextBoxFor(m => m.InputModel.Property3) @Html.ValidationMessageFor(m => m.InputModel.Property3)</p>
    <p>@Html.LabelFor(m => m.InputModel.Property4) @Html.TextBoxFor(m => m.InputModel.Property4) @Html.ValidationMessageFor(m => m.InputModel.Property4)</p>
    <p>@Html.LabelFor(m => m.InputModel.Property5) @Html.TextBoxFor(m => m.InputModel.Property5) @Html.ValidationMessageFor(m => m.InputModel.Property5)</p>
    <p><button type="submit">Submit</button></p>
}
```

There are two problems here - firstly, the `.InputModel.` is very repetitive and adds a lot of noise and secondly, the post action forces you to bind to the `SomeViewModel` model type, which means technically, you need to remember to mark `Title` and other read-only values with the `[ReadOnly(true)]` attribute to prevent an [over-posting / mass-assignment vulnerability](https://en.wikipedia.org/wiki/Mass_assignment_vulnerability). It would be much simpler if you could specify that the form is against the `InputModel` sub-property of the main form and bind just the `InputModel` on the round-trip back to the server.

## HtmlHelper<TModel>.For extensions

ChameleonForms gives you two extension methods on the `HtmlHelper<TModel>` class that allow you to solve the above problems in a clean way:

```csharp
/// <summary>
/// Creates a HTML helper from a parent model to use a sub-property as it's model.
/// </summary>
/// <typeparam name="TParentModel">The model of the parent type</typeparam>
/// <typeparam name="TChildModel">The model of the sub-property to use</typeparam>
/// <param name="helper">The parent HTML helper</param>
/// <param name="propertyFor">The sub-property to use</param>
/// <param name="bindFieldsToParent">Whether to set field names to bind to the parent model type (true) or the sub-property type (false)</param>
/// <returns>A HTML helper against the sub-property</returns>
public static DisposableHtmlHelper<TChildModel> For<TParentModel, TChildModel>(this HtmlHelper<TParentModel> helper,
    Expression<Func<TParentModel, TChildModel>> propertyFor, bool bindFieldsToParent)

/// <summary>
/// Creates a HTML helper based on another HTML helper against a different model type.
/// </summary>
/// <typeparam name="TModel">The model type to create a helper for</typeparam>
/// <param name="htmlHelper">The original HTML helper</param>
/// <param name="model">An instance of the model type to use as the model</param>
/// <param name="htmlFieldPrefix">A prefix value to use for field names</param>
/// <returns>The HTML helper against the other model type</returns>
public static DisposableHtmlHelper<TModel> For<TModel>(this HtmlHelper htmlHelper,
    TModel model = default(TModel), string htmlFieldPrefix = null)
```

The first one allows you express an expression to identify a sub-property of the parent model of the page to create a new HTML helper against. The second allows you to specify an arbitrary type to create a new HTML helperfor (with optional instance to use as the model). The former allows you to control whether you want any form fields to bind back to the parent view model or directly to the child and the second allows you to add a prefix that will be used for binding names for fields.

You'll notice the return type is `DisposableHtmlHelper<TModel>` rather than `HtmlHelper<TModel>` - this class is a wrapper around `HtmlHelper<TModel>` that also implements `IDisposable` (the `Dispose()` method that is introduced does nothing) as a convenience so you can create a HTML helper around a section of your page for readability reasons.

## Examples

The previous two examples on this page could be re-written using the extension methods to become:

```html
@model LoginViewModel

<h1>Login</h1>
@using (Html.BeginForm()) {
    <p>@Html.LabelFor(m => m.Username) @Html.TextBoxFor(m => m.Username) @Html.ValidationMessageFor(m => m.Username)</p>
    <p>@Html.LabelFor(m => m.Password) @Html.PasswordFor(m => m.Password) @Html.ValidationMessageFor(m => m.Password)</p>
    <p><button type="submit">Login</button></p>
}

<h2>Don't have an account?</h2>
<p>You can easily create a new account in seconds - just start by selecting a username below.</p>
@using (var signupHtml = Html.For<SignupStep1ViewModel>()) {
    @using (signupHtml.BeginForm("Step1", "Signup") {
        <p>@signupHtml.TextBoxFor(m => m.Username, new {placeholder = "Enter your preferred username"})</p>
        <p><button type="submit">Begin signup - check if my username is available &raquo;</button></p>
    }
}
```

and:

```html
@model SomeViewModel

<h1>Creating new xyz against @Model.Title</h1>
@using (var html = Html.For(m => m.InputModel, bindToParent: false)) {
    @using (html.BeginForm()) {
        <p>@html.LabelFor(m => m.Property1) @html.TextBoxFor(m => m.Property1) @html.ValidationMessageFor(m => m.Property1)</p>
        <p>@html.LabelFor(m => m.Property2) @html.TextBoxFor(m => m.Property2) @html.ValidationMessageFor(m => m.Property2)</p>
        <p>@html.LabelFor(m => m.Property3) @html.TextBoxFor(m => m.Property3) @html.ValidationMessageFor(m => m.Property3)</p>
        <p>@html.LabelFor(m => m.Property4) @html.TextBoxFor(m => m.Property4) @html.ValidationMessageFor(m => m.Property4)</p>
        <p>@html.LabelFor(m => m.Property5) @html.TextBoxFor(m => m.Property5) @html.ValidationMessageFor(m => m.Property5)</p>
        <p><button type="submit">Submit</button></p>
    }
}
```

In the second example you would change the controller action to take the type of `SomeViewModel.InputModel` rather than `SomeViewModel`.

## Html helper properties

There are a few things to note about the HTML helper that is created:

* The request context, route collection and writer are all the same as the original HTML helper
* The HTML prefix will be the same as the original HTML helper (plus any given prefix, or the sub property if binding a sub-property to the parent)
* View data / view bag will be a copy of the original HTML helper at the time of calling the `For` extension method - any changes made to it will not propogate between the two helpers

## ChameleonForms forms

ChameleonForms uses these extensions internally to allow you to [create a form against an model type different from the page model](different-form-models.md).
