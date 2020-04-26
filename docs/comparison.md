Compare ChameleonForms to out-of-the-box ASP.NET MVC
====================================================

One of the easiest ways to understand ChameleonForms is to see an equivalent sample of something you are already familiar with (e.g. out-of-the-box ASP.NET MVC) alongside the equivalent with ChameleonForms. With this in mind we have created an example comparison in the [Example project](https://github.com/MRCollective/ChameleonForms/tree/master/ChameleonForms.Example) that you can run and inspect to illustrate how to implement an example form using:

* *(Coming soon)* Tag Helpers
* [Standard HTML helpers and HTML](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Views/Comparison/HtmlHelpers.cshtml)
* [Abstracted and templated Editor Templates](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Views/Comparison/EditorTemplates.cshtml)
* [ChameleonForms](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Views/Comparison/ChameleonForms.cshtml)

Example Form
------------

The example in question is an arbitrary signup form against the `SignupViewModel` in the [`ComparisonController`](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.Example/Controllers/ComparisonController.cs):

```csharp
        public class SignupViewModel
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
            [Required]
            public DateTime DateOfBirth { get; set; }

            [Required]
            [EmailAddress]
            public string EmailAddress { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Required]
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

Example Form Structure
----------------------

The sample implementation of the form has been split up as follows:

* Form
    * Message (Information)
        * Instructions to fill in the form to sign up for an account
    * Section to enter details
        * Fields for first name, last name and date of birth
    * Section to enter account details
        * Fields for email address, password, and drop down for membership type
    * Section to enter additional details
        * Field for homepage and text area for bio
    * Message (Action)
        * Link to terms and conditions and checkbox for user to confirm
    * Navigation
        * Submit button

In-depth comparison of details Form Section
-------------------------------------------

In the case where we use the Form Section to ask the user to enter account details, here is a comparison of the different implementations:

### HTML Helpers

```html
@using (Html.BeginForm("HtmlHelpers", "Comparison", FormMethod.Post, new {id = "signup-form"}))
{
    <div class="information_message">
        <h3>Signup for an account</h3>
        <p>Please fill in your information below to signup for an account.</p>
    </div>
    
    <fieldset>
        <legend>Your details</legend>
        <dl>
            <dt>@Html.LabelFor(m => m.FirstName) <em class="required">*</em></dt>
            <dd>@Html.TextBoxFor(m => m.FirstName) @Html.ValidationMessageFor(m => m.FirstName)</dd>
            <dt>@Html.LabelFor(m => m.LastName) <em class="required">*</em></dt>
            <dd>@Html.TextBoxFor(m => m.LastName) @Html.ValidationMessageFor(m => m.LastName)</dd>
            <dt>@Html.LabelFor(m => m.DateOfBirth) <em class="required">*</em></dt>
            <dd>@Html.TextBoxFor(m => m.DateOfBirth, new{data_val_format = "d/M/yyyy"})<div class="hint">DD/MM/YYYY</div> @Html.ValidationMessageFor(m => m.DateOfBirth)</dd>
        </dl>
    </fieldset>
    
    <fieldset>
        <legend>Account details</legend>
        <dl>
            <dt>@Html.LabelFor(m => m.EmailAddress) <em class="required">*</em></dt>
            <dd>@Html.TextBoxFor(m => m.EmailAddress)<div class="hint">An email will be sent to this address to confirm you own it</div> @Html.ValidationMessageFor(m => m.EmailAddress)</dd>
            <dt>@Html.LabelFor(m => m.Password) <em class="required">*</em></dt>
            <dd>@Html.PasswordFor(m => m.Password) @Html.ValidationMessageFor(m => m.Password)</dd>
            <dt>@Html.LabelFor(m => m.MembershipType) <em class="required">*</em></dt>
            <dd>@Html.DropDownListFor(m => m.MembershipType,
                    Enum.GetValues(typeof(ComparisonController.MembershipType))
                        .Cast<ComparisonController.MembershipType>()
                        .Select(m => new SelectListItem{Text = m.Humanize(), Value = m.ToString()})
                ) @Html.ValidationMessageFor(m => m.MembershipType)
            </dd>
        </dl>
    </fieldset>
    
    <fieldset>
        <legend>Additional details</legend>
        <dl>
            <dt>@Html.LabelFor(m => m.Bio)</dt>
            <dd>@Html.TextAreaFor(m => m.Bio) @Html.ValidationMessageFor(m => m.Bio)</dd>
            <dt>@Html.LabelFor(m => m.Homepage)</dt>
            <dd>@Html.TextBoxFor(m => m.Homepage, new{placeholder = "http://"}) @Html.ValidationMessageFor(m => m.Homepage)</dd>
        </dl>
    </fieldset>
    
    <div class="action_message">
        <h3>Confirm the Terms &amp; Conditions</h3>
        <p>Please <a href="#">read the terms and conditions</a>.</p>
        @Html.EditorFor(m => m.TermsAndConditions) @Html.LabelFor(m => m.TermsAndConditions, "I agree to the terms and conditions")
    </div>
    
    <div class="form_navigation">
        <button type="submit">Signup</button>
    </div>
}
```

### Editor Templates

Note: for this to work there are [a lot of extra files you need to implement](https://github.com/MRCollective/ChameleonForms/tree/master/ChameleonForms.Example/Views/Comparison/EditorTemplates).

```csharp
@using (Html.BeginForm("EditorTemplates", "Comparison", FormMethod.Post, new {id = "signup-form"}))
{
    <div class="information_message">
        <h3>Signup for an account</h3>
        <p>Please fill in your information below to signup for an account.</p>
    </div>
    
    using (Html.BeginSection("Your details"))
    {
        @Html.EditorFor(m => m.FirstName)
        @Html.EditorFor(m => m.LastName)
        @Html.EditorFor(m => m.DateOfBirth, new {hint = "DD/MM/YYYY"})
    }

    using (Html.BeginSection("Account details"))
    {
        @Html.EditorFor(m => m.EmailAddress, new {hint = "An email will be sent to this address to confirm you own it"})
        @Html.EditorFor(m => m.Password)
        @Html.EditorFor(m => m.MembershipType)
    }

    using (Html.BeginSection("Additional details"))
    {
        @Html.EditorFor(m => m.Bio)
        @Html.EditorFor(m => m.Homepage, new {placeholder = "http://"})
    }
    
    <div class="action_message">
        <h3>Confirm the Terms &amp; Conditions</h3>
        <p>Please <a href="#">read the terms and conditions</a>.</p>
        @Html.CheckBoxFor(m => m.TermsAndConditions) @Html.LabelFor(m => m.TermsAndConditions, "I agree to the terms and conditions")
    </div>
    
    <div class="form_navigation">
        <button type="submit">Signup</button>
    </div>
}

```

### ChameleonForms

```csharp
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
        @s.FieldFor(m => m.Bio)
        @s.FieldFor(m => m.Homepage).Placeholder("http://")
    }

    using (var m = f.BeginMessage(MessageType.Action, "Confirm the Terms & Conditions"))
    {
        @m.Paragraph(@<text>Please <a href="#">read the terms and conditions</a></text>)
        @f.FieldElementFor(mm => mm.TermsAndConditions).InlineLabel("I agree to the terms and conditions")
    }

    using (var n = f.BeginNavigation())
    {
        @n.Submit("Signup")
    }
}
```

### HTML output

```html
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
                <input aria-describedby="EmailAddress--Hint" data-val="true" data-val-email="The Email address field is not a valid e-mail address." data-val-required="The Email address field is required." id="EmailAddress" name="EmailAddress" required="required" type="email" value="" /><div class="hint" id="EmailAddress--Hint">An email will be sent to this address to confirm you own it</div> <span class="field-validation-valid" data-valmsg-for="EmailAddress" data-valmsg-replace="true"></span>
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
                <textarea cols="20" id="Bio" name="Bio" rows="2">
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

Commentary
----------

There is an immediately discernible difference between the Html Helpers example as compared to Editor Templates / ChameleonForms, as well as some more subtle differences between the latter two.

### Html Helpers

When you are writing forms with this approach you need to think about the HTML output of the form since you have to write so much of it; in contrast to the other two examples where you are instead concentrating on the structure of the form. If you have consistent HTML for the forms in your site then we think this shift in mindset is an advantage and will lead to better designed forms that are easier to change. This also means that when you need to change the HTML structure of your forms you have a big job ahead of you rewriting large portions of your application (and testing every page thoroughly since it's so easy to make a mistake).

In addition to the above, you can't easily add conventions across your forms based on the type of field or data:
* You have to explicitly specify the type of field you want (e.g. `Html.TextBoxFor` vs `Html.PasswordFor` vs `Html.TextAreaFor` etc.)
* You have to manually add the required indicator
* You have to manually specify the select list items for each drop-down

You have to also specify the label, field and validation HTML separately and in the right order. It's easy to make copy/paste errors and not remember to update one of the lambda expressions.

Overall, the form is harder to read / more cluttered, harder to maintain, slower to develop and easier to get wrong.

### Editor Templates

On first glance the syntax of the Editor Templates example and the ChameleonForms example are very similar. There are a number of differences, however:

* The additional ad-hoc per-field configurations (e.g. adding a hint, adding a placeholder, etc.) are added via magic strings (adding them to an anonymous object is still a magic string in our books), which isn't type-safe and you don't get intellisense
    * e.g. `@Html.EditorFor(m => m.SomeField, new { hint = "hint..." })` vs `@s.FieldFor(m => m.SomeField).Hint("hint...")`
* Furthermore, the additional configurations aren't able to be chained fluently, which we feel makes them much quicker / easier to write and discover
    * e.g. `@s.FieldFor(m => m.SomeField).Hint("A hint...").Placeholder("http://").AddClass("urlfield")`
* If you want an enum to show up as a drop-down you have to create an editor template for every enum in your application
    * ChameleonForms automatically outputs an enum field as a dropdown
* If you want to change from a drop-down to a list of radio buttons for a particular field then you will need to do it inline every time or add extra code to every editor template that you want to do it for (or provide some sort of parent editor template that all of them use)
    * You can use `.AsRadioList()` to specify that the field should be a list of radio buttons instead, e.g. `@s.FieldFor(m => m.EnumField).AsRadioList()`
* In order to get it working that way we had to add 27 files with a total of 227 lines of code (big thanks to [Dan Malcolm](http://www.danmalcolm.com/2013/06/using-razor-to-customise-templates-used.html) for [converting the standard Editor Templates to Razor](https://github.com/danmalcolm/mvc-razor-display-and-editor-templates)) - if you wanted to provide test coverage of the form output then you would have even more files and lines of code
    * With ChameleonForms you simply `Install-Package ChameleonForms` using NuGet Package Manager Console and you are good to go
