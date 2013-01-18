using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component;
using ChameleonForms.Enums;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component.Config
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class FieldConfigurationShould
    {
        [Test]
        public void Proxy_html_attributes()
        {
            var fc = Field.Configure()
                .Attr("data-attr1", "value")
                .Attr(data_attr2 => "value")
                .Attrs(new Dictionary<string, object> {{"data-attr3", "value"}})
                .Attrs(new {data_attr4 = "value"})
                .Attrs(data_attr5 => "value")
                .AddClass("someclass");

            HtmlApprovals.VerifyHtml(fc.Attributes.ToHtmlString());
        }

        [Test]
        public void Set_textarea_attributes()
        {
            var fc = Field.Configure()
                .Rows(5)
                .Cols(60);

            HtmlApprovals.VerifyHtml(fc.Attributes.ToHtmlString());
        }

        [Test]
        public void Set_inline_label()
        {
            var fc = Field.Configure()
                .InlineLabel("Some label text");

            Assert.That(fc.InlineLabelText, Is.EqualTo("Some label text"));
        }

        [Test]
        public void Set_label()
        {
            var fc = Field.Configure()
                .Label("Some label text");

            Assert.That(fc.LabelText, Is.EqualTo("Some label text"));
        }

        [Test]
        public void Use_default_display_by_default()
        {
            var fc = Field.Configure();
            Assert.That(fc.DisplayType, Is.EqualTo(FieldDisplayType.Default));
        }

        [Test]
        public void Set_list_display()
        {
            var fc = Field.Configure()
                .AsList();

            Assert.That(fc.DisplayType, Is.EqualTo(FieldDisplayType.List));
        }

        [Test]
        public void Set_select_display()
        {
            var fc = Field.Configure()
                .AsDropDown();

            Assert.That(fc.DisplayType, Is.EqualTo(FieldDisplayType.DropDown));
        }

        [Test]
        public void Use_yes_as_true_string_by_default()
        {
            var fc = Field.Configure();
            
            Assert.That(fc.TrueString, Is.EqualTo("Yes"));
        }

        [Test]
        public void Allow_override_for_true_string()
        {
            var fc = Field.Configure().WithTrueAs("Hello");

            Assert.That(fc.TrueString, Is.EqualTo("Hello"));
        }

        [Test]
        public void Use_no_as_false_string_By_default()
        {
            var fc = Field.Configure();

            Assert.That(fc.FalseString, Is.EqualTo("No"));
        }

        [Test]
        public void Allow_override_for_false_string()
        {
            var fc = Field.Configure().WithTrueAs("World!");

            Assert.That(fc.TrueString, Is.EqualTo("World!"));
        }

        [Test]
        public void Use_empty_string_as_none_string_By_default()
        {
            var fc = Field.Configure();

            Assert.That(fc.NoneString, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Allow_override_for_none_string()
        {
            var fc = Field.Configure().WithNoneAs("None!");

            Assert.That(fc.NoneString, Is.EqualTo("None!"));
        }

        [Test]
        public void Set_and_encode_non_html_hint()
        {
            var fc = Field.Configure().WithHint("Some hint with <html> & characters");

            Assert.That(fc.Hint.ToHtmlString(), Is.EqualTo("Some hint with &lt;html&gt; &amp; characters"));
        }

        [Test]
        public void Set_html_hint()
        {
            var htmlHint = new HtmlString("");
            var fc = Field.Configure().WithHint(htmlHint);

            Assert.That(fc.Hint, Is.EqualTo(htmlHint));
        }

        [Test]
        public void Allow_for_custom_extension()
        {
            var fc = Field.Configure();

            fc.Bag.SomeConfigItem = "Hello!";

            Assert.That(fc.Bag.SomeConfigItem, Is.EqualTo("Hello!"));
        }

        [Test]
        public void Return_empty_enumeration_for_prepended_html_if_none_set()
        {
            var fc = Field.Configure();

            Assert.That(fc.PrependedHtml, Is.Empty);
        }

        [Test]
        public void Return_empty_enumeration_for_appended_html_if_none_set()
        {
            var fc = Field.Configure();

            Assert.That(fc.AppendedHtml, Is.Empty);
        }

        [Test]
        public void Return_html_for_prepended_html_if_one_set()
        {
            var x = new HtmlString("");
            var fc = Field.Configure().Prepend(x);

            Assert.That(fc.PrependedHtml, Is.EquivalentTo(new[]{x}));
        }

        [Test]
        public void Return_html_for_appended_html_if_one_set()
        {
            var x = new HtmlString("");
            var fc = Field.Configure().Append(x);

            Assert.That(fc.AppendedHtml, Is.EquivalentTo(new[] { x }));
        }

        [Test]
        public void Return_encoded_html_for_prepended_string_if_one_set()
        {
            const string x = "asdf<&>asdf\"";
            var fc = Field.Configure().Prepend(x);

            Assert.That(fc.PrependedHtml.Single().ToHtmlString(), Is.EqualTo("asdf&lt;&amp;&gt;asdf&quot;"));
        }

        [Test]
        public void Return_encoded_html_for_appended_string_if_one_set()
        {
            const string x = "asdf<&>asdf\"";
            var fc = Field.Configure().Append(x);

            Assert.That(fc.AppendedHtml.Single().ToHtmlString(), Is.EqualTo("asdf&lt;&amp;&gt;asdf&quot;"));
        }

        [Test]
        public void Return_ltr_html_for_prepended_html_if_multiple_set()
        {
            var x = new HtmlString("x");
            var y = new HtmlString("y");
            var z = new HtmlString("z");
            var fc = Field.Configure().Prepend(x).Prepend(y).Prepend(z);

            Assert.That(fc.PrependedHtml, Is.EquivalentTo(new[] { z, y, x }));
        }

        [Test]
        public void Return_ltr_html_for_appended_html_if_multiple_set()
        {
            var x = new HtmlString("x");
            var y = new HtmlString("y");
            var z = new HtmlString("z");
            var fc = Field.Configure().Append(x).Append(y).Append(z);

            Assert.That(fc.AppendedHtml, Is.EquivalentTo(new[] { x, y, z }));
        }
    }
}
