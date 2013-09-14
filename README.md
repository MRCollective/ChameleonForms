ChameleonForms
==============

This library will shape-shift your forms experience in ASP.NET MVC.

ChameleonForms takes away the pain and repetition of building forms with ASP.NET MVC by following a philosophy of:
* **Model-driven** defaults (e.g. enum is drop-down, `[DataType(DataType.Password)]` is password textbox)
* **Extensible and flexible** core - you can extend or completely change anything you want at any layer of ChameleonForms and you can drop out to plain HTML at any point in your form for those moments where pre-prepared field types and templates just don't cut it
* **Beautiful, terse, fluent APIs** - it's a pleasure to read and write the code
* **DRY** up your forms - your forms will be quicker to write and easier to maintain and you won't get stuck writing the same form boilerplate markup form after form after form
* **Consistent** - consistency of the API and form structure within your forms and consistency across all forms in your site via templating
* **Declarative** syntax - specify how the form is structured rather than the HTML output of the form; this, in combination with the aforementioned templating means that when it comes time to change the style of your site and/or HTML structure of your forms you can do so as painlessly as possible (think about a scenario where you rapidly prototype a new site using Twitter Bootstrap and you make it big and need to change to a custom design!)

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

[Find out more](https://github.com/MRCollective/ChameleonForms/wiki/why) about why we created ChameleonForms and the advantages it gives you.

Installing ChameleonForms
-------------------------
ChameleonForms is available via [NuGet](http://www.nuget.org/packages/chameleonforms).

    Install-Package ChameleonForms

Contributors
------------

### Core Team

* [robdmoore](http://github.com/robdmoore)
* [mdaviesnet](https://github.com/mattdavies)
* [royce](https://github.com/royce)

Documentation
-------------
Check out the [wiki](http://github.com/MRCollective/ChameleonForms/wiki).

Continuous Integration
----------------------

We have a [continuous integration build](http://ci.robdmoore.id.au:8010/project.html?projectId=ChameleonForms&tab=projectOverview) that automatically builds and runs tests when we push/merge to master and generates the NuGet packages that we can publish to NuGet.org at the click of a button.

Contributing
------------
If you would like to contribute to this project then feel free to communicate with us via Twitter [@robdmoore](http://twitter.com/robdmoore) / [@mdaviesnet](http://twitter.com/mdaviesnet) or alternatively send a pull request / issue to this GitHub project.

Roadmap
-------

Feel free to check out our [Trello board](https://trello.com/b/fSuyhwNZ). It gives some idea as to the eventual goals we have for the project and the current backlog we are working against. Beware that it's pretty rough around the edges at the moment.
