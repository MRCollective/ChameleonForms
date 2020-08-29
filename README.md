 ![Chameleon Forms logo](https://raw.githubusercontent.com/MRCollective/ChameleonForms/master/logo.png)

# ChameleonForms

[![Build status](https://ci.appveyor.com/api/projects/status/0pnpmjpkybb99ac3?svg=true)](https://ci.appveyor.com/project/MRCollective/chameleonforms)
[![NuGet version](https://img.shields.io/nuget/vpre/ChameleonForms.svg)](https://www.nuget.org/packages/ChameleonForms)
[![docs | up to date](https://img.shields.io/badge/docs-up%20to%20date-green)](https://mrcollective.github.io/ChameleonForms/)

ChameleonForms takes away the pain and repetition of building forms with ASP.NET Core MVC by following a philosophy of:

* **Model-driven** defaults - Spend less time with tedious reptition by letting your view models do the hard work for you. (e.g. enum is drop-down or radio list, collection/array of enums is a multi-select drop-down or checkbox list, `[DataType(DataType.Password)]` is password textbox, `int` is a `number` textbox with a `step` of `1`, etc.).
* **Extend the best of ASP.NET Core MVC** - makes use of HTML generation, client validation and model binding, but makes them work the way you'd expect in more scenarios so you spend more time pumping out business value and less time fighting and patching gaps in MVC.
* **DRY** up your forms - your forms will be quicker to write and easier to maintain and you won't get stuck writing the same form boilerplate markup form after form after form.
* **Consistent** - consistency of the (ChameleonForms) API and form structure within your forms and consistency across all forms in your site via templating. This will make your forms easier to maintain and have a better user experience.
* **Declarative** syntax - specify how the form is structured rather than worrying about the boilerplate HTML markup of the form.
* **Beautiful, terse, fluent APIs** - it's a pleasure to read and write the code.
* **Extensible and flexible** core - you can extend or completely change anything you want at any layer of ChameleonForms and you can drop out to plain HTML at any point in your form for those moments where pre-prepared field types and templates just don't cut it.

It's ideally suited for situations where you want to **quickly** build forms that are highly consistent and maintainable. If you are trying to build highly specialised forms that are individually, painstakingly crafted then that's not what this library is for. That's where it makes sense to break out your JavaScript library of choice.

[Find out more](https://mrcollective.github.io/ChameleonForms/docs/why.html) about why we created ChameleonForms and the advantages it gives you or check out the [documentation](https://mrcollective.github.io/ChameleonForms/).

## Contents

* [Getting started](#getting-started)
* [Contributors](#contributors)
* [Support](#support)
* [Roadmap](#roadmap)

## Getting started

### Prerequisites

This library works against netcoreapp3.1. If you are using a different version of .NET Core or are running ASP.NET Core against Full Framework then feel free to [raise an issue](https://github.com/MRCollective/ChameleonForms/issues) to discuss opening up broader support. If you are using ASP.NET MVC 5 then check out v3.0.3 of the [NuGet package](https://www.nuget.org/packages/ChameleonForms/3.0.3) and [documentation](https://chameleonforms.readthedocs.io/).

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

    Note: you can alter the configuration from the default, [see the docs](https://mrcollective.github.io/ChameleonForms/docs/configuration.html).

3. Add the following to your `_ViewImports.cshtml`:

    ```cshtml
    @using ChameleonForms;
    @using ChameleonForms.Enums;
    @using ChameleonForms.Component;

    @addTagHelper ChameleonForms.TagHelpers.*, ChameleonForms
    
    @* optional: *@
    @addTagHelper ChameleonForms.Templates.TwitterBootstrap3.*, ChameleonForms
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

    You have two options for your view - [tag helper syntax](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-3.1) or the more traditional [HTML helper syntax](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-3.1#tag-helpers-compared-to-html-helpers).
    
    **Tag Helpers variant**

    ```html
    @model MyWebApp.Controllers.ViewModel
    @{
        ViewData["Title"] = "My Form";
    }

    <chameleon-form>
        <form-section heading="About you!?">
            <field for="Name" />
            <field for="FavouriteNumber" />
            <field for="DateOfBirth" />
        </form-section>
        <form-navigation>
            <submit-button label="Submit" />
        </form-navigation>
    </chameleon-form>

    @section Scripts
    {
        <partial name="_ValidationScriptsPartial" />
        @* ... or relevant equivalent *@
    }

    ```

    **HTML Helpers variant**

    ```cs
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

### Documentation

We are using [DocFX](https://dotnet.github.io/docfx/) to generate the documentation. The documentation consists of three parts:

1. The shiny homepage
2. The main documentation area, which is generated off a bunch of Markdown files
3. The API documentation, which is auto-generated from the xmldoc comments in the source code

To see the documentation:

1. `choco install docfx` (Windows); `brew install docfx` (Mac); [Other](https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html#2-use-docfx-as-a-command-line-tool)
2. `> docfx docfx.json`
3. The documentation site will be running at <http://localhost:8080/>

The key files to look for to modify the documentation are:

1. `docfx.json` - DocFX config
2. `toc.yml` - Table of contents
3. `index.md` - Homepage
4. `docs/templates/chameleonforms/*.*` - Template
5. `docs/*.*` - Documentation files

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

To get an idea of where ChameleonForms is heading in the future check out the [roadmap in the documentation](https://mrcollective.github.io/ChameleonForms/docs/index.html#roadmap).
