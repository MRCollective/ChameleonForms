ChameleonForms Breaking Changes
-------------------------------

Version 0.9.20
==============

The submit/reset/button methods in Navigation now chain `HtmlAttributes` methods off the end rather than taking them as a parameter.

### Reason
This provides a much nicer experience when using these methods in your view - you don't have to new up a `HtmlAttributes` object anymore.

### Workaround
If you are using the old methods then change your `HtmlAttributes` object to use the same parameters that were in there, but by chaining method calls off of the end of the method.

For example:

    @n.Submit("Submit", new HtmlAttributes().AddClass("btn"))

Becomes:

    @n.Submit("Submit").AddClass("btn")

If you were passing in the `HtmlAttributes` object to the view rather than generating it inline then use `.Attrs(existingHtmlAttributesObject)`

