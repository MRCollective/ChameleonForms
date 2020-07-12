# Getting Started with Chameleon Forms

## Getting started

### Prerequisites

This library works against netcoreapp3.1. If you are using a different version of .NET Core or are running ASP.NET Core against Full Framework then feel free to [raise an issue](https://github.com/MRCollective/ChameleonForms/issues) to discuss opening up broader support. If you are using ASP.NET MVC 5 then check out v3.0.3 of the [NuGet package](https://www.nuget.org/packages/ChameleonForms/3.0.3) and [documentation](https://chameleonforms.readthedocs.io/en/3.0.3/).

This library works against ASP.NET Core MVC - if you want to use it for Blazor or Razor Pages then feel free to [raise an issue](https://github.com/MRCollective/ChameleonForms/issues) to discuss.

### Getting it running

1. Install the NuGet package `Install-Package ChameleonForms -pre` (v4 is currently marked beta so you need to include pre-release versions)
2. Register ChameleonForms in your `Startup.cs` file:

    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddMvc(...);
        ...
        services.AddChameleonForms();
    }
    ```

    Note: you can alter the configuration from the default, [see the docs](https://chameleonforms.readthedocs.io/en/latest/configuration/).

3. Add the following to your `_ViewImports.cshtml`:

    ```cshtml
    @using ChameleonForms;
    @using ChameleonForms.Enums;
    @using ChameleonForms.Component;
    ```

4. Create your first form, e.g.:

    `~/Controllers/MyFormController.cs`:
    ```cs
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;

    namespace MyWebApp.Controllers
    {
        public class MyFormViewModel
        {
            [Required]
            public string Name { get; set; }

            public int FavouriteNumber { get; set; }

            [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime DateOfBirth { get; set; }
        }
        public class MyFormController : Controller
        {
            public IActionResult Index()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Index(MyFormViewModel vm)
            {
                if (ModelState.IsValid)
                {
                    // Do stuff
                    return RedirectToAction("Index");
                }
                return View(vm);
            }
        }
    }
    ```

    `~/Views/MyForm/Index.cshtml`:
    ```cshtml
    @model MyWebApp.Controllers.ViewModel
    @{
        ViewData["Title"] = "My Form";
    }

    @using (var f = Html.BeginChameleonForm())
    {
        using (var s = f.BeginSection("About you!?"))
        {
            @s.FieldFor(m => m.Name)
            @s.FieldFor(m => m.FavouriteNumber)
            @s.FieldFor(m => m.DateOfBirth)
        }
        using (var n = f.BeginNavigation())
        {
            @n.Submit("Submit")
        }
    }

    @section Scripts
    {
        <partial name="_ValidationScriptsPartial" />
        @* ... or relevant equivalent *@
    }

    ```

5. Run it!
6. *(Optional)* If you want to add the additional client-side validation support in ChameleonForms (which supports both [jquery validate unobtrusive validation]() and [aspnet-validation]()) then add the following to your `_ValidationScriptsPartial.cshtml` or equivalent file:

    ```html
    <script src="~/lib/chameleonforms/unobtrusive-date-validation.chameleonforms.js" asp-append-version="true"></script>
    ```

7. *(Optional)* If you are using Twitter Bootstrap 3 then add the following to your `_ValidationScriptsPartial.cshtml` (which only supports jquery validate unobtrusive validation for now):

    ```html
    <script src="~/lib/chameleonforms/unobtrusive-twitterbootstrap3-validation.chameleonforms.js" asp-append-version="true"></script>
    ```

    And add the following to your `_Layout.cshtml` or equivalent file:

    ```html
    <link href="~/lib/chameleonforms/chameleonforms-twitterbootstrap3.css" rel="stylesheet" type="text/css" asp-append-version="true" />
    ```


## Show me a basic ChameleonForms example next to its ASP.NET Core MVC counterpart!

Say you had the following view model:

```csharp
    public class BasicViewModel
    {
        [Required]
        public string RequiredString { get; set; }

        public SomeEnum SomeEnum { get; set; }

        public bool SomeCheckbox { get; set; }
    }
```

And assuming for a moment you used definition lists to wrap your HTML fields then you might end up with something like this in your Razor view:

```html
@using (Html.BeginForm())
{
    <fieldset>
        <legend>A form</legend>
        <dl>
            <dt>@Html.LabelFor(m => m.RequiredString, "Some string")</dt>
            <dd>@Html.TextBoxFor(m => m.RequiredString) @Html.ValidationMessageFor(m => m.RequiredString)</dd>
            <dt>@Html.LabelFor(m => m.SomeEnum)</dt>
            <dd>@Html.DropDownListFor(m => m.SomeEnum, Enum.GetNames(typeof(SomeEnum)).Select(x => new SelectListItem {Text = ((SomeEnum)Enum.Parse(typeof(SomeEnum), x)).Humanize(), Value = x})) @Html.ValidationMessageFor(m => m.SomeEnum)</dd>
            <dt>@Html.LabelFor(m => m.SomeCheckbox)</dt>
            <dd>@Html.CheckBoxFor(m => m.SomeCheckbox) @Html.LabelFor(m => m.SomeCheckbox, "Are you sure?") @Html.ValidationMessageFor(m => m.SomeCheckbox)</dd>
        </dl>
    </fieldset>
    <div class="form_navigation">
        <input type="submit" value="Submit" />
    </div>
}
```

The equivalent of this form with out-of-the-box ChameleonForms functionality is:

```cshtml
@using (var f = Html.BeginChameleonForm()) {
    using (var s = f.BeginSection("A form")) {
        @s.FieldFor(m => m.RequiredString).Label("Some string")
        @s.FieldFor(m => m.SomeEnum)
        @s.FieldFor(m => m.SomeCheckbox).InlineLabel("Are you sure?")
    }
    using (var n = f.BeginNavigation()) {
        @n.Submit("Submit")
    }
}
```

## What does ChameleonForms do for me?

Chameleon Forms provides an object hierarchy that allows you to declaratively specify the structure of your form. From there:

* It will output the boilerplate template of your form by way of a form template
* It will discern a number of defaults about each field based on inspecting the model metadata for each property
* It will allow you to tweak individual fields by chaining methods using the fluent api off of each field (and some other elements such as submit buttons) declaration
* If gives you the freedom to break out into HTML/Razor anywhere in the form when the template / built-in structures don't meet your needs
* It gives you the ability to apply global conventions across your forms

It makes use of convention over configuration, `using` statements and an opinionated structure (that is easy enough to opt out of or create your own structure if you like) to make each form consistent and demonstrating a minimum of repetition.

## How are ChameleonForms forms structured?

There is a general structure that ChameleonForms encourages with the out-of-the-box setup that can be described by the following diagrams. In reality you can use any structure you like and you can break out into plain old HTML any time you need to, but this explains the default structure that ChameleonForms empowers you to specify by default.

### Form

At the top level is the Form - a Form can have any number of Form Components underneath it or, for ultimate flexibility, the separate components that make up a Field on an ad hoc basis(using the Field Element, the Field Label and the Field Validation HTML).

![Form contains any number of Form Components, Field Elements, Field Labels and Field Validation HTML](form.png)

The Form Components that come with ChameleonForms out of the box are:

* Message
* Section
* Navigation

To create a Form simply use the `BeginChameleonForms` extension method off of the Html helper:

```cshtml
@using (var f = Html.BeginChameleonForm()) {
    @* The form ... *@
}
```

Random Field Elements, Field Labels and Field Validation HTML that don't fit in to a Section (see below) can be output from the Form object anywhere within your form like so:

```cshtml
<p>@f.LabelFor(m => m.SomeCheckbox).Label("Hello!") @f.FieldFor(m => m.SomeCheckbox) @f.ValidationMessageFor(m => m.SomeCheckbox)</p>
```

### Message

At any point in the form you can create a Message component - a Message always has a type as well as a Heading and inside of the message you can add any content you like, but ChameleonForms gives you the option of easily specifying Message Paragraph's - the HTML for which is defined in your form template.

![Message components have a Heading and any number of Paragraphs](message.png)

The message types available are:

* Action - User action required
* Success - Action successful
* Failure - Action failed
* Information - Informational message
* Warning - Warning message

You can output different HTML in your form template depending on the type of message (e.g. different class or completely different HTML structure).

To create a Message simply use the `BeginMessage` extension method off of the Form object:

```cshtml
@using (var m = f.BeginMessage(MessageType.Success, "Submission successful")) {
    @m.Paragraph("Some sort of success message")
    @* Other Paragraph's or any HTML at all really ... *@
}
```

### Section

A Section component holds a set of Fields (see below for definition of Field) or nested sections (to no more than one level deep). A Section will start with a Heading. The default form template that comes with Chameleon Forms defines a top-level section as a `fieldset`.

![Section components have a Heading followed by any number of Fields or single level deep nested Sections](section.png)

To create a Section simply use the `BeginSection` extension method off of the Form object (or off of the Section object to create a nested one):

```cshtml
@using (var s = f.BeginSection("Basic information")) {
    using (var ss = s.BeginSection("Nested section")) {
        @* Fields... *@
    }
    @* Fields... *@
}
```

### Field

A Field is a single data collection unit within a Section and comprises of an Element, a Label, Validation HTML and a Field Configuration.

Fields can have other Fields nested within them (to one level deep).

To create a Field simply use the `FieldFor` extension method off of the Section object or the `BeginFieldFor` extension method off of the Section object to start a Field with nested Fields:

```cshtml
@s.FieldFor(m => m.SomeField).FieldConfigurationMethodsCanBeChainedOffOfTheEnd()
@using (var ff = s.BeginFieldFor(m => m.AnotherField, Field.Configure().FieldConfigurationMethodsCanBeChainedHere()) {
    @ff.FieldFor(m => m.ChildField)
}
```

### Navigation

A Navigation component will usually be placed at the end of the form (although there is nothing stopping you placing it elsewhere or even multiple times on the form - e.g. top and bottom). The Navigation component allows you to easily create Submit buttons, Reset buttons and normal Buttons.

![Navigation components can have Submit buttons, Reset buttons and normal Buttons](navigation.png)

To create a Navigation simply use the `BeginNavigation` extension method off of the Form object:

```cshtml
@using (var n = f.BeginNavigation()) {
    @n.Submit("Submit").ChainHtmlAttributesOffOfTheEnd()
    @n.Reset("Reset").ChainHtmlAttributesOffOfTheEnd()
    @n.Button("A button").ChainHtmlAttributesOffOfTheEnd()
}
```

## What terminology is used in ChameleonForms?

Some of the terminology around the structure of ChameleonForms forms are defined above, but following is a more comprehensive list of terms that the library uses:

* Form - The container of a single form
* Form Component - Some sort of container nested within a Form
* Heading - A title given to a Form Component (not all Form Components have one though)
* Message - A message to show the user
* Message Paragraph - A discrete part of a message to show the user
* Section - A grouping for a set of Fields
* Field - A single data collection unit
* Field Element - The HTML that makes up a control to accept data from the user
* Field Label - Text that describes a Field Element to a user (and is linked to a Field Element)
* Field Validation HTML - Markup that acts as a placeholder to display any validation messages for a particular Field Element
* Field Configuration - The configuration for a particular Field, Field Element and/or Field Label
* Navigation - A grouping for a set of Navigation elements
* Navigation Submit - A button that will submit the form
* Navigation Reset - A button that will reset the form to it's initial state
* Navigation Button - A button that has a user-defined behaviour
* Field Generator - A class that generates HTML for a single Field
* Field Generator Handler - A class that generates HTML for a particular type of Field Element
* Form Template - A class that defines the HTML boilerplate to render Forms, Form Components, Fields and Navigation elements
* HTML Attributes - A class that defines a set of HTML attributes to apply to a HTML element
