![Chameleon Forms logo](https://raw.githubusercontent.com/MRCollective/ChameleonForms/master/logo.png)

# ChameleonForms

[![Build status](https://ci.appveyor.com/api/projects/status/0pnpmjpkybb99ac3?svg=true)](https://ci.appveyor.com/project/MRCollective/chameleonforms)
[![NuGet version](https://img.shields.io/nuget/vpre/ChameleonForms.svg)](https://www.nuget.org/packages/ChameleonForms)

ChameleonForms takes away the pain and repetition of building forms with ASP.NET Core MVC by following a philosophy of:

* **Model-driven** defaults (e.g. enum is drop-down or radio list, collection/array of enums is a multi-select drop-down or checkbox list, `[DataType(DataType.Password)]` is password textbox, int is number textbox with a step of 1, etc.)
* **Extend the best of ASP.NET Core MVC** - makes use of HTML generation, client validation and model binding, but makes them work the way you'd expect in more scenarios so you spend more time pumping out business value and less time fighting and patching gaps in MVC
* **DRY** up your forms - your forms will be quicker to write and easier to maintain and you won't get stuck writing the same form boilerplate markup form after form after form
* **Consistent** - consistency of the API and form structure within your forms and consistency across all forms in your site via templating
* **Declarative** syntax - specify how the form is structured rather than worrying about the boilerplate HTML markup of the form
* **Beautiful, terse, fluent APIs** - it's a pleasure to read and write the code
* **Extensible and flexible** core - you can extend or completely change anything you want at any layer of ChameleonForms and you can drop out to plain HTML at any point in your form for those moments where pre-prepared field types and templates just don't cut it

It's ideally suited for situations where you want to **quickly** build forms that are highly consistent and maintainable. If you are trying to build highly specialised forms that are individually, painstakingly crafted then that's not what this library is for. That's where it makes sense to break out your JavaScript library of choice.

[Find out more](http://chameleonforms.readthedocs.org/en/latest/why/) about why we created ChameleonForms and the advantages it gives you or check out the [documentation](http://chameleonforms.readthedocs.org/en/latest/).

## Contents

* [Getting started](#getting-started)
* [Example](#example)
* [Contributors](#contributors)
* [Support](#support)
* [Roadmap](#roadmap)

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


## Example

So, what does a ChameleonForms form look like? Check out the basic signup form example below.

### View model class

```csharp
public class SignupViewModel
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public MembershipType MembershipType { get; set; }

    [Url]
    public Uri Homepage { get; set; }
    [DataType(DataType.MultilineText)]
    public string Bio { get; set; }

    [Required]
    public bool TermsAndConditions { get; set; }
}
public enum MembershipType
{
    Standard,
    Bonze,
    Silver,
    Gold,
    Platinum
}
```

### Razor view

```csharp
@model SignupViewModel
<h1>Account signup</h1>
@using (var f = Html.BeginChameleonForm(htmlAttributes: Html.Attrs().Id("signup-form")))
{
    using (var m = f.BeginMessage(MessageType.Information, "Signup for an account"))
    {
        @m.Paragraph("Please fill in your information below to signup for an account.")
    }

    using (var s = f.BeginSection("Your details"))
    {
        @s.FieldFor(m => m.FirstName)
        @s.FieldFor(m => m.LastName)
        @s.FieldFor(m => m.DateOfBirth).WithHint("DD/MM/YYYY")
    }

    using (var s = f.BeginSection("Account details"))
    {
        @s.FieldFor(m => m.EmailAddress).WithHint("An email will be sent to this address to confirm you own it")
        @s.FieldFor(m => m.Password)
        @s.FieldFor(m => m.MembershipType)
    }

    using (var s = f.BeginSection("Additional details"))
    {
        @s.FieldFor(m => m.Bio).Rows(2).Cols(60)
        @s.FieldFor(m => m.Homepage).Placeholder("https://")
    }

    using (var m = f.BeginMessage(MessageType.Action, "Confirm the Terms & Conditions"))
    {
        @m.Paragraph(@<text>Please <a href="/terms">read the terms and conditions</a></text>)
        @f.FieldElementFor(mm => mm.TermsAndConditions).InlineLabel("I agree to the terms and conditions")
    }

    using (var n = f.BeginNavigation())
    {
        @n.Submit("Signup")
    }
}
```

### Rendered result

Here's what the rendered form looks like (using the default form template with some [light CSS applied](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/wwwroot/css/styles.css)):

![Rendered result](docs/account-signup-render.png)

Here's what the HTML looks like (using the default form template, which you can change):

```html
<h1>ChameleonForms example</h1>
<form action="" id="signup-form" method="post" novalidate="novalidate">      <div class="information_message">
          <h3>Signup for an account</h3>
          <div class="message">

<p>
    Please fill in your information below to signup for an account.
</p>          </div>
      </div>
    <fieldset>
        <legend>Your details</legend>
        <dl>
            <dt><label for="FirstName">First name</label> <em class="required">*</em></dt>
            <dd>
                <input data-val="true" data-val-required="The First name field is required." id="FirstName" name="FirstName" required="required" type="text" value="" /> <span class="field-validation-valid" data-valmsg-for="FirstName" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="LastName">Last name</label> <em class="required">*</em></dt>
            <dd>
                <input data-val="true" data-val-required="The Last name field is required." id="LastName" name="LastName" required="required" type="text" value="" /> <span class="field-validation-valid" data-valmsg-for="LastName" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="DateOfBirth">Date of birth</label> <em class="required">*</em></dt>
            <dd>
                <input aria-describedby="DateOfBirth--Hint" data-val="true" data-val-date="The field Date of birth must be a date with format d/M/yyyy." data-val-format="d/M/yyyy" data-val-required="The Date of birth field is required." id="DateOfBirth" name="DateOfBirth" required="required" type="text" value="" /><div class="hint" id="DateOfBirth--Hint">DD/MM/YYYY</div> <span class="field-validation-valid" data-valmsg-for="DateOfBirth" data-valmsg-replace="true"></span>
            </dd>
        </dl>
    </fieldset>
    <fieldset>
        <legend>Account details</legend>
        <dl>
            <dt><label for="EmailAddress">Email address</label> <em class="required">*</em></dt>
            <dd>
                <input aria-describedby="EmailAddress--Hint" data-val="true" data-val-required="The Email address field is required." id="EmailAddress" name="EmailAddress" required="required" type="email" value="" /><div class="hint" id="EmailAddress--Hint">An email will be sent to this address to confirm you own it</div> <span class="field-validation-valid" data-valmsg-for="EmailAddress" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="Password">Password</label> <em class="required">*</em></dt>
            <dd>
                <input data-val="true" data-val-required="The Password field is required." id="Password" name="Password" required="required" type="password" /> <span class="field-validation-valid" data-valmsg-for="Password" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="MembershipType">Membership type</label> <em class="required">*</em></dt>
            <dd>
                <select data-val="true" data-val-required="The Membership type field is required." id="MembershipType" name="MembershipType" required="required"><option selected="selected" value="Standard">Standard</option>
<option value="Bonze">Bonze</option>
<option value="Silver">Silver</option>
<option value="Gold">Gold</option>
<option value="Platinum">Platinum</option>
</select> <span class="field-validation-valid" data-valmsg-for="MembershipType" data-valmsg-replace="true"></span>
            </dd>
        </dl>
    </fieldset>
    <fieldset>
        <legend>Additional details</legend>
        <dl>
            <dt><label for="Bio">Bio</label></dt>
            <dd>
                <textarea cols="60" id="Bio" name="Bio" rows="2">
</textarea> <span class="field-validation-valid" data-valmsg-for="Bio" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="Homepage">Homepage</label></dt>
            <dd>
                <input id="Homepage" name="Homepage" placeholder="http://" type="url" value="" /> <span class="field-validation-valid" data-valmsg-for="Homepage" data-valmsg-replace="true"></span>
            </dd>
        </dl>
    </fieldset>
      <div class="action_message">
          <h3>Confirm the Terms & Conditions</h3>
          <div class="message">

<p>
    Please <a href="#">read the terms and conditions</a>
</p><input data-val="true" data-val-required="The Terms and conditions field is required." id="TermsAndConditions" name="TermsAndConditions" required="required" type="checkbox" value="true" /> <label for="TermsAndConditions">I agree to the terms and conditions</label>          </div>
      </div>
        <div class="form_navigation">
<button type="submit">Signup</button>        </div>
    </form>
```

### Summary

Key things to note that ChameleonForms has automatically done (with default configuration):

* HTML5 validation turned off (instead unobtrusive validation provides a better exerience)
* The form performs a POST to the current URL (i.e. self-submitting form)
* Fields are grouped into sections with fieldsets
* All boilerplate HTML surrounding a field, its label (with correct `for`), its hint (if any) and its validation message is added for you just by specifying the view model property - this means:
    * You can switch a form template once globally and all forms will automatically change
    * You can be confident all fields have labels, validation HTML etc. that are correctly hooked up and you haven't misspelt any IDs etc.
    * Similarly, you can be sure that all fields will definitely bind to the view model when posted back to the server
    * The form is much quicker to write and read since it's way terser
* The correct field types have automatically been inferred, including HTML5 field types where they are useful (e.g. not for dates since you have no control over date format):
    * `string` -> `<input type="text">`
    * `[EmailAddress]` -> `<input type="email">`
    * `[DataType(DataType.Password)]` -> `<input type="password">`
    * `MembershipType` (enum) -> `<select>` (Can easily be made a radio list with `.AsRadioList()`)
    * `[Url]` -> `<input type="url">`
    * `[DataType(DataType.MultilineText)]` -> `<textarea>`
    * `bool` -> `<input type="checkbox">`
* Required fields (either with `[Required]` or via a non-nullable value type) automatically have `data-val-required`, `required="required"` and visual required designators added to them
* Labels are automatically human readable e.g. `public string FirstName { get; set; }` became `First name` without us having to specify that
* Client-side validation and server-side validation has been added for the `DateTime` that is Format-aware (based on `[DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]`)
* Hints are automatically attached to their field for screen readers via the `aria-describedby` attribute

## Contributors

### Core Team

* [robdmoore](http://github.com/robdmoore)
* [mdaviesnet](https://github.com/mattdavies)

### Other contributors

The core team would like to thank everyone that has contributed code to this project! Check out the [contributors graph](https://github.com/MRCollective/ChameleonForms/graphs/contributors) to see them :). Thanks to [Jason Roberts](https://twitter.com/robertsjason) for the logo.

### Contributing

If you would like to contribute to this project then feel free to communicate with us via Twitter [@robdmoore](http://twitter.com/robdmoore) / [@mdaviesnet](http://twitter.com/mdaviesnet) or alternatively send a pull request / issue to this GitHub project.


### Continuous Integration

We have a [continuous integration build](https://ci.appveyor.com/project/MRCollective/chameleonforms) in AppVeyor that automatically builds and runs tests when we push/merge to master as well as all pull requests and generates the NuGet packages that we can publish to NuGet.org at the click of a button. This is implemented using the [MRCollective AppVeyor yml](https://github.com/MRCollective/AppVeyorConfig).

### Code coverage

To see code coverage, ensure you have [ReportGenerator](https://github.com/danielpalme/ReportGenerator) installed:

```
> dotnet tool install -g dotnet-reportgenerator-globaltool
```

And then run:

```
rmdir ChameleonForms.Tests/TestResults -Force
dotnet test --collect:"XPlat Code Coverage" --settings ChameleonForms.Tests/coverlet.runsettings ChameleonForms.Tests/ChameleonForms.Tests.csproj
reportgenerator "-reports:ChameleonForms.Tests/TestResults/**/coverage.cobertura.xml" "-targetdir:ChameleonForms.Tests/TestResults/report"
```

If you are using VSCode, then the first command can be achieved using the `test with coverage` task.

## Support

If you need to raise an issue or check for an existing issue, see <https://github.com/MRCollective/ChameleonForms/issues>.

## Roadmap

Some ideas for the library in the future are:

* Ability to opt-in to switch on HTML5 validation and switch off client validation
* Bootstrap 4 and Material UI support
* Tag helpers
* Blazor, Razor Pages and Carter support
* Support `IList<nullable<value type>>` (doesn't bind well in ASP.NET Core MVC - needs to be patched)
* Support localisation
* Improve configurability
