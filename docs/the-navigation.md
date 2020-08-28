# Navigation

The Navigation is a grouping of a set of navigation elements; you create a Navigation by using a `<form-navigation>` tag helper or instantiating a `Navigation<TModel>` within a `using` block. The start and end of the `using` block will output the start and end HTML for the Navigation and the inside of the `using` block will contain the Navigation elements.

The `Navigation<TModel>` class looks like this and is in the `ChameleonForms.Component` namespace:

```cs
    /// <summary>
    /// Wraps the output of the navigation area of a form.
    /// For example the area with submit buttons.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the current view</typeparam>
    public class Navigation<TModel> : FormComponent<TModel>
    {
        /// <summary>
        /// Creates a form navigation area.
        /// </summary>
        /// <param name="form">The form the message is being created in</param>
        public Navigation(IForm<TModel> form);

        /// <summary>
        /// Returns the HTML representation of the beginning of the form component.
        /// </summary>
        /// <returns>The beginning HTML for the form component</returns>
        public virtual IHtmlContent Begin();

        /// <summary>
        /// Returns the HTML representation of the end of the form component.
        /// </summary>
        /// <returns>The ending HTML for the form component</returns>
        public virtual IHtmlContent End();

        /// <summary>
        /// Creates the HTML for a submit &lt;button&gt;.
        /// </summary>
        /// <param name="text">The text to display in the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Submit(string text);

        /// <summary>
        /// Creates the HTML for a submit &lt;button&gt;.
        /// </summary>
        /// <param name="content">The content to display in the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Submit(IHtmlContent content);

        /// <summary>
        /// Creates the HTML for a submit &lt;button&gt;.
        /// </summary>
        /// <param name="content">The content to display in the button as a templated razor delegate</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Submit(Func<dynamic, IHtmlContent> content);

        /// <summary>
        /// Creates the HTML for a submit button that submits a value in the form post when clicked.
        /// </summary>
        /// <param name="name">The name of the element</param>
        /// <param name="value">The value to submit with the form</param>
        /// <param name="content">The text the user sees (leave as the default null if you want the user to see the value instead)</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Submit(string name, string value, IHtmlContent content = null);

        /// <summary>
        /// Creates the HTML for a submit button that submits a value in the form post when clicked.
        /// </summary>
        /// <param name="name">The name of the element</param>
        /// <param name="value">The value to submit with the form</param>
        /// <param name="content">The text the user sees as a templated razor delegate</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Submit(string name, string value, Func<dynamic, IHtmlContent> content);

        /// <summary>
        /// Creates the HTML for a &lt;button&gt;.
        /// </summary>
        /// <param name="text">The text to display in the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Button(string text);

        /// <summary>
        /// Creates the HTML for a &lt;button&gt;.
        /// </summary>
        /// <param name="content">The content to display in the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Button(IHtmlContent content);

        /// <summary>
        /// Creates the HTML for a &lt;button&gt;.
        /// </summary>
        /// <param name="content">The content to display in the button as a templated razor delegate</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Button(Func<dynamic, IHtmlContent> content);

        /// <summary>
        /// Creates the HTML for a reset &lt;button&gt;.
        /// </summary>
        /// <param name="text">The text to display for the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Reset(string text);

        /// <summary>
        /// Creates the HTML for a reset &lt;button&gt;.
        /// </summary>
        /// <param name="content">The content to display for the button</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Reset(IHtmlContent content);

        /// <summary>
        /// Creates the HTML for a reset &lt;button&gt;.
        /// </summary>
        /// <param name="content">The content to display for the button as a templated razor delegate</param>
        /// <returns>Html attributes class to chain modifications to the button's attributes; call .ToHtmlString() to generate the button HTML</returns>
        public ButtonHtmlAttributes Reset(Func<dynamic, IHtmlContent> content);
    }
```
The start and end HTML of the Navigation are generated via the `BeginNavigation` and `EndNavigation` methods in the [form template](form-templates.md). The HTML for the various types of buttons are all generated via the `Button` method in the template.

## Default usage

# [Tag Helpers variant](#tab/navigation-th)

In order to get a navigation you can use the `<form-navigation>` tag helper, e.g.:

```cshtml
<form-navigation>
    @* Navigation elements go here *@
</form-navigation>
```

# [HTML Helpers variant](#tab/navigation-hh)

In order to get an instance of a `Navigation<TModel>` you can use the `BeginNavigation` method on the Form, e.g.:

```cshtml
@using (var n = f.BeginNavigation()) {
    @* Navigation elements go here *@
}
```

The `BeginNavigation` extension method looks like this:

```cs
        /// <summary>
        /// Creates a navigation section.
        /// </summary>
        /// <example>
        /// @using (var n = f.BeginNavigation()) {
        ///     @n.Submit("Previous", "previous")
        ///     @n.Submit("Save", "save")
        ///     @n.Submit("Next", "next")
        /// }
        /// </example>
        /// <typeparam name="TModel">The view model type for the current view</typeparam>        
        /// <param name="form">The form the navigation is being created in</param>
        /// <returns>The form navigation</returns>
        public static Navigation<TModel> BeginNavigation<TModel>(this IForm<TModel> form)
        {
            return new Navigation<TModel>(form);
        }
```


***

## Buttons

From within a section you can create Navigation Submit, Reset and normal Buttons.

# [Tag Helpers variant](#tab/buttons-th)

You can create buttons by using the `<form-button>`, `<reset-button>` and `<submit-button>` tag helpers within a `<form-navigation>` tag helper.

```cshtml
<form-navigation>
    <form-button add-class="button" id="button1" label="text button" />
    <form-button add-class="button" id="button2"><strong>html button</strong></form-button>
    <reset-button add-class="button" id="button3" label="text reset" />
    <reset-button add-class="button" id="button4"><strong>html reset</strong></reset-button>
    <submit-button add-class="button" id="button5" label="text reset" />
    <submit-button add-class="button" id="button6"><strong>html reset</strong></reset-button>
    <submit-button name="name" value="value" add-class="button" id="button7"><strong>html submit with value</strong></submit-button>
</form-navigation>
```

# [HTML Helpers variant](#tab/buttons-hh)

You can create buttons by using the methods on the `Navigation<T>` object. You can chain [HTML Attributes](html-attributes.md) specifiers off the end of the buttons you create, e.g.:

```cshtml
@using (var n = f.BeginNavigation()) {
    @n.Button("text button").AddClass("button").Id("button1")
    @n.Button(new HtmlString("<strong>html button</strong>")).AddClass("button").Id("button2")
    @n.Button(@<strong>html button</strong>)
    @n.Reset("text reset").AddClass("button").Id("button3")
    @n.Reset(new HtmlString("<strong>html reset</strong>")).AddClass("button").Id("button4")
    @n.Reset(@<strong>html reset</strong>)
    @n.Submit(new HtmlString("<strong>html submit</strong>")).AddClass("button").Id("button5")
    @n.Submit("text submit").AddClass("button").Id("button6")
    @n.Submit("name", "value", new HtmlString("<strong>html submit with value</strong>")).AddClass("button").Id("button7")
    @n.Submit("name", "value").AddClass("button").Id("button8")
    @n.Submit(@<strong>html submit</strong>)
}
```

***

## Extending Navigation Buttons

See [the HTML Attributes documentation](html-attributes.md#extending-navigation-buttons-specifically) for more information.

## Default HTML

### Begin HTML

```html
        <div class="form_navigation">
```

### End HTML

```html
        </div>
```

### Button HTML

```html
<button (%htmlAttributes%)>%content%</button>
```

### Reset HTML

```html
<button type="reset" (%htmlAttributes%)>%content%</button>
```

### Submit HTML

```html
<button type="submit" (%htmlAttributes%)>%content%</button>
```

If you specify a name and value to submit when the form is submitted via the button and you don't provide content then the HTML will be ([see why](http://rommelsantor.com/clog/2012/03/12/fixing-the-ie7-submit-value/)):

```html
<input type="submit" name="%name%" id="%name%" value="%value%" />
```

If you specify a name, value and content the HTML will be (if you call this method you are opting out of IE7 support for capturing the submitted value on the server-side):

```html
<button type="submit" name="%name%" id="%name%" value="%value%" (%htmlAttributes%)>%content%</button>
```

Twitter Bootstrap 3 HTML
------------------------

### Begin HTML

```html
        <div class="btn-group">
```

### End HTML

```html
        </div>
```

### Button/Reset/Submit HTML

The HTML is the same as the default except a class of `btn` will always be added and if no emphasis classes are added (see below) then a class of `btn-default` will be added.

### Add emphasis style

There is an extension method in the `ChameleonForms.Templates.TwitterBootstrap3` namespace that allows you to add an emphasis style to the buttons:

```cs
        /// <summary>
        /// Adds the given emphasis to the button.
        /// </summary>
        /// <example>
        /// @n.Submit("Submit").WithStyle(EmphasisStyle.Warning)
        /// </example>
        /// <param name="attrs">The Html Attributes from a navigation button</param>
        /// <param name="style">The style of button</param>
        /// <returns>The Html Attribute object so other methods can be chained off of it</returns>
        public static ButtonHtmlAttributes WithStyle(this ButtonHtmlAttributes attrs, EmphasisStyle style)
        {
            attrs.AddClass(string.Format("btn-{0}", style.ToString().ToLower()));
            return attrs;
        }
```

The `EmphasisStyle` enum is as follows:

```cs
    /// <summary>
    /// Twitter Bootstrap alert/emphasis colors: http://getbootstrap.com/css/#type-emphasis
    /// </summary>
    public enum EmphasisStyle
    {
        /// <summary>
        /// Default styling.
        /// </summary>
        Default,
        /// <summary>
        /// Primary action styling.
        /// </summary>
        Primary,
        /// <summary>
        /// Success styling.
        /// </summary>
        Success,
        /// <summary>
        /// Information styling.
        /// </summary>
        Info,
        /// <summary>
        /// Warning styling.
        /// </summary>
        Warning,
        /// <summary>
        /// Danger styling.
        /// </summary>
        Danger
    }
```


# [Tag Helpers variant](#tab/emphasis-style-th)

There is a tag helper in the `ChameleonForms.Templates.TwitterBootstrap3` namespace that adds an `emphasis-style` property to the `<form-button>`, `<reset-button>` and `<submit-button>` tag helpers. Or, you can make use of the `fluent-attrs` property on those tag helpers.

To use the custom tag helper you need to import the tag helper within your `_ViewImports.cshtml` or equivalent file:

```cshtml
@addTagHelper ChameleonForms.Templates.TwitterBootstrap3.*, ChameleonForms
```

Here is an example of its use:

```cshtml
<form-navigation>
    <submit-button label="Submit" emphasis-style="Primary" />
    @* or *@
    <submit-button label="Submit" fluent-attrs='c => c.WithStyle(EmphasisStyle.Primary)' />
</form-navigation>
```

# [HTML Helpers variant](#tab/emphasis-style-hh)

You can use the extension method like this:

```cshtml
@using (var n = f.BeginNavigation()) {
    @n.Submit("Submit").WithStyle(EmphasisStyle.Primary)
}
```

In order to be able to swap out the extension method usage across your application easily if you change your form template we recommend that rather than adding a using statement to `ChameleonForms.Templates.TwitterBootstrap3` for each view that has a form using the extension method you instead add the namespace to your `_ViewImports.cshtml` or equivalent file.

***


Which would result in:

```html
<div class="btn-group">
    <button type="submit" class="btn btn-primary">Submit</button>
</div>
```

### Change button size

There is an extension method in the `ChameleonForms.Templates.TwitterBootstrap3` namespace that allows you to change the size of your buttons:

```cs
        /// <summary>
        /// Changes the button to use the given size.
        /// </summary>
        /// <example>
        /// @n.Submit("Submit").WithSize(ButtonSize.Large)
        /// </example>
        /// <param name="attrs">The Html Attributes from a navigation button</param>
        /// <param name="size">The size of button</param>
        /// <returns>The Html Attribute object so other methods can be chained off of it</returns>
        public static ButtonHtmlAttributes WithSize(this ButtonHtmlAttributes attrs, ButtonSize size)
        {
            if (size != ButtonSize.Default)
                attrs.AddClass(string.Format("btn-{0}", size.Humanize()));
            return attrs;
        }
```

The `ButtonSize` enum is as follows:

```cs
    /// <summary>
    /// Twitter Bootstrap button sizes: http://getbootstrap.com/css/#buttons-sizes
    /// </summary>
    public enum ButtonSize
    {
        /// <summary>
        /// Extra small button size.
        /// </summary>
        [Description("xs")]
        ExtraSmall,
        /// <summary>
        /// Small button size.
        /// </summary>
        [Description("sm")]
        Small,
        /// <summary>
        /// Default button size.
        /// </summary>
        Default,
        /// <summary>
        /// Large button size.
        /// </summary>
        [Description("lg")]
        Large
    }
```

# [Tag Helpers variant](#tab/button-size-th)

There is a tag helper in the `ChameleonForms.Templates.TwitterBootstrap3` namespace that adds an `size` property to the `<form-button>`, `<reset-button>` and `<submit-button>` tag helpers. Or, you can make use of the `fluent-attrs` property on those tag helpers.

To use the custom tag helper you need to import the tag helper within your `_ViewImports.cshtml` or equivalent file:

```cshtml
@addTagHelper ChameleonForms.Templates.TwitterBootstrap3.*, ChameleonForms
```

Here is an example of its use:

```cshtml
<form-navigation>
    <submit-button label="Submit" size="Large" />
    @* or *@
    <submit-button label="Submit" fluent-attrs='c => c.WithSize(ButtonSize.Large)' />
</form-navigation>
```

# [HTML Helpers variant](#tab/button-size-hh)

You can use the extension method like this:

```cshtml
@using (var n = f.BeginNavigation()) {
    @n.Submit("Submit").WithSize(ButtonSize.Large)
}
```

In order to be able to swap out the extension method usage across your application easily if you change your form template we recommend that rather than adding a using statement to `ChameleonForms.Templates.TwitterBootstrap3` for each view that has a form using the extension method you instead add the namespace to your `_ViewImports.cshtml` or equivalent file.

***

Which would result in:

```html
<div class="btn-group">
    <button type="submit" class="btn btn-default btn-lg">Submit</button>
</div>
```

### Add icon to button

There is an extension method in the `ChameleonForms.Templates.TwitterBootstrap3` namespace that allows you to add icons to your buttons:

```cs
        /// <summary>
        /// Adds the given icon to the start of a navigation button.
        /// </summary>
        /// <example>
        /// @n.Submit("Submit").WithIcon("arrow-right")
        /// // Output:
        /// &lt;button type="submit">&lt;span class="glyphicon glyphicon-arrow-right">&lt;/span> Submit&lt;/button>
        /// </example>
        /// <param name="attrs">The Html Attributes from a navigation button</param>
        /// <param name="icon">The icon to use; see https://getbootstrap.com/docs/3.3/components/#glyphicons</param>
        /// <returns>The Html Attribute object so other methods can be chained off of it</returns>
        public static ButtonHtmlAttributes WithIcon(this ButtonHtmlAttributes attrs, string icon)
        {
            attrs.Attr(TwitterBootstrap3FormTemplate.IconAttrKey, icon);
            return attrs;
        }
```

You can see the list of possible icon names to choose from on the [Twitter Bootstrap documentation site](https://getbootstrap.com/docs/3.3/components/#glyphicons) (drop the `glyphicon-` from the icon names on this page e.g. use `adjust` instead of `glyphicon-adjust`).

# [Tag Helpers variant](#tab/button-icon-th)

There is a tag helper in the `ChameleonForms.Templates.TwitterBootstrap3` namespace that adds an `icon` property to the `<form-button>`, `<reset-button>` and `<submit-button>` tag helpers. Or, you can make use of the `fluent-attrs` property on those tag helpers.

To use the custom tag helper you need to import the tag helper within your `_ViewImports.cshtml` or equivalent file:

```cshtml
@addTagHelper ChameleonForms.Templates.TwitterBootstrap3.*, ChameleonForms
```

Here is an example of its use:

```cshtml
<form-navigation>
    <submit-button label="Submit" icon="adjust" />
    @* or *@
    <submit-button label="Submit" fluent-attrs='c => c.WithIcon("adjust")' />
</form-navigation>
```

# [HTML Helpers variant](#tab/button-icon-hh)

You can use the extension method like this:

```cshtml
@using (var n = f.BeginNavigation()) {
    @n.Submit("Submit").WithIcon("adjust")
}
```

In order to be able to swap out the extension method usage across your application easily if you change your form template we recommend that rather than adding a using statement to `ChameleonForms.Templates.TwitterBootstrap3` for each view that has a form using the extension method you instead add the namespace to your `_ViewImports.cshtml` or equivalent file.

***

Which would result in:

```html
<div class="btn-group">
    <button type="submit" class="btn btn-default"><span class="glyphicon glyphicon-adjust"></span> Submit</button>
</div>
```

### Example

Here is an example from the example project of what the buttons can look like:

![Screenshot of buttons generated using Twitter Bootstrap 3 template](bootstrap-buttons.png)

Here is the code that generated the above screenshot:

# [Tag Helpers variant](#tab/example-th)

```cshtml
<form-navigation>
    <form-button label="text button" emphasis-style="Primary" size="Default" />
    <form-button add-class="random-class"><strong>html button</strong></form-button>
    <reset-button label="text reset" icon="refresh" />
    <reset-button><strong>html reset</strong></reset-button>
    <submit-button><strong>html submit</strong></submit-button>
    <submit-button label="text submit" emphasis-style="Danger" />
    <submit-button name="name" value="value"><strong>html submit with value</strong></submit-button>
    <submit-button name="name" value="value" label="value" icon="star" emphasis-style="Success" />
</form-navigation>

<form-navigation>
    <form-button label="Small button 1" size="Small" />
    <form-button label="Small button 2" fluent-attrs='c => c.WithSize(ButtonSize.Small)' />
</form-navigation>

<form-navigation>
    <form-button label="Extra small button 1" size="ExtraSmall" />
    <form-button label="Extra small button 2" fluent-attrs='c => c.WithSize(ButtonSize.ExtraSmall)' />
</form-navigation>

<form-navigation>
    <form-button label="Large button 1" size="Large" />
    <form-button label="Large button 2" fluent-attrs='c => c.WithSize(ButtonSize.Large)' />
</form-navigation>
```


# [HTML Helpers variant](#tab/example-hh)

```cshtml
@using (var n = f.BeginNavigation())
{
    @n.Button("text button").WithStyle(EmphasisStyle.Primary).WithSize(ButtonSize.Default)
    @n.Button(new HtmlString("<strong>html button</strong>")).AddClass("random-class")
    @n.Reset("text reset").WithIcon("refresh")
    @n.Reset(new HtmlString("<strong>html reset</strong>"))
    @n.Submit(new HtmlString("<strong>html submit</strong>"))
    @n.Submit("text submit").WithStyle(EmphasisStyle.Danger)
    @n.Submit("name", "value", new HtmlString("<strong>html submit with value</strong>"))
    @n.Submit("name", "value").WithIcon("star").WithStyle(EmphasisStyle.Success)
}

@using (var n = f.BeginNavigation())
{
    @n.Button("Small button 1").WithSize(ButtonSize.Small)
    @n.Button("Small button 2").WithSize(ButtonSize.Small)
}

@using (var n = f.BeginNavigation())
{
    @n.Button("Extra small button 1").WithSize(ButtonSize.ExtraSmall)
    @n.Button("Extra small button 2").WithSize(ButtonSize.ExtraSmall)
}

@using (var n = f.BeginNavigation())
{
    @n.Button("Large button 1").WithSize(ButtonSize.Large)
    @n.Button("Large button 2").WithSize(ButtonSize.Large)
}
```

***
