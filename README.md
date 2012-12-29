ChameleonForms
==============

This library will shape-shift your forms experience in ASP.NET MVC.

It makes it really easy to tersely output a form and easily apply a different template with a single line of code. The library is built with extensibility and flexibility in mind and at any point if you have a usecase that is too complex for what the library handles you can easy drop back into plain Razor / HTML for that part and utilise this library for the bulk of the form that is more standard.

This library is in the very early stages of development and can be considered Beta quality software. We are very confident that the code is comprehensively tested and we are actively adding functionality.

Basic Example
-------------

Say you had the following view model:

```c#
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

There are a number of problems with this:

* It's tedious to write
* There is a lot of repetition (each field essentially has the same structure - label in a dt and field and validation in a dd)
* It's easy to miss out something (e.g. validation HTML) and this results in inconsistency
* If you want to change the HTML template that you use for your forms (in a small or big way) across the whole site then you need to go into every single `.cshtml` file and change them - i.e. it's a maintenance _nightmare_!

The same HTML output can be achieved with Chameleon Forms out of the box with the following code:

```c#
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

Here is a more complex example:

View Model
----------

    public class ViewModelExample
    {
        public ViewModelExample()
        {
            // This could be set using a model binder if it's populated from a database or similar
            List = new List<ListItem> {new ListItem{Id = 1, Name = "A"}, new ListItem{Id = 2, Name = "B"}};
        }

        [Required]
        public string RequiredStringField { get; set; }

        public string NestedField { get; set; }

        public SomeEnum SomeEnum { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }
		
        [DataType(DataType.MultilineText)]
        public string TextAreaField { get; set; }

        public bool SomeCheckbox { get; set; }

        public List<ListItem> List { get; set; }
        [ExistsIn("List", "Id", "Name")]
        public int ListId { get; set; }
    }

    public class ListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

Razor view
----------

	@using (var f = Html.BeginChameleonForm(Url.Action("Index", "Home"), FormMethod.Post, enctype: EncType.Multipart))
	{
		using (var m = form.BeginMessage(MessageType.Success, "Success!"))
		{
			@m.Paragraph("Hello!!!!!!!!")
		}
		<p>@f.LabelFor(m => m.SomeCheckbox).Label("Are you ready for: ") @f.FieldFor(m => m.SomeCheckbox) @f.ValidationMessageFor(m => m.SomeCheckbox)</p>
		using (var s = f.BeginSection("My Section!", InstructionalText()))
		{
			using (var ff = s.BeginFieldFor(m => m.RequiredStringField, Field.Configure().Attr("data-some-attr", "value")))
			{
				@ff.FieldFor(m => m.NestedField).Attr("data-attr1", "value")
			}
			using (var ss = s.BeginSection("Nested section"))
			{
				@ss.FieldFor(m => m.FileUpload).Attr("data-attr1", "value")
			}
			@s.FieldFor(m => m.SomeEnum).Attr("data-attr1", "value")
			@s.FieldFor(m => m.TextAreaField).Cols(60).Rows(5).Label("Some Label")
			@s.FieldFor(m => m.SomeCheckbox).InlineLabel("Some label").WithHint("Format: XXX")
			@s.FieldFor(m => m.SomeCheckbox).AsList().WithTrueAs("True").WithFalseAs("False")
			@s.FieldFor(m => m.SomeCheckbox).AsDropDown()
			@s.FieldFor(m => m.ListId)
			@s.FieldFor(m => m.ListId).AsList()
		}
		using (var n = f.BeginNavigation())
		{
			@n.Submit("Submit")
			@n.Reset("Reset")
		}
	}

HTML output (using default template that comes with Chameleon)
------------------------------------

	<form action="/" enctype="multipart/form-data" method="post">
	<div class="success_message">
        <h3>Success!</h3>
          <div class="message">
	<p>Hello!!!!!!!!</p>
          </div>
    </div>
	<p><label for="SomeCheckbox">Are you ready for: </label> <input data-val="true" data-val-required="The Some checkbox field is required." id="SomeCheckbox" name="SomeCheckbox" type="checkbox" value="true" /> <label for="SomeCheckbox">Some checkbox</label> <span class="field-validation-valid" data-valmsg-for="SomeCheckbox" data-valmsg-replace="true"></span></p>
    <fieldset>
        <legend>My Section!</legend>
            <p>Leading instructional text.</p>

        <dl>
            <dt><label for="RequiredStringField">Required string field</label></dt>
            <dd>
                <input data-some-attr="value" data-val="true" data-val-required="The Required string field field is required." id="RequiredStringField" name="RequiredStringField" type="text" value="" /> <span class="field-validation-valid" data-valmsg-for="RequiredStringField" data-valmsg-replace="true"></span>
                <dl>
            <dt><label for="NestedField">Nested field</label></dt>
            <dd>
                <input data-attr1="value" id="NestedField" name="NestedField" type="text" value="" /> <span class="field-validation-valid" data-valmsg-for="NestedField" data-valmsg-replace="true"></span>
            </dd>
                </dl>
            </dd>
            <dt>Nested section</dt>
            <dd>
                
                <dl>
            <dt><label for="FileUpload">File upload</label></dt>
            <dd>
                <input data-attr1="value" id="FileUpload" name="FileUpload" type="file" value="" /> <span class="field-validation-valid" data-valmsg-for="FileUpload" data-valmsg-replace="true"></span>
            </dd>
                </dl>
            </dd>
            <dt><label for="SomeEnum">Some enum</label></dt>
            <dd>
                <select data-attr1="value" data-val="true" data-val-required="The Some enum field is required." id="SomeEnum" name="SomeEnum"><option selected="selected" value="Value1">Value 1</option>
	<option value="ValueWithDescription">Fiendly name</option>
	<option value="SomeOtherValue">Some other value</option>
	</select> <span class="field-validation-valid" data-valmsg-for="SomeEnum" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="TextAreaField">Some Label</label></dt>
            <dd>
                <textarea cols="60" id="TextAreaField" name="TextAreaField" rows="5">
	</textarea> <span class="field-validation-valid" data-valmsg-for="TextAreaField" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="SomeCheckbox">Some checkbox</label></dt>
            <dd>
                <input id="SomeCheckbox" name="SomeCheckbox" type="checkbox" value="true" /> <label for="SomeCheckbox">Some label</label> <div class="hint">Format: XXX</div>
    <span class="field-validation-valid" data-valmsg-for="SomeCheckbox" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="SomeCheckbox">Some checkbox</label></dt>
            <dd>
                    <ul>
        <li><input id="SomeCheckbox_1" name="SomeCheckbox" type="radio" value="true" /> <label for="SomeCheckbox_1">True</label></li>
        <li><input checked="checked" id="SomeCheckbox_2" name="SomeCheckbox" type="radio" value="false" /> <label for="SomeCheckbox_2">False</label></li>
    </ul>
    <span class="field-validation-valid" data-valmsg-for="SomeCheckbox" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="SomeCheckbox">Some checkbox</label></dt>
            <dd>
                <select id="SomeCheckbox" name="SomeCheckbox"><option value="true">Yes</option>
    <option selected="selected" value="false">No</option>
    </select> <span class="field-validation-valid" data-valmsg-for="SomeCheckbox" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="ListId">List id</label></dt>
            <dd>
                <select data-val="true" data-val-number="The field List id must be a number." data-val-required="The List id field is required." id="ListId" name="ListId"><option value="1">A</option>
    <option value="2">B</option>
    </select> <span class="field-validation-valid" data-valmsg-for="ListId" data-valmsg-replace="true"></span>
            </dd>
            <dt><label for="ListId">List id</label></dt>
            <dd>
                    <ul>
        <li><input id="ListId_1" name="ListId" type="radio" value="1" /> <label for="ListId_1">A</label></li>
        <li><input id="ListId_2" name="ListId" type="radio" value="2" /> <label for="ListId_2">B</label></li>
    </ul>
    <span class="field-validation-valid" data-valmsg-for="ListId" data-valmsg-replace="true"></span>
            </dd>
        </dl>
    </fieldset>
        <div class="form_navigation">
    <input type="submit" value="Submit" /><input type="reset" value="Reset" />        </div>
    </form>

Contributions
-------------

If you would like to contribute to this project then feel free to communicate with us via Twitter @robdmoore / @mdaviesnet or alternatively send a pull request.

Roadmap
-------

Feel free to check out our [Trello board](https://trello.com/board/chameleonforms/504df3392ad570121c36c3f7). It gives some idea as to the eventual goals we have for the project and the current backlog we are working against. Beware that it's pretty rough around the edges at the moment.
