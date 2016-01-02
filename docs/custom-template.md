Creating custom form templates
==============================

To create a custom form template you need to have a class that derives from the `IFormTemplate` interface:

```c#
    /// <summary>
    /// A Chameleon Forms form template renderer.
    /// </summary>
    public interface IFormTemplate
    {
        /// <summary>
        /// Allows the template the modify the field configuration for a particular field.
        /// </summary>
        /// <typeparam name="TModel">The type of model the form is being displayed for</typeparam>
        /// <typeparam name="T">The type of the property the field is being generated against</typeparam>
        /// <param name="fieldGenerator">The instance of the field generator that will be used to generate the field</param>
        /// <param name="fieldGeneratorHandler">The instance of the field generator handler that will be used to generate the field element</param>
        /// <param name="fieldConfiguration">The field configuration that is being used to configure the field</param>
        /// <param name="fieldParent">The parent component of the field</param>
        void PrepareFieldConfiguration<TModel, T>(IFieldGenerator<TModel, T> fieldGenerator, IFieldGeneratorHandler<TModel, T> fieldGeneratorHandler, IFieldConfiguration fieldConfiguration, FieldParent fieldParent);

        /// <summary>
        /// Creates the starting HTML for a form.
        /// </summary>
        /// <param name="action">The form action</param>
        /// <param name="method">The form method</param>
        /// <param name="htmlAttributes">Any HTML attributes the form should use; specified as an anonymous object</param>
        /// <param name="enctype">The encoding type for the form</param>
        /// <returns>The starting HTML for a form</returns>
        IHtmlString BeginForm(string action, FormMethod method, HtmlAttributes htmlAttributes, EncType? enctype);

        /// <summary>
        /// Creates the ending HTML for a form.
        /// </summary>
        /// <returns>The ending HTML for a form</returns>
        IHtmlString EndForm();

        /// <summary>
        /// Creates the beginning HTML for a section.
        /// </summary>
        /// <param name="heading">The heading of the section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the section</param>
        /// <param name="htmlAttributes">Any HTML attributes the section container should use; specified as an anonymous object</param>
        /// <returns>The beginning HTML for a section</returns>
        IHtmlString BeginSection(IHtmlString heading = null, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null);

        /// <summary>
        /// Creates the ending HTML for a section.
        /// </summary>
        /// <returns>The ending HTML for a section</returns>
        IHtmlString EndSection();

        /// <summary>
        /// Creates the beginning HTML for a section that is nested within another section.
        /// </summary>
        /// <param name="heading">The heading of the nested section</param>
        /// <param name="leadingHtml">Any HTML to output at the start of the nested section</param>
        /// <param name="htmlAttributes">Any HTML attributes the nested section container should use; specified as an anaonymous object</param>
        /// <returns>The beginning HTML for a nested section</returns>
        IHtmlString BeginNestedSection(IHtmlString heading = null, IHtmlString leadingHtml = null, HtmlAttributes htmlAttributes = null);

        /// <summary>
        /// Creates the ending HTML for a section that is nested within another section.
        /// </summary>
        /// <returns>The ending HTML for a nested section</returns>
        IHtmlString EndNestedSection();

        /// <summary>
        /// Creates the HTML for a single form field.
        /// </summary>
        /// <param name="labelHtml">The HTML that comprises the form label</param>
        /// <param name="elementHtml">The HTML that comprieses the field itself</param>
        /// <param name="validationHtml">The HTML that comprises the field's validation messages</param>
        /// <param name="fieldMetadata">The metadata for the field being created</param>
        /// <param name="fieldConfiguration">Configuration for the field</param>
        /// <param name="isValid">Whether or not the field is valid</param>
        /// <returns>The HTML for the field</returns>
        IHtmlString Field(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid);

        /// <summary>
        /// Creates the beginning HTML for a single form field that contains other fields nested within it.
        /// </summary>
        /// <param name="labelHtml">The HTML that comprises the form label</param>
        /// <param name="elementHtml">The HTML that comprieses the field itself</param>
        /// <param name="validationHtml">The HTML that comprises the field's validation messages</param>
        /// <param name="fieldMetadata">The metadata for the field being created</param>
        /// <param name="fieldConfiguration">Configuration for the field</param>
        /// <param name="isValid">Whether or not the field is valid</param>
        /// <returns>The beginning HTML for the parent field</returns>
        IHtmlString BeginField(IHtmlString labelHtml, IHtmlString elementHtml, IHtmlString validationHtml, ModelMetadata fieldMetadata, IReadonlyFieldConfiguration fieldConfiguration, bool isValid);

        /// <summary>
        /// Creates the ending HTML for a single form field that contains other fields nested within it.
        /// </summary>
        /// <returns>The ending HTML for the parent field</returns>
        IHtmlString EndField();

        /// <summary>
        /// Creates the beginning HTML for a navigation section.
        /// </summary>
        /// <returns>The beginning HTML for a navigation section</returns>
        IHtmlString BeginNavigation();

        /// <summary>
        /// Creates the ending HTML for a navigation section.
        /// </summary>
        /// <returns>The ending HTML for a navigation section</returns>
        IHtmlString EndNavigation();

        /// <summary>
        /// Creates the beginning HTML for a message.
        /// </summary>
        /// <param name="messageType">The type of message being displayed</param>
        /// <param name="heading">The heading for the message</param>
        /// <returns>The beginning HTML for the message</returns>
        IHtmlString BeginMessage(MessageType messageType, IHtmlString heading);

        /// <summary>
        /// Creates the ending HTML for a message.
        /// </summary>
        /// <returns>The ending HTML for the message</returns>
        IHtmlString EndMessage();

        /// <summary>
        /// Creates the HTML for a paragraph in a message.
        /// </summary>
        /// <param name="paragraph">The paragraph HTML</param>
        /// <returns>The HTML for the message paragraph</returns>
        IHtmlString MessageParagraph(IHtmlString paragraph);

        /// <summary>
        /// Creates the HTML for a button.
        /// </summary>
        /// <param name="content">The content for the user to see or null if the value should be used instead</param>
        /// <param name="type">The type of button or null if a generic button should be used</param>
        /// <param name="id">The name/id of the button or null if one shouldn't be set</param>
        /// <param name="value">The value to submit if the button is clicked or null if one shouldn't be set</param>
        /// <param name="htmlAttributes">Any HTML attributes to add to the button or null if there are none</param>
        /// <returns>The HTML for the button</returns>
        IHtmlString Button(IHtmlString content, string type, string id, string value, HtmlAttributes htmlAttributes);

        /// <summary>
        /// Creates the HTML for a list of radio buttons or checkboxes.
        /// </summary>
        /// <param name="list">The list of HTML items (one per radio/checkbox)</param>
        /// <param name="isCheckbox">Whether the list is for checkboxes rather than radio buttons</param>
        /// <returns>The HTML for the radio list</returns>
        IHtmlString RadioOrCheckboxList(IEnumerable<IHtmlString> list, bool isCheckbox);
    }
```

Recommendation
==============

To make it easy for yourself we recommend that you start by extending the `DefaultFormTemplate` class and override each method in turn that you want to change the HTML for. For an example of this approach see the [Twitter Bootstrap 3 Form Template](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/Templates/TwitterBootstrap3/TwitterBootstrapFormTemplate.cs), which extends most, but not all methods.

RazorGenerator
==============

Rather than expressing all of the complex template HTML as a string we recommend that you instead use the [RazorGenerator Visual Studio extension](http://razorgenerator.codeplex.com/) so that you can specify the HTML using Razor syntax in a `.cshtml` file beginning with:

```c#
@* Generator: MvcHelper *@
```

And then including a `@helper` method for every snippet of HTML that you need to generate. Change the `Custom Tool` property in Visual Studio against your `.cshtml` file to be `RazorGenerator` and every time you save the file it will compile it to a C# class thanks to the RazorGenerator extension.

This is how we generate the HTML for the built-in templates within ChameleonForms, e.g. the [Default form template](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/Templates/Default/DefaultHtmlHelpers.cshtml).

PrepareFieldConfiguration
=========================

The `PrepareFieldConfiguration` method allows you to make arbitrary changes to the [Field Configuration](field-configuration) of a field before it is rendered by the other template methods. For an example of the type of changes you can make, see the [Twitter Bootstrap 3 form template](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/Templates/TwitterBootstrap3/TwitterBootstrapFormTemplate.cs#L31).

This technique allows you to take advantage of the expressiveness and flexibility afforded by the Field Configuration to:

* Make easily unit testable changes
* Keep your templates terse and reduce the potential noise of lots of control statements

BeginSection / BeginNestedSection
=================================

The heading is not required and we recommend having logic like the following to conditionally display the heading:

```html
@if (heading != null && !string.IsNullOrWhiteSpace(heading.ToString())) {
    @* heading html here *@
}
```

Similarly, the leading HTML is optional and you might want to use similar conditional logic for that field (unless you don't wrap it in any HTML in which case you can simply output it).

You should always have a container element for your section which has the `HtmlAttributes` object [applied to it](html-attributes#output-it-directly-to-the-page).

Field
=====

There are a lot of parameters passed to the field method - only use the ones that you need for your template (e.g. if you don't specify different HTML when a field is invalid just ignore the `isValid` variable).

If you want the ability to specify the required designator separately within the template then make sure to call the `RequiredDesignator` method from within the `Field` method to get the HTML for the required designator.

If you want a fully-fledged Field HTML implementation then you need to account for:

* Outputting the Field Label HTML, Field Element HTML and Field Validation Message HTML
* Outputting the required designator if the field is required (e.g. `@(new HtmlString(fieldMetadata != null && fieldMetadata.IsRequired ? requiredDesignator.ToHtmlString() : ""))`)
* Outputting the prepended HTML before the field (e.g. `foreach (var html in fieldConfiguration.PrependedHtml) {@html}`)
* Outputting the appended HTML after the field (e.g. `foreach (var html in fieldConfiguration.AppendedHtml) {@html}`)
* Outputting the hint (e.g. `if (fieldConfiguration.Hint != null) {<div class="hint">@fieldConfiguration.Hint</div>}`)

Obviously, if you never plan on using some of those then you can exclude them, but if you are creating a template that you want other people to use we recommend you include all of those points so people aren't surprised when they try to use the template and it doesn't behave how they expect.

Nested fields
=============

If you don't want to support nested fields then we recommend you throw a `NotSupportedException` from the `BeginField` and `EndField` methods (which are only used for supporting nested fields).

If you do want to support them then it's likely that you will reuse a lot of the same HTML for a normal field and a parent field. The Twitter Bootstrap 3 form template and the Default form template both use a shared private `@helper` called `FieldInternal` to house this shared logic. Feel free to use the same technique.

Automated Testing
=================

If you would like to have confidence in the HTML that your template generates then we recommend that you provide HTML [Approval Test](https://github.com/approvals/ApprovalTests.Net) coverage of the HTML output of your template.

For an example of this see the tests for the [Twitter Bootstrap 3 form template](https://github.com/MRCollective/ChameleonForms/tree/master/ChameleonForms.Tests/Templates/TwitterBootstrap3).

Applying a global template change
=================================

You can often encounter the situation where the `DefaultFormTemplate` is mostly sufficient for your needs, but you want to make slight tweaks for example adding classes to all buttons, or changing the required designator. In those instances you can easily create a custom form template to use in your application that simply extends the `DefaultFormTemplate` class and overrides the relevant methods - in the above example, you would override the `Button` or `RequiredDesignator` methods respectively.

Even further to that, we can change the `HtmlAttributes` object that is passed into the `Button` method and then call `base.Button(...)` to keep the original button generation code. For an example of this in action see the `Button method` in the [Twitter Bootstrap 3 template](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/Templates/TwitterBootstrap3/TwitterBootstrapFormTemplate.cs#L126).

Using a custom form template
============================

See the [Using different form templates](form-templates) page.

Contributions
=============

If you create a form template that you believe will be useful to other people then please feel free to [send us a pull request](https://github.com/MRCollective/ChameleonForms/pulls) and we will consider it for inclusion in the core package.