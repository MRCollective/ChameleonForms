# Use partial views for repeated or abstracted form areas

You can use partial views to abstract a complex part of your form, reuse common parts of your form or to [change the model type when using tag helpers](different-form-models.md#tag-helpers).

Given ChameleonForms is type-safe against the view's model type, when including a partial and changing the model type some care needes to be taken to ensure the correct ChameleonForms classes are being used. There are a lot of smarts built into ChameleonForms to auto-detect this where possible. The different scenarios and how to invoke them are listed below.

Tag Helpers don't allow you to change a model within a page so for any situation where you want to change the model type (temporarily or for the whole form) you need to use partials. See below for examples.

The best way to see how this works is by looking at the output of the partials acceptance tests ([tag helpers](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.AcceptanceTests/IntegrationTests/PartialForTests.Should_render_correctly_when_used_via_form_or_section_and_when_used_for_top_level_property_or_sub_property_via_tag_helpers.approved.html), [HTML helpers](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.AcceptanceTests/IntegrationTests/PartialForTests.Should_render_correctly_when_used_via_form_or_section_and_when_used_for_top_level_property_or_sub_property.approved.html)) or by looking at the changing context example page for [tag helpers](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Views/ExampleForms/ChangingContextTH.cshtml).

## Partial with same model as the parent

You can include the partial view using built-in ASP.NET MVC functionality:

# [Tag Helpers variant](#tab/same-th)

```cshtml
    <chameleon-form ...>
        ...
        <partial name="_PartialWithSameModelAsParent" />
        ...
    </chameleon-form>
```

# [HTML Helpers variant](#tab/same-hh)

```cshtml
    @using (var f = Html.BeginChameleonForm(...))
    {
        ...
        @await Html.PartialAsync("_PartialWithSameModelAsParent")
        ...
    }
```

***

## Partial has child property as model, but binds against parent

You need to ensure that the ChameleonForms types get explicitly converted to the child model within the partial, while still binding against the parent.

# [Tag Helpers variant](#tab/child-in-parent-th)

When using tag helpers you can use the provided `<form-partial />` tag helper:

```cshtml
    <chameleon-form ...>
        ...
        <form-partial for="ChildProperty" name="_PartialWithChildModelBindingToParent" />
        ...
    </chameleon-form>
```

# [HTML Helpers variant](#tab/child-in-parent-hh)

When using HTML helpers you can use the `PartialForAsync` extension method on either the `Form` or `Section` (whichever is in context):

```cshtml
    @using (var f = Html.BeginChameleonForm(...))
    {
        ...
        @* When not in a form section: *@
        @(await f.PartialForAsync(m => m.ChildProperty, "_PartialWithChildModelBindingToParent"))
        ...
        using (var s = f.BeginSection(...))
        {
            ...
            @* When in a form section: *@
            @(await s.PartialForAsync(m => m.ChildProperty, "_PartialWithChildModelBindingToParent"))
            ...
        }
        ...
    }
```

When using HTML helpers, you can also do this [without using partials](different-form-models.md#creating-a-form-for-a-child-property-that-binds-back-to-the-parent-view-model).

***

## Partial has child property as model and binds against it

In this case you can simply invoke a partial using built-in ASP.NET MVC Core functionality passing in the child property as the model of the partial.

# [Tag Helpers variant](#tab/child-th)

```cshtml
    <partial name="_PartialWithChildAsModel" model="Model?.ChildProperty" />
```

# [HTML Helpers variant](#tab/child-hh)

```cshtml
    @await Html.PartialAsync("_PartialWithChildAsModel", Model?.ChildProperty)
```

When using HTML helpers, you can also do this [without using partials](different-form-models.md#binding-against-a-child-property).

***

## Partial has a different model entirely from the parent

In this case you can simply invoke a partial using built-in ASP.NET MVC Core functionality passing in the other model as the model of the partial.

# [Tag Helpers variant](#tab/other-th)

```cshtml
    <partial name="_PartialWithOtherModel" model="new OtherModel()" />
```

# [HTML Helpers variant](#tab/other-hh)

```cshtml
    @await Html.PartialAsync("_PartialWithOtherModel", new OtherModel())
```

When using HTML helpers, you can also do this [without using partials](different-form-models.md#using-a-different-view-model).

***

## Accessing the current form / form section / form field in a partial

When inside a partial view you can use the following methods off of `this`:

* `Html.IsInChameleonForm()` - returns whether you are currently inside of a ChameleonForms form.
* `Html.GetChameleonForm()` - returns the current ChameleonForms form, otherwise it throws an Exception.
* `Html.IsInChameleonFormsSection()` - returns whether you are currently inside of a ChameleonForms Section.
* `Html.GetChameleonFormsSection()` - returns the current ChameleonForms Section instance if there is one, otherwise it throws an Exception.
* `Html.IsInChameleonFormsField()` - returns whether you are currently inside of a ChameleonForms Field.
* `Html.GetChameleonFormsField()` - returns the current ChameleonForms Field instance if there is one, otherwise it throws an Exception.
* `Html.IsInChameleonFormsMessage()` - returns whether you are currently inside of a ChameleonForms Message.
* `Html.GetChameleonFormsMessage()` - returns the current ChameleonForms Message instance if there is one, otherwise it throws an Exception.
* `Html.IsInChameleonFormsNavigation()` - returns whether you are currently inside of a ChameleonForms Navigation.
* `Html.GetChameleonFormsNavigation()` - returns the current ChameleonForms Navigation instance if there is one, otherwise it throws an Exception.

This also works in the parent view so this functionality actually also lets you switch between tag helper syntax and HTML helper syntax on the fly. You can also switch from HTML helper syntax to tag helper syntax by including the tag helpers - they will automatically pick up the ambient context for you.
