using System.Collections.Generic;
using ChameleonForms.Templates;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates
{
    [TestFixture]
    class HtmlAttributesShould
    {
        private HtmlAttributes ReturnAsHtmlAttributes(HtmlAttributes attrs)
        {
            return attrs;
        }

        private const string ExpectedHtml = " cellpadding=\"0\" class=\"class1 class2\" data-somedata=\"&quot;rubbi&amp;h&quot;\" src=\"http://url/\" style=\"width: 100%;\"";
        private static readonly Dictionary<string, object> Dictionary = new Dictionary<string, object> { { "style", "width: 100%;" }, { "cellpadding", 0 }, { "class", "class1 class2" }, { "src", "http://url/" }, { "data-somedata", "\"rubbi&h\"" } };
        private static readonly object AnonymousObject = new { style = "width: 100%;", cellpadding = 0, @class = "class1 class2", src = "http://url/", data_somedata = "\"rubbi&h\"" };
        
        [Test]
        public void Construct_via_lambdas()
        {
            var h = new HtmlAttributes(style => "width: 100%;", cellpadding => 0, @class => "class1 class2", src => "http://url/", data_somedata => "\"rubbi&h\"");

            Assert.That(h.ToHtmlString(), Is.EqualTo(ExpectedHtml));
        }

        [Test]
        public void Construct_via_dictionary()
        {
            var h = new HtmlAttributes(Dictionary);

            Assert.That(h.ToHtmlString(), Is.EqualTo(ExpectedHtml));
        }

        [Test]
        public void Construct_via_dictionary_extension_method()
        {
            var h = Dictionary.ToHtmlAttributes();

            Assert.That(h.ToHtmlString(), Is.EqualTo(ExpectedHtml));
        }

        [Test]
        public void Construct_via_dictionary_implicit_conversion()
        {
            var h = ReturnAsHtmlAttributes(Dictionary);

            Assert.That(h.ToHtmlString(), Is.EqualTo(ExpectedHtml));
        }

        [Test]
        public void Construct_via_anonymous_object()
        {
            var h = new HtmlAttributes(AnonymousObject);

            Assert.That(h.ToHtmlString(), Is.EqualTo(ExpectedHtml));
        }

        [Test]
        public void Construct_via_anonymous_object_extension_method()
        {
            var h = AnonymousObject.ToHtmlAttributes();

            Assert.That(h.ToHtmlString(), Is.EqualTo(ExpectedHtml));
        }

        [Test]
        public void Add_css_classes()
        {
            var h = new HtmlAttributes(@class => "class1");

            h.AddClass("class2 class3");

            Assert.That(h.ToHtmlString(), Is.EqualTo(" class=\"class2 class3 class1\""));
        }

        [Test]
        public void Add_id()
        {
            var h = new HtmlAttributes(@class => "class1");

            h.Id("anId");

            Assert.That(h.ToHtmlString(), Is.EqualTo(" class=\"class1\" id=\"anId\""));
        }

        [Test]
        public void Add_sanitised_id()
        {
            var h = new HtmlAttributes();

            h.Id("Model.Id");

            Assert.That(h.ToHtmlString(), Is.EqualTo(" id=\"Model_Id\""));
        }

        [Test]
        public void Make_readonly()
        {
            var h = new HtmlAttributes(@class => "class1");

            h.Readonly();

            Assert.That(h.ToHtmlString(), Is.EqualTo(" class=\"class1\" readonly=\"readonly\""));
        }

        [Test]
        public void Not_make_readonly_when_guard_is_false()
        {
            var h = new HtmlAttributes(@class => "class1");

            h.Readonly(false);

            Assert.That(h.ToHtmlString(), Is.EqualTo(" class=\"class1\""));
        }

        [Test]
        public void Make_disabled()
        {
            var h = new HtmlAttributes(@class => "class1");

            h.Disabled();

            Assert.That(h.ToHtmlString(), Is.EqualTo(" class=\"class1\" disabled=\"disabled\""));
        }

        [Test]
        public void Not_make_disabled_when_guard_is_false()
        {
            var h = new HtmlAttributes(@class => "class1");

            h.Disabled(false);

            Assert.That(h.ToHtmlString(), Is.EqualTo(" class=\"class1\""));
        }

        [Test]
        public void Add_new_attribute()
        {
            var h = new HtmlAttributes(href => "http://url/");

            h.Attr("data-value", "val");

            Assert.That(h.ToHtmlString(), Is.EqualTo(" data-value=\"val\" href=\"http://url/\""));
        }

        [Test]
        public void Add_new_attribute_using_a_lambda()
        {
            var h = new HtmlAttributes(href => "http://url/");

            h.Attr(data_value => "val");

            Assert.That(h.ToHtmlString(), Is.EqualTo(" data-value=\"val\" href=\"http://url/\""));
        }

        [Test]
        public void Replace_existing_attribute()
        {
            var h = new HtmlAttributes(href => "http://url/");

            h.Attr("href", "newhref");

            Assert.That(h.ToHtmlString(), Is.EqualTo(" href=\"newhref\""));
        }

        [Test]
        public void Replace_existing_attribute_using_a_lambda()
        {
            var h = new HtmlAttributes(href => "http://url/");

            h.Attr(href => "newhref");

            Assert.That(h.ToHtmlString(), Is.EqualTo(" href=\"newhref\""));
        }

        [Test]
        public void Replace_and_add_attributes_using_lambdas()
        {
            var h = new HtmlAttributes(data_existing => "old");

            h.Attrs(data_existing => "new", data_new => "newnew");

            Assert.That(h.ToHtmlString(), Is.EqualTo(" data-existing=\"new\" data-new=\"newnew\""));
        }

        [Test]
        public void Replace_and_add_attributes_using_dictionary()
        {
            var h = new HtmlAttributes(data_existing => "old");

            h.Attrs(new Dictionary<string, object> {{"data-existing", "new"}, {"data-new", "newnew"}});

            Assert.That(h.ToHtmlString(), Is.EqualTo(" data-existing=\"new\" data-new=\"newnew\""));
        }

        [Test]
        public void Replace_and_add_attributes_using_anonymous_object()
        {
            var h = new HtmlAttributes(data_existing => "old");

            h.Attrs(new {data_existing = "new", data_new = "newnew"});

            Assert.That(h.ToHtmlString(), Is.EqualTo(" data-existing=\"new\" data-new=\"newnew\""));
        }

        [Test]
        public void Replace_attributes_with_empty_string_when_null_value_added_using_key_value([Values(1, 2)] int setMethod)
        {
            var h = new HtmlAttributes(name => "Old");

            switch (setMethod)
            {
                case 1:
                    h.Attr(name => null);
                    break;
                case 2:
                    h.Attr("name", null);
                    break;
            }

            Assert.That(h.ToHtmlString(), Is.EqualTo(" name=\"\""));
        }

        [Test]
        public void Ensure_merged_attributes_are_case_sensitive([Values(1,2,3)] int setMethod)
        {
            var h = new HtmlAttributes(name => "Old");

            switch (setMethod)
            {
                case 1:
                    h.Attr(Name => "honey-badger");
                    break;
                case 2:
                    h.Attr("Name", "honey-badger");
                    break;
                case 3:
                    h.Attrs(new {Name = "honey-badger"});
                    break;
            }

            Assert.That(h.ToHtmlString(), Is.EqualTo(" name=\"honey-badger\""));
        }

        [Test]
        public void Ensure_new_attributes_are_case_sensitive([Values(1, 2, 3)] int constructorOption)
        {
            HtmlAttributes h = null;

            switch (constructorOption)
            {
                case 1:
                    h = new HtmlAttributes(Name => "honey-badger");
                    break;
                case 2:
                    h = new HtmlAttributes(new Dictionary<string, object>{{"Name", "honey-badger"}});
                    break;
                case 3:
                    h = new HtmlAttributes(new { Name = "honey-badger" });
                    break;
            }

            Assert.That(h.ToHtmlString(), Is.EqualTo(" name=\"honey-badger\""));
        }
    }
}