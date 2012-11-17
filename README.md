ChameleonForms
==============

This library will shape-shift your forms experience in ASP.NET MVC.

It makes it really easy to tersely output a form and easily apply a different template with a single line of code. The library is built with extensibility and flexibility in mind and at any point if you have a usecase that is too complex for what the library handles you can easy drop back into plain Razor / HTML for that part and utilise this library for the bulk of the form that is more standard.

This library is in the very early stages of development and can be considered Beta quality software. We are very confident that the code is comprehensively tested, however it only takes care of the very simple cases at the moment.

Following is an example of what you can do.

View Model
----------

    public class ViewModelExample
    {
        [Required]
        public string RequiredStringField { get; set; }

        public string NestedField { get; set; }

        public SomeEnum SomeEnum { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }
		
        [DataType(DataType.MultilineText)]
        public string TextAreaField { get; set; }
    }

Razor view
----------

	@using (var f = Html.BeginChameleonForm(Url.Action("Index", "Home"), FormMethod.Post, EncType.Multipart))
	{
		using (var m = form.BeginMessage(MessageType.Success, "Success!"))
		{
			@m.Paragraph("Hello!!!!!!!!")
		}
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
			@s.FieldFor(m => m.TextAreaField).Cols(60).Rows(5)
		}
		using (var n = f.BeginNavigation())
		{
			@n.Submit("Submit")
			@n.Reset("Reset")
		}
	}

HTML output (using default template that comes with Chameleon)
------------------------------------

    <form action="/" method="post">
	<div class="success_message">
        <h3>Success!</h3>
          <div class="message">
	<p>Hello!!!!!!!!</p>
          </div>
    </div>
    <fieldset>
        <legend>My Section!</legend>
            <p>Leading instructional text.</p>

        <dl>
            <dt><label for="RequiredStringField">Required string field</label></dt>
            <dd>
                <input data-attr1="value" data-val="true" data-val-required="The Required string field field is required." id="RequiredStringField" name="RequiredStringField" type="text" value="" /> <span class="field-validation-valid" data-valmsg-for="RequiredStringField" data-valmsg-replace="true"></span>
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
            <dt><label for="TextAreaField">Text area field</label></dt>
            <dd>
                <textarea cols="60" id="TextAreaField" name="TextAreaField" rows="5">
</textarea> <span class="field-validation-valid" data-valmsg-for="TextAreaField" data-valmsg-replace="true"></span>
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
