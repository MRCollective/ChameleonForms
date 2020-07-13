<div class="row">
<div class="col-md-4">
    <h2>Model-driven forms</h2>
    <p>Spend less time with tedious reptition by letting your view models do the hard work for you.</p>
</div>
<div class="col-md-8 equal-heights">

# [View Model](#tab/model-1)
```c#
public class SignupViewModel
{
    [Required]
    public MembershipClass? MembershipType { get; set; }
}

public enum MembershipClass
{
    Standard,
    Bonze,
    Silver,
    Gold,
    Platinum
}
```

# [View](#tab/model-2)

**Tag helpers variant**

```html
<form-section heading="Form section">
    <field for="MembershipType" />
</form-section>
```

**HTML helpers variant**

```cshtml
@using (var s = f.BeginSection("Form section")) {
    @s.FieldFor(m => m.MembershipType)
}
```

# [HTML output](#tab/model-3)
```html
<fieldset>
    <legend>Form section</legend>
    <dl>
        <dt><label for="MembershipType">Membership type</label> <em class="required">*</em></dt>
        <dd>
            <select data-val="true" data-val-required="The Membership type field is required." id="MembershipType" name="MembershipType" required="required">
                <option selected="selected" value="Standard">Standard</option>
                <option value="Bonze">Bonze</option>
                <option value="Silver">Silver</option>
                <option value="Gold">Gold</option>
                <option value="Platinum">Platinum</option>
            </select>
            <span class="field-validation-valid" data-valmsg-for="MembershipType" data-valmsg-replace="true"></span>
        </dd>
    </dl>
</fieldset>
```

# [Notes](#tab/model-4)

Here are the things that ChameleonForms has done for us based on the model:

* The field label is based on the field name, but humanised (e.g. `MembershipType` -> `Membership type`)
* The `[Required]` attribute results in a required designator (`<em class="required">*</em>`), required attribute on the field (`required="required"`) required unobtrusive client-side validation attributes (`data-val` and `data-val-required`)
* The enum type automatically results in a `<select>` with the different enum values translated to `<option>`'s

# [Further reading](#tab/model-5)

Other model-driven form features you can explore:

* [Getting started](docs/getting-started.md)
* Inference from model type to output [Boolean fields](docs/boolean.md), [DateTime fields](docs/datetime.md), [Enum fields](docs/enum.md), [List fields](docs/list.md), [File upload fields](docs/file-upload.md) and [Number fields](docs/number.md)
* Multiple-select: [Flags enum fields](docs/flags-enum.md), [Multiple-select enum fields](docs/multiple-enum.md) and [Multiple-select list fields](docs/multiple-list.md)
* Inference from model property attributes to output [Textarea fields](docs/textarea.md), [Password fields](docs/password.md), [Email fields](docs/email.md) and [Uri fields](docs/uri.md) as well as supporting controlling the client-side and server-side validation of [DateTime fields](docs/datetime.md)
* Client-side and server-side validation is given to us from the model metadata based on the [built-in ASP.NET Core MVC features](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-3.1).
* The HTML that is rendered will always bind correctly to the view model on the ASP.NET Core MVC controller using the [built-in model binding](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-3.1).

***

</div>
</div>

<p>&nbsp;</p>
<p>&nbsp;</p>


<div class="row">
<div class="col-md-4">
    <h2>Rapid, consistent, correct forms</h2>
    <p>Use terse, declarative, type-safe, intellisense-friendly syntax to quickly define your forms and let conventions and templates take care of the detail so you don't have to.</p>
    <p>Your forms will be quicker to write and easier to maintain and you won't get stuck writing the same form boilerplate markup form after form after form. Plus, you can build in accessibility and consistency as cross-cutting concerns.</p>
    <p><strong>ChameleonForms really shines when you need to build a lot of forms quickly and consistently.</strong></p>
</div>
<div class="col-md-8">


# [View (TH)](#tab/rapid-1)

**Tag helpers variant**

```cshtml
@model SignupViewModel
<h1>Account signup</h1>
<chameleon-form attr-id="signup-form">
    <form-message type="Information" heading="Signup for an account">
        <message-paragraph>Please fill in your information below to signup for an account.</message-paragraph>
    </form-message>

    <form-section heading="Your details">
        <field for="FirstName" />
        <field for="LastName" />
        <field for="DateOfBirth" hint="DD/MM/YYYY" />
    </form-section>

    <form-section heading="Account details">
        <field for="EmailAddress" hint="An email will be sent to this address to confirm you own it" />
        <field for="Password" />
        <field for="MembershipType" />
    </form-section>

    <form-section heading="Additional details">
        <field for="Bio" />
        <field for="Homepage" placeholder="https://" />
    </form-section>

    <form-message type="Action" heading="Confirm the Terms & Conditions">
        <message-paragraph>Please <a href="#">read the terms and conditions</a></message-paragraph>
        <field-element for="TermsAndConditions" inline-label="I agree to the terms and conditions" />
    </form-message>

    <form-navigation>
        <submit-button label="Signup" emphasis-style="Primary" />
    </form-navigation>
</chameleon-form>
```

# [View (HH)](#tab/rapid-2)

**HTML helpers variant**

```cshtml
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
        @n.Submit("Signup").WithStyle(EmphasisStyle.Primary)
    }
}
```

# [Visual result](#tab/rapid-3)

![Screenshot of the rendered form](docs/signup-example-render.png)

# [HTML output](#tab/rapid-4)
```html
<h1>Account signup</h1>
<form action="" enctype="application/x-www-form-urlencoded" id="signup-form" method="post" novalidate="novalidate">
  <div class="panel panel-info">
    <div class="panel-heading">
      <h4 class="panel-title">Signup for an account</h4>
    </div>
    <div class="panel-body">
      <p>
        Please fill in your information below to signup for an account.
      </p>
    </div>
  </div>
  <fieldset>
    <legend>Your details</legend>
    <div class="form-group">
      <label class="control-label" for="FirstName">First name</label>
      <div class="input-group">
        <input class="form-control" data-val="true" data-val-required="The First name field is required." id="FirstName" name="FirstName" required="required" type="text" value="" />
        <div class="input-group-addon required">
          <em class="required" title="Required">&lowast;</em>
        </div>
      </div>
      <span class="field-validation-valid help-block" data-valmsg-for="FirstName" data-valmsg-replace="true"></span>
    </div>
    <div class="form-group">
      <label class="control-label" for="LastName">Last name</label>
      <div class="input-group">
        <input class="form-control" data-val="true" data-val-required="The Last name field is required." id="LastName" name="LastName" required="required" type="text" value="" />
        <div class="input-group-addon required">
          <em class="required" title="Required">&lowast;</em>
        </div>
      </div>
      <span class="field-validation-valid help-block" data-valmsg-for="LastName" data-valmsg-replace="true"></span>
    </div>
    <div class="form-group">
      <label class="control-label" for="DateOfBirth">Date of birth</label>
      <div class="input-group">
        <input aria-describedby="DateOfBirth--Hint" class="form-control" data-val="true" data-val-date="The field Date of birth must be a date with format d/M/yyyy." data-val-format="d/M/yyyy" data-val-required="The Date of birth field is required." id="DateOfBirth" name="DateOfBirth" required="required" type="text" value="" />
        <div class="input-group-addon required">
          <em class="required" title="Required">&lowast;</em>
        </div>
      </div>
      <div class="help-block form-hint" id="DateOfBirth--Hint">DD/MM/YYYY</div>
      <span class="field-validation-valid help-block" data-valmsg-for="DateOfBirth" data-valmsg-replace="true"></span>
    </div>
  </fieldset>
  <fieldset>
    <legend>Account details</legend>
    <div class="form-group">
      <label class="control-label" for="EmailAddress">Email address</label>
      <div class="input-group">
        <input aria-describedby="EmailAddress--Hint" class="form-control" data-val="true" data-val-email="The Email address field is not a valid e-mail address." data-val-required="The Email address field is required." id="EmailAddress" name="EmailAddress" required="required" type="email" value="" />
        <div class="input-group-addon required">
          <em class="required" title="Required">&lowast;</em>
        </div>
      </div>
      <div class="help-block form-hint" id="EmailAddress--Hint">An email will be sent to this address to confirm you own it</div>
      <span class="field-validation-valid help-block" data-valmsg-for="EmailAddress" data-valmsg-replace="true"></span>
    </div>
    <div class="form-group">
      <label class="control-label" for="Password">Password</label>
      <div class="input-group">
        <input class="form-control" data-val="true" data-val-required="The Password field is required." id="Password" name="Password" required="required" type="password" />
        <div class="input-group-addon required">
          <em class="required" title="Required">&lowast;</em>
        </div>
      </div>
      <span class="field-validation-valid help-block" data-valmsg-for="Password" data-valmsg-replace="true"></span>
    </div>
    <div class="form-group">
      <label class="control-label" for="MembershipType">Membership type</label>
      <div class="input-group">
        <select class="form-control" data-val="true" data-val-required="The Membership type field is required." id="MembershipType" name="MembershipType" required="required">
          <option selected="selected" value="Standard">Standard</option>
          <option value="Bonze">Bonze</option>
          <option value="Silver">Silver</option>
          <option value="Gold">Gold</option>
          <option value="Platinum">Platinum</option>
        </select>
        <div class="input-group-addon required">
          <em class="required" title="Required">&lowast;</em>
        </div>
      </div>
      <span class="field-validation-valid help-block" data-valmsg-for="MembershipType" data-valmsg-replace="true"></span>
    </div>
  </fieldset>
  <fieldset>
    <legend>Additional details</legend>
    <div class="form-group">
      <label class="control-label" for="Bio">Bio</label>
      <textarea class="form-control" cols="20" id="Bio" name="Bio" rows="2"></textarea>
      <span class="field-validation-valid help-block" data-valmsg-for="Bio" data-valmsg-replace="true"></span>
    </div>
    <div class="form-group">
      <label class="control-label" for="Homepage">Homepage</label>
      <input class="form-control" id="Homepage" name="Homepage" placeholder="http://" type="url" value="" />
      <span class="field-validation-valid help-block" data-valmsg-for="Homepage" data-valmsg-replace="true"></span>
    </div>
  </fieldset>
  <div class="panel panel-primary">
    <div class="panel-heading">
      <h4 class="panel-title">Confirm the Terms &amp; Conditions</h4>
    </div>
    <div class="panel-body">
      <p>
        Please 
        <a href="#">read the terms and conditions</a>
      </p>
      <input data-val="true" data-val-required="The Terms and conditions field is required." id="TermsAndConditions" name="TermsAndConditions" required="required" type="checkbox" value="true" />
      <label for="TermsAndConditions">I agree to the terms and conditions</label>
    </div>
  </div>
  <div class="btn-group">
    <button class="btn btn-primary" type="submit">Signup</button>
  </div>
</form>
```

# [Startup / VM](#tab/rapid-5)
**Startup**
```c#
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.AddChameleonForms<TwitterBootstrap3FormTemplate>();
    }
```

**View model**
```c#
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

# [Notes](#tab/rapid-6)

Here are the things that ChameleonForms has done for us:

* We've been able to declaratively declare the structure of the form rather than the presentation of the form - this is akin to the separation we get from declarative HTML separated from CSS and JS.
* The resulting markup for the form itself, the user messages, form sections, fields and navigation have all been sorted out, consistently, for us using a form template we've been able to select in `Startup.cs` (in this case using Bootstrap). The amount of HTML that is required to render that form (see HTML output tab) is a lot - it's easy to parts of that wrong if you need to specify the boilerplate manually, plus it then couples all of your forms to that specific template. With ChameleonForms we can swap out the template with a single line of code, for instance when you want to switch from Bootstrap to a more customised setup if you make it big and want to add some bespoke design love.
* All of the syntax is type-safe and thus benefits from a combination of intellisense to speed up writing and protection from runtime mistakes (e.g. id mismatches etc.). We also know that the form will definitely correctly bind to the view model on the server-side MVC controller without needing to perform slow UI or manual tests.
* All fields automatically have a combination of server-side validation and client-side validation logic and messages added in for us built on top of the ASP.NET Core MVC features.
* All fields are easily and tersely configurable to include hints and other tweaks to the rendered markup using typesafe / intellisense code.

Documentation worth exploring to dive into more detail includes:

* [Deep-dive on the example](docs/index.md)
* [Configuring ChameleonForms](docs/configuration.md)
* [Bootstrap template](docs/bootstrap-template.md)
* [Field Configuration](docs/field-configuration.md)
* [Form structure](docs/getting-started.md#how-are-chameleonforms-forms-structured): [Form](docs/the-form.md), [Message](docs/the-message.md), [Section](docs/the-section.md), [Navigation](docs/the-navigation.md), [Field](docs/field.md), [Field Element](docs/field-element.md), [Field Label](docs/field-label.md) and [Field Validation HTML](docs/field-validation-html.md)
* [Using different form templates](docs/form-templates.md) and [creating custom form templates](docs/custom-template.md)

***

</div>
</div>

<p>&nbsp;</p>
<p class="home-buttons"><a href="docs/index.md" class="btn btn-primary btn-lg">Tell me more?!</a> <a href="docs/getting-started.md" class="btn btn-primary btn-lg">I want to get started!</a></p>
