# HTML Attributes

HTML Attributes in ChameleonForms provides the ability to specify a set of HTML attributes in a fluent, expressive way. Specifying HTML Attributes is done by chaining calls to the methods on the `HtmlAttributes` class or by [adding equivalent attributes to one of the supported tag helpers](#tag-helper-attributes).

The `HtmlAttributes` class looks like this and is in the `ChameleonForms` namespace:

```cs
    /// <summary>
    /// Represents a set of HTML attributes.
    /// </summary>
    public class HtmlAttributes : IHtmlContent
    {
        /// <summary>
        /// Dictionary of the attributes currently stored in the object.
        /// </summary>
        public IDictionary<string, string> Attributes { get { return _tagBuilder.Attributes; } }

        /// <summary>
        /// Constructs a <see cref="HtmlAttributes"/> object using lambda methods to express the attributes.
        /// </summary>
        /// <example>
        /// var h = new HtmlAttributes(style => "width: 100%;", cellpadding => 0, @class => "class1 class2", src => "http://url/", data_somedata => "\"rubbi&amp;h\"");
        /// </example>
        /// <param name="attributes">A list of lambas where the lambda variable name is the name of the attribute and the value is the value</param>
        public HtmlAttributes(params Func<object, object>[] attributes);

        /// <summary>
        /// Constructs a <see cref="HtmlAttributes"/> object using a dictionary to express the attributes.
        /// </summary>
        /// <example>
        /// var h = new HtmlAttributes(new Dictionary&lt;string, object&gt; {{"style", "width: 100%;"}, {"cellpadding", 0}, {"class", "class1 class2"}, {"src", "http://url/"}, {"data-somedata", "\"rubbi&amp;h\""}});
        /// </example>
        /// <param name="attributes">A dictionary of attributes</param>
        public HtmlAttributes(IDictionary<string, object> attributes);

        /// <summary>
        /// Constructs a <see cref="HtmlAttributes"/> object using a dictionary to express the attributes.
        /// </summary>
        /// <example>
        /// var h = new HtmlAttributes(new Dictionary&lt;string, string&gt; {{"style", "width: 100%;"}, {"cellpadding", "0"}, {"class", "class1 class2"}, {"src", "http://url/"}, {"data-somedata", "\"rubbi&amp;h\""}});
        /// </example>
        /// <param name="attributes">A dictionary of attributes</param>
        public HtmlAttributes(IDictionary<string, string> attributes);

        /// <summary>
        /// Constructs a <see cref="HtmlAttributes"/> object using an anonymous object to express the attributes.
        /// </summary>
        /// <example>
        /// var h = new HtmlAttributes(new { style = "width: 100%;", cellpadding = 0, @class = "class1 class2", src = "http://url/", data_somedata = "\"rubbi&amp;h\"" });
        /// </example>
        /// <param name="attributes">An anonymous object of attributes</param>
        public HtmlAttributes(object attributes);

        /// <summary>
        /// Adds a CSS class (or a number of CSS classes) to the attributes.
        /// </summary>
        /// <param name="class">The CSS class(es) to add</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes AddClass(string @class);
        
        /// <summary>
        /// Set the id attribute.
        /// </summary>
        /// <param name="id">The text to use for the id</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Id(string id);

        /// <summary>
        /// Sets the disabled attribute.
        /// </summary>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Disabled(bool disabled = true);

        /// <summary>
        /// Sets the readonly attribute.
        /// </summary>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Readonly(bool @readonly = true);

        /// <summary>
        /// Sets the required attribute.
        /// </summary>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Required(bool required = true);

        /// <summary>
        /// Returns whether or not a value is set for the given attribute.
        /// </summary>
        /// <param name="key">The name of the HTML attribute to check</param>
        /// <returns>Whether or not there is a value set for the attribute</returns>
        public bool Has(string key);

        /// <summary>
        /// Adds or updates a HTML attribute with a given value.
        /// </summary>
        /// <param name="key">The name of the HTML attribute to add/update</param>
        /// <param name="value">The value of the HTML attribute to add/update</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attr(string key, object value);

        /// <summary>
        /// Adds or updates a HTML attribute with using a lambda method to express the attribute.
        /// </summary>
        /// <example>
        /// h.Attr(style => "width: 100%;")
        /// </example>
        /// <param name="attribute">A lambda expression representing the attribute to set and its value</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attr(Func<object, object> attribute);

        /// <summary>
        /// Adds or updates a set of HTML attributes using lambda methods to express the attributes.
        /// </summary>
        /// <param name="attributes">A list of lambas where the lambda variable name is the name of the attribute and the value is the value</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attrs(params Func<object, object>[] attributes);

        /// <summary>
        /// Adds or updates a set of HTML attributes using a dictionary to express the attributes.
        /// </summary>
        /// <param name="attributes">A dictionary of attributes</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attrs(IDictionary<string, object> attributes);

        /// <summary>
        /// Adds or updates a set of HTML attributes using anonymous objects to express the attributes.
        /// </summary>
        /// <param name="attributes">An anonymous object of attributes</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attrs(object attributes);

        /// <summary>
        /// Adds or updates a set of HTML attributes using a dictionary to express the attributes.
        /// </summary>
        /// <param name="attributes">A dictionary of attributes</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attrs(IDictionary<string, string> attributes);

        /// <summary>
        /// Implicitly convert from a dictionary to a new <see cref="HtmlAttributes"/> object.
        /// </summary>
        /// <param name="attributes">The dictionary of HTML attributes</param>
        /// <returns>The new <see cref="HtmlAttributes"/> object</returns>
        public static implicit operator HtmlAttributes(Dictionary<string, object> attributes);

        /// <summary>
        /// Implicitly convert from a dictionary to a new <see cref="HtmlAttributes"/> object.
        /// </summary>
        /// <param name="attributes">The dictionary of HTML attributes</param>
        /// <returns>The new <see cref="HtmlAttributes"/> object</returns>
        public static implicit operator HtmlAttributes(Dictionary<string, string> attributes);

        /// <inheritdoc />
        public virtual void WriteTo(TextWriter writer, HtmlEncoder encoder);

        /// <summary>
        /// Returns the HTML attributes as a dictionary.
        /// </summary>
        /// <returns>A dictionary of HTML attributes compatible with the standard ASP.NET MVC method signatures</returns>
        public IDictionary<string, object> ToDictionary();
    }
```

The xmldoc comments above should give a pretty good indication of how each of those methods are meant to be used.

The [Field Configuration](field-configuration.md) wraps a HTML Attributes object and a lot of these methods also appear on that interface. The HTML Attributes can also be passed into the [Form](the-form.md) and the [Section](the-section.md) and can be chained from [Navigation Buttons](the-navigation.md).

## Default Usage

There are a number of choices when using HTML Attributes.

### Tag Helper attributes

Most `HTMLAttributes` methods map to a tag helper attribute by convention - `UpperCamelCase` to `upper-camel-case` (i.e. kebab case). They are all available on the tag helpers that [support HTML Attributes](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/TagHelpers/HtmlAttributesTagHelper.cs#L10):

* `chameleon-form`
* `form-section`
* `form-button`
* `submit-button`
* `reset-button`

| HTML Attributes Method                                | Equivalent Tag Helper attribute                       |
|-------------------------------------------------------|-------------------------------------------------------|
| `Id(string id)`                                       | `id="{id}"`                                           |
| `AddClass(string @class)`                             | `add-class="{class}"`                                 |
| `Attr(string key, object value)`                      | `attr-{key}="{value}"`                                |
| `Attr(Func<object, object> attribute)`                | *No equivalent*                                       |
| `Attrs(params Func<object, object>[] attributes)`     | *No equivalent*                                       |
| `Attrs(IDictionary<string, object> attributes)`       | *No equivalent*                                       |
| `Attrs(IDictionary<string, string> attributes)`       | `attrs="{attributes}"`                                |
| `Attrs(object attributes)`                            | *No equivalent*                                       |
| `Disabled(bool disabled = true)`                      | `disabled="{disabled}"`                               |
| `Readonly(bool @readonly = true)`                     | *No equivalent*                                       |
| `Required(bool required = true)`                      | *No equivalent*                                       |

### Chaining

If you are interacting with a method that returns a HTML Attributes object then you can simply chain method calls, e.g.:

# [Tag Helpers variant](#tab/chaining-th)

```cshtml
<form-navigation>
    <submit-button fluent-config='c => c.Attr("data-something", "value").AddClass("a-class").Id("buttonId")'>
</form-navigation>
```

# [HTML Helpers variant](#tab/chaining-hh)

```cshtml
@using (var n = f.BeginNavigation()) {
    @n.Submit("Submit").Attr("data-something", "value").AddClass("a-class").Id("buttonId")
}
```

***


### Instantiation

You can new up an instance and use one of the four constructors (empty constructor and method chaining, pass in an anonymous object, pass in a dictionary, or use lambda expressions.

### Instantiation with empty constructor and method chaining

You can new up an instance and then chain methods off that instance, e.g.:

```cs
new HtmlAttributes().AddClass("form").Id("someForm")
```

### Instantiation with lambda expressions

```cs
new HtmlAttributes(@class => "form", id => "someForm")
```

If you want to output a HTML Attribute that has a `-` in the name then use a `_` in the variable name, e.g.:

```cs
new HtmlAttributes(data_something => "value")
```

### Instantiation with anonymous object

You can convert an anonymous object to a HTML Attributes object, e.g.:

```cs
new { @class="form", id="someForm" }.ToHtmlAttributes()
```

If you want to output a HTML Attribute that has a `-` in the name then use a `_` in the property name, e.g.:

```cs
new {data_something => "value"}.ToHtmlAttributes()
```

### Instantiation with dictionary

You can convert a dictionary to a HTML Attributes object, e.g.:

```cs
new Dictionary<string, object>{ {"class", "form"}, {"id", "someForm"} }.ToHtmlAttributes()
new Dictionary<string, string>{ {"class", "form"}, {"id", "someForm"} }.ToHtmlAttributes()
```

## Outputting HTML Attributes

There are a number of options when using a HTML Attributes object.

### Use the attributes in a tag builder

You can use the HTML Attributes object with the `TagBuilder` class in MVC, e.g.:

```cs
var h = new HtmlAttributes().Id("id");
var t = new TagBuilder("p");
t.MergeAttributes(h.Attributes);
```

### Output directly to the page

You may notice that the `HTMLAttributes` definition above extends `IHtmlContent`. As you might expect, this means you can directly output it to the page, e.g.

```cshtml
@{
    var h = new HtmlAttributes().Id("id");
}
<p @h>Text</p>
```

It will automatically handle encoding attribute values to prevent HTML injection.

### Use the attributes as a dictionary

When you need ultimate flexibility then you can get the attributes out as a dictionary, e.g.:

```cs
var h = new HtmlAttributes().Id("id");
var d1 = h.Attributes; // Dictionary<string, string>
var d2 = h.ToDictionary(); // Dictionary<string, object>, many MVC methods take this type
```

### Retrieve the attributes as an encoded string

If you need to retrive the attribute values as a string (already encoded), then you can use the `.ToHtmlString()` extension method.

```cs
var h = new HtmlAttributes().Id("id&1");
string s = h.ToHtmlString(); // "id=\"id&amp;1\""
```

## Extending HTML Attributes

You can easily create your own methods on HTML Attributes by creating an extension method, e.g.:

```cs
public static HtmlAttributesExtensions 
{
    public static HtmlAttributes Coordinate(this HtmlAttributes attrs, Coordinate c)
    {
        return attrs.Attr(data_coordinate => JsonConvert.Serialize(c));
    }
}
```

Then you could do something like this:

```cshtml
@using (var n = f.BeginNavigation()) {
    @n.Button(Model.Coordinate1.ToString()).Coordinate(Model.Coordinate1)
    @n.Button(Model.Coordinate2.ToString()).Coordinate(Model.Coordinate2)
    @n.Button(Model.Coordinate3.ToString()).Coordinate(Model.Coordinate2)
}
```

### Extending Navigation Buttons specifically

[Navigation Buttons](the-navigation.md) return a sub-class of `HtmlAttributes` called `ButtonHtmlAttributes`. This allows you to target extension methods specifically for buttons, which must then be invoked first before any of the methods targeting the more general `HtmlAttributes`. As an example consider the following extension method in the Twitter Bootstrap 3 template to add a size to the button:

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

If you want to consume these extension methods on a button based tag helper you have two options:

1. Use the `fluent-attrs` attribute e.g. `<submit-button label="Submit" fluent-attrs='a => a.WithSize(ButtonSize.Large).WithStyle(EmphasisStyle.Info).WithIcon("calendar")' />`
2. Use a tag helper that adds the attributes before the main tag helper gets processed, such as the [Bootstrap 3 `ButtonTagHelper`](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/Templates/TwitterBootstrap3/ButtonTagHelper.cs) e.g. `<submit-button label="Submit" size="Large" emphasis-style="Info" icon="calendar" />`

## Create methods that chain HTML Attributes

Returning a HTML Attributes object from a method so that the user can chain attribute methods off it before outputting it in a view (like the [Navigation Buttons](the-navigation.md)) can be tricky by default, so ChameleonForms provides a special way to handle this situation.

If the HTML that you are outputting relies on the HTML Attributes to be defined, then you don't want to generate it until after the final chaining call is made. Luckily, you know when the final call is made because MVC will call the `ToHtmlString` method for you (since HtmlAttributes overrides the `IHtmlContent` interface).

The only remaining problem is that you don't have control over the code in `ToHtmlString` since it's inside ChameleonForms, and in fact the `ToHtmlString` method returns the HTML for the attributes by default as shown above.

The class you need to use in this case is `LazyHtmlAttributes`, which is in the `ChameleonForms` namespace. If you new up one of those and return it (but make the method return type `HtmlAttributes` then you have the ability to define what HTML is output when the `Write` method is called, but still allow `HtmlAttributes` method chaining until that happens.

### Simple example

If you created the following extension method on the HTML Helper:

```cs
    public static class HtmlExtensions
    {
        public static HtmlAttributes Paragraph(this HtmlHelper h, string paragraphText)
        {
            return new LazyHtmlAttributes(a =>
                {
                    var t = new TagBuilder("p");
                    t.SetInnerText(paragraphText);
                    t.MergeAttributes(a.Attributes);
                    return t;
                }
            );
        }
    }
```

In this example, the lambda expression passed into the constructor is called when the eventual call to `Write` on the `HtmlAttributes` object returned from `Paragraph` is called. You can see at that point in time we can safely use the `Attributes` property since we know all of the method chaining will be finished at that point in time.

In this case, if you put the following in your razor view:

```cshtml
@Html.Paragraph("Display some text").Id("paragraphId").AddClass("a-class").Attr(data_some_data => "{mydata:true}")
```

It would output the following HTML:

```html
<p class="a-class" data-some-data="{mydata:true}" id="paragraphId">Display some text</p>
```

Magic!
