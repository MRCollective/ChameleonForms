HTML Attributes
===============

HTML Attributes in ChameleonForms provides the ability to specify a set of HTML attributes in a fluent, expressive way. Specifying HTML Attributes is done by chaining calls to the methods on the `HtmlAttributes` class.

The `HtmlAttributes` class looks like this and is in the `ChameleonForms` namespace:

```csharp
    /// <summary>
    /// Represents a set of HTML attributes.
    /// </summary>
    public class HtmlAttributes : IHtmlContent
    {
        /// <summary>
        /// Dictionary of the attributes currently stored in the object.
        /// </summary>
        public IDictionary<string, string> Attributes { get; }

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
        /// Implicitly convert from a dictionary to a new <see cref="HtmlAttributes"/> object.
        /// </summary>
        /// <param name="attributes">The dictionary of HTML attributes</param>
        /// <returns>The new <see cref="HtmlAttributes"/> object</returns>
        public static implicit operator HtmlAttributes(Dictionary<string, object> attributes);

        /// <summary>
        /// Returns the HTML attributes as a dictionary.
        /// </summary>
        /// <returns>A dictionary of HTML attributes compatible with the standard ASP.NET MVC method signatures</returns>
        public IDictionary<string, object> ToDictionary();
    }
```

The xmldoc comments above should give a pretty good indication of how each of those methods are meant to be used.

The [Field Configuration](field-configuration) wraps a HTML Attributes object and a lot of these methods also appear on that interface. The HTML Attributes can also be passed into the [Form](the-form) and the [Section](the-section) and can be chained from [Navigation Buttons](the-navigation).

Default Usage
-------------

There are a number of choices when using HTML Attributes.

### Chaining

If you are interacting with a method that returns a HTML Attributes object then you can simply chain method calls, e.g.:

```csharp
using (var n = f.BeginNavigation()) {
    @n.Submit("Submit").Attr("data-something", "value").AddClass("a-class").Id("buttonId")
}
```

### Instantiation

You can new up an instance and use one of the three constructors (pass in an anonymous object, pass in a dictionary, or use lambda expressions as per below), e.g.:

```csharp
@using (var f = Html.BeginChameleonForm(htmlAttributes: new HtmlAttributes(@class => "form", id => "someForm")) {
    @* ... *@
}
```

If you want to output a HTML Attribute that has a `-` in the name then use a `_` in the variable name, e.g.:

```csharp
new HtmlAttributes(data_something => "value")
```

### Instantiation with chaining

You can new up an instance and then chain methods off that instance, e.g.:

```csharp
@using (var f = Html.BeginChameleonForm(htmlAttributes: new HtmlAttributes().AddClass("form").Id("someForm")) {
    @* ... *@
}
```

### Anonymous Object

You can convert an anonymous object to a HTML Attributes object, e.g.:

```csharp
@using (var f = Html.BeginChameleonForm(htmlAttributes: new { @class="form", id="someForm" }.ToHtmlAttributes())) {
    @* ... *@
}
```

If you want to output a HTML Attribute that has a `-` in the name then use a `_` in the property name, e.g.:

```csharp
new {data_something => "value"}.ToHtmlAttributes()
```

### Dictionary

You can convert a dictionary to a HTML Attributes object, e.g.:

```csharp
@using (var f = Html.BeginChameleonForm(htmlAttributes: new Dictionary<string, object>{ {"class", "form"}, {"id", "someForm"} }.ToHtmlAttributes())) {
    @* ... *@
}
```

Outputting HTML Attributes
--------------------------

There are a number of options when using a HTML Attributes object.

### Use the attributes in a tag builder

You can use the HTML Attributes object with the `TagBuilder` class in MVC, e.g.:

```csharp
var h = new HtmlAttributes().Id("id");
var t = new TagBuilder("p");
t.MergeAttributes(h.Attributes);
```

### Output it directly to the page

You may notice that the `HTMLAttributes` definition above extends `IHtmlContent`. As you might expect, this means you can directly output it to the page, e.g.

```html
@{
    var h = new HtmlAttributes().Id("id");
}
<p @h>Text</p>
```

### Use the attributes as a dictionary

When you need ultimate flexibility then you can get the attributes out as a dictionary, e.g.:

```csharp
var h = new HtmlAttributes().Id("id");
var d1 = h.Attributes; // Dictionary<string, string>
var d2 = h.ToDictionary(); // Dictionary<string, object>, most MVC methods take this type
```

Extending HTML Attributes
-------------------------

You can easily create your own methods on HTML Attributes by creating an extension method, e.g.:

```csharp
public static HtmlAttributesExtensions 
{
    public static HtmlAttributes Coordinate(this HtmlAttributes attrs, Coordinate c)
    {
        return attrs.Attr(data_coordinate => JsonConvert.Serialize(c));
    }
}
```

Then you could do:

```csharp
using (var n = f.BeginNavigation()) {
    @n.Button(Model.Coordinate1.ToString()).Coordinate(Model.Coordinate1)
    @n.Button(Model.Coordinate2.ToString()).Coordinate(Model.Coordinate2)
    @n.Button(Model.Coordinate3.ToString()).Coordinate(Model.Coordinate2)
}
```

### Extending Navigation Buttons specifically

[Navigation Buttons](the-navigation) return a sub-class of `HtmlAttributes` called `ButtonHtmlAttributes`. This allows you to target extension methods specifically for buttons, which must then be invoked first before any of the methods targeting the more general `HtmlAttributes`. As an example consider the following extension method in the Twitter Bootstrap 3 template to add a size to the button:

```csharp
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

Create methods that chain HTML Attributes
-----------------------------------------

Returning a HTML Attributes object from a method so that the user can chain attribute methods off it before outputting it in a view (like the [Navigation Buttons](the-navigation)) can be tricky by default, so ChameleonForms provides a special way to handle this situation.

If the HTML that you are outputting relies on the HTML Attributes to be defined, then you don't want to generate it until after the final chaining call is made. Luckily, you know when the final call is made because MVC will call the `ToHtmlString` method for you (since HtmlAttributes overrides the `IHtmlContent` interface).

The only remaining problem is that you don't have control over the code in `ToHtmlString` since it's inside ChameleonForms, and in fact the `ToHtmlString` method returns the HTML for the attributes by default as shown above.

The class you need to use in this case is `LazyHtmlAttributes`, which is in the `ChameleonForms` namespace. If you new up one of those and return it (but make the method return type `HtmlAttributes` then you have the ability to define what HTML is output when the `ToHtmlString` method is called.

### Simple example

If you created the following extension method on the HTML Helper:

```csharp
    public static class HtmlExtensions
    {
        public static HtmlAttributes Paragraph(this HtmlHelper h, string paragraphText)
        {
            return new LazyHtmlAttributes(a =>
                {
                    var t = new TagBuilder("p");
                    t.SetInnerText(paragraphText);
                    t.MergeAttributes(a.Attributes);
                    return new HtmlString(t.ToString());
                }
            );
        }
    }
```

Then if you put the following in your view:

```csharp
@Html.Paragraph("Display some text").Id("paragraphId").AddClass("a-class").Attr(data_some_data => "{mydata:true}")
```

It would output the following HTML:

```html
<p class="a-class" data-some-data="{mydata:true}" id="paragraphId">Display some text</p>
```