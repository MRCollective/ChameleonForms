Compare ChameleonForms to out-of-the-box ASP.NET MVC
====================================================

One of the easiest ways to understand ChameleonForms is to see an equivalent sample of something you are already familiar with (e.g. out-of-the-box ASP.NET MVC) alongside the equivalent with ChameleonForms. With this in mind we have created an example comparison in the [Example project](https://github.com/MRCollective/ChameleonForms/tree/master/ChameleonForms.Example) that you can run and inspect to illustrate how to implement an example form using:

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
            [Required]
            [DisplayFormat(DataFormatString = "d/M/yyyy", ApplyFormatInEditMode = true)]
            public DateTime DateOfBirth { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            public string EmailAddress { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Required]
            public MembershipType MembershipType { get; set; }

            [DataType(DataType.Url)]
            public string Homepage { get; set; }
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
```

### Editor Templates

```csharp
    using (Html.BeginSection("Account details"))
    {
        @Html.EditorFor(m => m.EmailAddress, new {hint = "An email will be sent to this address to confirm you own it"})
        @Html.EditorFor(m => m.Password)
        @Html.EditorFor(m => m.MembershipType)
    }
```

### ChameleonForms

```csharp
    using (var s = f.BeginSection("Account details"))
    {
        @s.FieldFor(m => m.EmailAddress).WithHint("An email will be sent to this address to confirm you own it")
        @s.FieldFor(m => m.Password)
        @s.FieldFor(m => m.MembershipType)
    }
```

### HTML output

```html
    <fieldset>
        <legend>Account details</legend>
        <dl>
            <dt><label for="EmailAddress">Email address</label> <em class="required">*</em></dt>
            <dd><input data-val="true" data-val-required="The Email address field is required." id="EmailAddress" name="EmailAddress" type="text" value="" /><div class="hint">An email will be sent to this address to confirm you own it</div> <span class="field-validation-valid" data-valmsg-for="EmailAddress" data-valmsg-replace="true"></span></dd>
            <dt><label for="Password">Password</label> <em class="required">*</em></dt>
            <dd><input data-val="true" data-val-required="The Password field is required." id="Password" name="Password" type="password" /> <span class="field-validation-valid" data-valmsg-for="Password" data-valmsg-replace="true"></span></dd>
            <dt><label for="MembershipType">Membership type</label> <em class="required">*</em></dt>
            <dd><select data-val="true" data-val-required="The Membership type field is required." id="MembershipType" name="MembershipType"><option value="Standard">Standard</option><option value="Bonze">Bonze</option><option value="Silver">Silver</option><option value="Gold">Gold</option><option value="Platinum">Platinum</option></select> <span class="field-validation-valid" data-valmsg-for="MembershipType" data-valmsg-replace="true"></span>
            </dd>
        </dl>
    </fieldset>
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
* Furthermore, the additional configurations aren't able to be chained fluently, which we feel makes them much quicker / easier to write
    * e.g. `@s.FieldFor(m => m.SomeField).Hint("A hint...").Placeholder("http://").AddClass("urlfield")`
* If you want an enum to show up as a drop-down you have to create an editor template for every enum in your application
    * ChameleonForms automatically outputs an enum field as a dropdown
* If you want to change from a drop-down to a list of radio buttons for a particular field then you will need to do it inline every time or add extra code to every editor template that you want to do it for (or provide some sort of parent editor template that all of them use)
    * You can use `.AsRadioList()` to specify that the field should be a list of radio buttons instead, e.g. `@s.FieldFor(m => m.EnumField).AsRadioList()`
* In order to get it working that way we had to add 27 files with a total of 227 lines of code (big thanks to [Dan Malcolm](http://www.danmalcolm.com/2013/06/using-razor-to-customise-templates-used.html) for [converting the standard Editor Templates to Razor](https://github.com/danmalcolm/mvc-razor-display-and-editor-templates)) - if you wanted to provide test coverage of the form output then you would have even more files and lines of code
    * With ChameleonForms you simply `Install-Package ChameleonForms` using NuGet Package Manager Console and you are good to go

There are two things that you do get with the Editor Templates in our example that you don't currently get with ChameleonForms:

* The Editor Templates automatically use HTML5 field types for numerics and dates
* The Editor Templates understand all of the `[DataType()]` types (e.g. email address, URLs, etc.) and both uses HTML5 data-types (where appropriate) and includes relevant unobtrusive client-side validation properties (except for Password and Multiline Text, which ChameleonForms uses to output a password field and textarea field)

There are [plans in the backlog](https://trello.com/b/fSuyhwNZ/) to provide support for HTML5 field types and the DataTypes, but it has been deliberately deprioritised because:

* There are some annoyances and browser compatibility issues with using HTML5 field types as well as extra attributes that you can use to augment them and we want to get it right
* The DataTypes are misleading because they don't provide server-side validation - only client-side validation - so we don't encourage use of them; again, we want to get it right

Also, it's worth noting that Editor Templates can handle sub-view models that are shared across models, whereas it's difficult to do that in Chameleon (there are [plans in the backlog](https://trello.com/b/fSuyhwNZ/) to address this eventually).

Further comparison
------------------

The above commentary and comparison were for a single section of the form - if you want to compare the rest of the form then you can check out the [Example project](https://github.com/MRCollective/ChameleonForms/tree/master/ChameleonForms.Example).