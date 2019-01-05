using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;


namespace ChameleonForms
{
    /// <summary>
    /// Represents a set of HTML attributes.
    /// </summary>
    public class HtmlAttributes : IHtmlContent
    {
        private readonly TagBuilder _tagBuilder = new TagBuilder("p");

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
        public HtmlAttributes(params Func<object, object>[] attributes)
        {
            Attrs(attributes);
        }

        /// <summary>
        /// Constructs a <see cref="HtmlAttributes"/> object using a dictionary to express the attributes.
        /// </summary>
        /// <example>
        /// var h = new HtmlAttributes(new Dictionary&lt;string, object&gt; {{"style", "width: 100%;"}, {"cellpadding", 0}, {"class", "class1 class2"}, {"src", "http://url/"}, {"data-somedata", "\"rubbi&amp;h\""}});
        /// </example>
        /// <param name="attributes">A dictionary of attributes</param>
        public HtmlAttributes(IDictionary<string, object> attributes)
        {
            Attrs(attributes);
        }

        /// <summary>
        /// Constructs a <see cref="HtmlAttributes"/> object using an anonymous object to express the attributes.
        /// </summary>
        /// <example>
        /// var h = new HtmlAttributes(new { style = "width: 100%;", cellpadding = 0, @class = "class1 class2", src = "http://url/", data_somedata = "\"rubbi&amp;h\"" });
        /// </example>
        /// <param name="attributes">An anonymous object of attributes</param>
        public HtmlAttributes(object attributes)
        {
            Attrs(attributes);
        }

        /// <summary>
        /// Adds a CSS class (or a number of CSS classes) to the attributes.
        /// </summary>
        /// <param name="class">The CSS class(es) to add</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes AddClass(string @class)
        {
            _tagBuilder.AddCssClass(@class);

            return this;
        }
        
        /// <summary>
        /// Set the id attribute.
        /// </summary>
        /// <param name="id">The text to use for the id</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Id(string id)
        {
            Attr("id", TagBuilder.CreateSanitizedId(id, "_"));
            return this;
        }

        /// <summary>
        /// Sets the disabled attribute.
        /// </summary>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Disabled(bool disabled = true)
        {
            if (disabled)
                Attr("disabled", "disabled");
            return this;
        }

        /// <summary>
        /// Sets the readonly attribute.
        /// </summary>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Readonly(bool @readonly = true)
        {
            if (@readonly)
                Attr("readonly", "readonly");
            return this;
        }

        /// <summary>
        /// Adds or updates a HTML attribute with a given value.
        /// </summary>
        /// <param name="key">The name of the HTML attribute to add/update</param>
        /// <param name="value">The value of the HTML attribute to add/update</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attr(string key, object value)
        {
            _tagBuilder.MergeAttribute(key.ToLower(), value == null ? string.Empty : value.ToString(), true);

            return this;
        }

        /// <summary>
        /// Adds or updates a HTML attribute with using a lambda method to express the attribute.
        /// </summary>
        /// <example>
        /// h.Attr(style => "width: 100%;")
        /// </example>
        /// <param name="attribute">A lambda expression representing the attribute to set and its value</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attr(Func<object, object> attribute)
        {
            var item = attribute(null);
            _tagBuilder.MergeAttribute(attribute.Method.GetParameters()[0].Name.Replace("_", "-").ToLower(), item == null ? string.Empty : item.ToString(), true);

            return this;
        }

        /// <summary>
        /// Adds or updates a set of HTML attributes using lambda methods to express the attributes.
        /// </summary>
        /// <param name="attributes">A list of lambas where the lambda variable name is the name of the attribute and the value is the value</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attrs(params Func<object, object>[] attributes)
        {
            foreach (var func in attributes)
                Attr(func);

            return this;
        }

        /// <summary>
        /// Adds or updates a set of HTML attributes using a dictionary to express the attributes.
        /// </summary>
        /// <param name="attributes">A dictionary of attributes</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attrs(IDictionary<string, object> attributes)
        {
            attributes = attributes.ToDictionary(d => d.Key.ToLower(), d => d.Value);

            _tagBuilder.MergeAttributes(attributes, true);

            return this;
        }

        /// <summary>
        /// Adds or updates a set of HTML attributes using anonymous objects to express the attributes.
        /// </summary>
        /// <param name="attributes">An anonymous object of attributes</param>
        /// <returns>The <see cref="HtmlAttributes"/> attribute to allow for method chaining</returns>
        public HtmlAttributes Attrs(object attributes)
        {
            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes)
                .ToDictionary(d => d.Key.ToLower(), d => d.Value);
            _tagBuilder.MergeAttributes(attrs, true);

            return this;
        }

        /// <summary>
        /// Implicitly convert from a dictionary to a new <see cref="HtmlAttributes"/> object.
        /// </summary>
        /// <param name="attributes">The dictionary of HTML attributes</param>
        /// <returns>The new <see cref="HtmlAttributes"/> object</returns>
        public static implicit operator HtmlAttributes(Dictionary<string, object> attributes)
        {
            return new HtmlAttributes(attributes);
        }

        /// <inheritdoc />
        public virtual void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            foreach (var attr in _tagBuilder.Attributes)
            {
                writer.Write(string.Format(" {0}=\"{1}\"",
                    encoder.Encode(attr.Key),
                    encoder.Encode(attr.Value))
                );
            }
        }

        /// <summary>
        /// Returns the HTML attributes as a dictionary.
        /// </summary>
        /// <returns>A dictionary of HTML attributes compatible with the standard ASP.NET MVC method signatures</returns>
        public IDictionary<string, object> ToDictionary()
        {
            return _tagBuilder.Attributes.ToDictionary<KeyValuePair<string, string>, string, object>(attribute => attribute.Key, attribute => attribute.Value);
        }
    }

    /// <summary>
    /// Extension methods for the <see cref="HtmlAttributes"/> class.
    /// </summary>
    public static class HtmlAttributesExtensions
    {
        /// <summary>
        /// Explicitly convert a dictionary to a <see cref="HtmlAttributes"/> class.
        /// </summary>
        /// <param name="htmlAttributes">A dictionary of HTML attributes</param>
        /// <returns>A new <see cref="HtmlAttributes"/> with the attributes</returns>
        public static HtmlAttributes ToHtmlAttributes(this IDictionary<string, object> htmlAttributes)
        {
            return new HtmlAttributes(htmlAttributes);
        }

        /// <summary>
        /// Convert from an anonymous object to a <see cref="HtmlAttributes"/> class.
        /// </summary>
        /// <param name="htmlAttributes">An anonymous object of HTML attributes</param>
        /// <returns>A new <see cref="HtmlAttributes"/> with the attributes</returns>
        public static HtmlAttributes ToHtmlAttributes(this object htmlAttributes)
        {
            return new HtmlAttributes(htmlAttributes);
        }
    }
}
