# ChameleonForms Documentation

## Overview
ChameleonForms takes away the pain and repetition of building forms with ASP.NET MVC by following a philosophy of:
* **Model-driven** defaults (e.g. enum is drop-down, `[DataType(DataType.Password)]` is password textbox)
* **DRY** up your forms - your forms will be quicker to write and easier to maintain and you won't get stuck writing the same form boilerplate markup form after form after form
* **Consistent** - consistency of the API and form structure within your forms and consistency across all forms in your site via templating
* **Declarative** syntax - specify how the form is structured rather than worrying about the boilerplate HTML markup of the form; this has the same beneficial effect as separating HTML markup and CSS
* **Beautiful, terse, fluent APIs** - it's a pleasure to read and write the code
* **Extensible and flexible** core - you can extend or completely change anything you want at any layer of ChameleonForms and you can drop out to plain HTML at any point in your form for those moments where pre-prepared field types and templates just don't cut it

Twitter Bootstrap 3
-------------------

The ASP.NET MVC application templates now come powered by Bootstrap by default. Have you noticed the gross boilerplate HTML that is repeated again and again in every view though?! Ugh!

ChameleonForms has got you covered; it has built-in support for building forms using **[Twitter Bootstrap 3](wiki/bootstrap-template)**!

![Example of the code and display of a Chameleon-powered Bootstrap form](wiki/bootstrap-example-banner.png)

Note: The example above shows a vertical layout, but the ASP.NET MVC templates use the horizontal layout. See the [Twitter Bootstrap 3 template documentation page](wiki/bootstrap-template#horizontal-and-inline-forms) to see more about the horizontal form.

What does a ChameleonForms form look like?
------------------------------------------
So what does a ChameleonForms form look like? Here is a (very) basic example:

```c#
@using (var f = Html.BeginChameleonForm()) {
    using (var s = f.BeginSection("Signup for an account")) {
        @s.FieldFor(m => m.FirstName)
        @s.FieldFor(m => m.LastName)
        @s.FieldFor(m => m.Mobile).Placeholder("04XX XXX XXX")
        @s.FieldFor(m => m.LicenseAgreement).InlineLabel("I agree to the terms and conditions")
    }
    using (var n = f.BeginNavigation()) {
        @n.Submit("Create")
    }
}
```

We expect that you know how to use ASP.NET MVC's form generation, model-binding and validation support to be able to effectively use and understand this library. If you need a hand getting started with that knowledge then [see below](wiki/#aspnet-mvc-posts).

## Philosophy
* [Why is ChameleonForms needed](wiki/why) - A rant about building forms and why ChameleonForms removes a lot of the pain

## Basic usage
* [Getting started](wiki/getting-started) - What does ChameleonForms do for me? How are ChameleonForms forms structured? What terminology is used in ChameleonForms? Namespaces in `Views\web.config`.
* [Comparison](wiki/comparison) - See an example of a ChameleonForms form versus a traditional MVC form (HTML Helpers and Editor Templates)
* [Changing to Twitter Bootstrap 3 template](wiki/bootstrap-template) - Changing from the default template to the Twitter Bootstrap template
* [Automatically sentence case form labels](wiki/auto-sentence-case) - How to automatically sentence case your form labels without having to annotate them with `[DisplayName]` by adding a single line to `global.asax`
* [Field Configuration](wiki/field-configuration) - An overview of the common options available to configure a form field via the `IFieldConfiguration` interface
* [HTML Attributes](wiki/html-attributes) - An overview of how to define HTML attributes using the `HtmlAttributes` class

## Form structure
Examples for generating a form and each type of default component within the form. The following pages show both the ChameleonForms syntax, as well as the default generated HTML (which you can easily override to suit your own needs).
* [Form](wiki/the-form) - How to output and configure the containing form
* [Message](wiki/the-message) - How to output and configure a message
* [Section](wiki/the-section) - How to output and configure a form section
* [Navigation](wiki/the-navigation) - How to output and configure a form navigation area and add buttons to it
* [Field](wiki/field) - How to output and configure templated fields
    * [Field Element](wiki/field-element) - How to output the HTML for a field
    * [Field Label](wiki/field-label) - How to output and configure field labels
    * [Field Validation HTML](wiki/field-validation-html) - How to output validation messages for a field

## Field types
* [Boolean fields](wiki/boolean) - Display booleans as a single checkbox, a select-list or a list of radio checkboxes
* [DateTime fields](wiki/datetime) - Display DateTimes as a text box including model binding and client-side validation that respects `[DisplayFormat]`
    * [Client-side validation of DateTime fields](wiki/datetime-client-side-validation) - How to use `jquery.validate.unobtrusive.chameleon.js`
* [Enum fields](wiki/enum) - Display enums as drop-downs or a list of radio buttons
* [Multiple-select enum fields](wiki/multiple-enum) - Display enums as multi-select drop-downs or a list of checkboxes
* [List fields](wiki/list) - Display drop-downs or lists of radio buttons to allow users to select an item from a list
* [Multiple-select list fields](wiki/multiple-list) - Display multi-select drop-downs or lists of checkboxes to allow users to select multiple items from a list
* [Textarea fields](wiki/textarea) - Display textarea fields
* [File upload fields](wiki/file-upload) - Display file-upload fields
* [Password fields](wiki/password) - Display password fields
* [Default (`<input type="text" />`) fields](wiki/default-fields)

## Advanced usage
* [Use partial views for repeated or abstracted form areas](wiki/partials)
* [Using different form templates](wiki/form-templates)
* [Creating custom form templates](wiki/custom-template)
* [Extending the field configuration](wiki/extending-field-configuration)
* [Extending the form components](wiki/extending-form-components)
* [Creating and using a custom field generator](wiki/custom-field-generator)
* [Creating and using custom field generator handlers](wiki/custom-field-generator-handlers)

## ASP.NET MVC Posts

If you need a hand getting started with ASP.NET MVC's form generation, model-binding and validation support then see the below.

* [Building ASP.NET MVC Forms with Razor (ASP.NET MVC Foundations Series)](http://blog.michaelckennedy.net/2012/01/20/building-asp-net-mvc-forms-with-razor/)

## Contributing
If you would like to contribute to this project then feel free to communicate with us via Twitter [@robdmoore](http://twitter.com/robdmoore) / [@mdaviesnet](http://twitter.com/mdaviesnet) or alternatively send a pull request / issue to this GitHub project.

## Roadmap

Feel free to check out our [Trello board](https://trello.com/board/chameleonforms/504df3392ad570121c36c3f7). It gives some idea as to the eventual goals we have for the project and the current backlog we are working against. Beware that it's pretty rough around the edges at the moment.