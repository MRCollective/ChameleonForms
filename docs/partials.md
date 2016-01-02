Use partial views for repeated or abstracted form areas
=======================================================

You can use partial views to abstract a complex part of your form or to reuse common parts of your form. You can do this by calling the `Partial` or `PartialFor` methods on the Form or Section.

The `PartialFor` method allows you to specify a sub-property of your model to pass in as the model of the partial. ChameleonForms will automatically set the correct name of any fields that are input to ensure they bind back to the parent model on postback.

When inside a partial view you can use the following methods off of `this`:

* `this.Form()` - returns the current Form instance
* `this.FormSection()` - returns the current Section instance if there is one, otherwise it throws an Exception
* `this.IsInFormSection()` - returns whether you are currently inside of a Section

The best way to see how this works is by looking at the output of the [partials acceptance test](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms.AcceptanceTests/PartialForTests.Should_render_correctly_when_used_via_form_or_section_and_when_used_for_top_level_property_or_sub_property.approved.html).