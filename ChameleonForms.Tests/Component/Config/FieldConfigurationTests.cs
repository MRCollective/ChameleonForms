using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.Enums;
using Microsoft.AspNetCore.Html;
using NUnit.Framework;

namespace ChameleonForms.Tests.Component.Config
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class FieldConfigurationShould
    {
        private HtmlString _someHtmlString = new HtmlString("");
        private const string _someTextWithHtmlCharacters = "Some text with <html> & characters";
        private const string _someTextWithHtmlCharactersEscaped = "Some text with &lt;html&gt; &amp; characters";

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
        public void Set_id_attribute()
        {
            var fc = Field.Configure()
                .Id("DifferentId");

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
        public void Set_disabled_attribute()
        {
            var fc = Field.Configure()
                .Disabled();

            HtmlApprovals.VerifyHtml(fc.Attributes.ToHtmlString());
        }

        [Test]
        public void Not_set_disabled_attribute_when_false()
        {
            var fc = Field.Configure()
                .Disabled(false);

            Assert.That(fc.Attributes.ToHtmlString(), Is.Empty);
        }

        [Test]
        public void Set_readonly_attribute()
        {
            var fc = Field.Configure()
                .Readonly();

            HtmlApprovals.VerifyHtml(fc.Attributes.ToHtmlString());
        }

        [Test]
        public void Not_set_readonly_attribute_when_false()
        {
            var fc = Field.Configure()
                .Readonly(false);

            Assert.That(fc.Attributes.ToHtmlString(), Is.Empty);
        }

        [Test]
        public void Set_placeholder_attribute()
        {
            var fc = Field.Configure()
                .Placeholder("Some placeholder text");

            HtmlApprovals.VerifyHtml(fc.Attributes.ToHtmlString());
        }

        [Test]
        public void Set_autofocus_attribute()
        {
            var fc = Field.Configure().AutoFocus();

            HtmlApprovals.VerifyHtml(fc.Attributes.ToHtmlString());
        }

        [Test]
        public void Set_tabindex_attribute()
        {
            var fc = Field.Configure().TabIndex(44);

            HtmlApprovals.VerifyHtml(fc.Attributes.ToHtmlString());
        }

        [Test]
        public void Set_inline_label()
        {
            var fc = Field.Configure()
                .InlineLabel(_someTextWithHtmlCharacters);

            Assert.That(fc.InlineLabelText.ToHtmlString(), Is.EqualTo(_someTextWithHtmlCharactersEscaped));
        }

        [Test]
        public void Set_inline_label_html()
        {
            var fc = Field.Configure()
                .InlineLabel(_someHtmlString);

            Assert.That(fc.InlineLabelText, Is.EqualTo(_someHtmlString));
        }

        [Test]
        public void Set_label()
        {
            var fc = Field.Configure()
                .Label(_someTextWithHtmlCharacters);

            Assert.That(fc.LabelText.ToHtmlString(), Is.EqualTo(_someTextWithHtmlCharactersEscaped));
        }

        [Test]
        public void Set_label_html()
        {
            var fc = Field.Configure()
                .Label(_someHtmlString);

            Assert.That(fc.LabelText, Is.EqualTo(_someHtmlString));
        }

        [Test]
        public void Use_default_display_by_default()
        {
            var fc = Field.Configure();
            Assert.That(fc.DisplayType, Is.EqualTo(FieldDisplayType.Default));
        }

        [Test]
        public void Set_list_display_using_radiolist()
        {
            var fc = Field.Configure()
                .AsRadioList();

            Assert.That(fc.DisplayType, Is.EqualTo(FieldDisplayType.List));
        }

        [Test]
        public void Set_list_display_using_checkboxlist()
        {
            var fc = Field.Configure()
                .AsCheckboxList();

            Assert.That(fc.DisplayType, Is.EqualTo(FieldDisplayType.List));
        }

        [Test]
        public void Ensure_checkboxlist_always_performs_same_action_as_radiolist()
        {
            var fcCheckbox = Field.Configure().AsCheckboxList();
            var fcRadio = Field.Configure().AsRadioList();

            Assert.That(fcCheckbox.DisplayType, Is.EqualTo(fcRadio.DisplayType));
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
            var fc = Field.Configure().WithHint(_someTextWithHtmlCharacters);

            Assert.That(fc.Hint.ToHtmlString(), Is.EqualTo(_someTextWithHtmlCharactersEscaped));
        }

        [Test]
        public void Set_html_hint()
        {
            var fc = Field.Configure().WithHint(_someHtmlString);

            Assert.That(fc.Hint, Is.EqualTo(_someHtmlString));
        }

        [Test]
        public void Allow_for_custom_extension()
        {
            var fc = Field.Configure();

            fc.Bag.SomeConfigItem = "Hello!";

            Assert.That(fc.Bag.SomeConfigItem, Is.EqualTo("Hello!"));
        }

        [Test]
        public void Get_nullable_primitive_bag_data_if_present()
        {
            int? value = 5;
            var fc = Field.Configure();
            fc.Bag.Property = value;

            Assert.That(fc.GetBagData<int?>("Property"), Is.EqualTo(value));
        }

        [Test]
        public void Get_non_nullable_primitive_bag_data_if_present()
        {
            const int value = 5;
            var fc = Field.Configure();
            fc.Bag.Property = value;

            Assert.That(fc.GetBagData<int>("Property"), Is.EqualTo(value));
        }

        [Test]
        public void Get_nullable_object_bag_data_if_present()
        {
            var fc = Field.Configure();
            fc.Bag.Property = fc;

            Assert.That(fc.GetBagData<IFieldConfiguration>("Property"), Is.SameAs(fc));
        }

        [Test]
        public void Return_null_if_getting_non_existant_nullable_primitive_property()
        {
            var fc = Field.Configure();

            Assert.That(fc.GetBagData<int?>("Property"), Is.Null);
        }

        [Test]
        public void Return_null_if_getting_non_existant_object_property()
        {
            var fc = Field.Configure();

            Assert.That(fc.GetBagData<FieldConfiguration>("Property"), Is.Null);
        }

        [Test]
        public void Return_default_value_if_getting_non_existant_non_nullable_property()
        {
            var fc = Field.Configure();

            Assert.That(fc.GetBagData<int>("Property"), Is.EqualTo(0));
        }

        [Test]
        public void Return_null_if_getting_different_typed_nullable_primitive_property()
        {
            var fc = Field.Configure();
            fc.Bag.Property = "different_type";

            Assert.That(fc.GetBagData<int?>("Property"), Is.Null);
        }

        [Test]
        public void Return_null_if_getting_different_typed_object_property()
        {
            var fc = Field.Configure();
            fc.Bag.Property = "different_type";

            Assert.That(fc.GetBagData<FieldConfiguration>("Property"), Is.Null);
        }

        [Test]
        public void Return_default_value_if_getting_different_typed_non_nullable_property()
        {
            var fc = Field.Configure();
            fc.Bag.Property = "different_type";

            Assert.That(fc.GetBagData<int>("Property"), Is.EqualTo(0));
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

            Assert.That(fc.PrependedHtml, Is.EqualTo(new[]{x}));
        }

        [Test]
        public void Return_html_for_appended_html_if_one_set()
        {
            var x = new HtmlString("");
            var fc = Field.Configure().Append(x);

            Assert.That(fc.AppendedHtml, Is.EqualTo(new[] { x }));
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

            Assert.That(fc.PrependedHtml, Is.EqualTo(new[] { z, y, x }));
        }

        [Test]
        public void Return_ltr_html_for_appended_html_if_multiple_set()
        {
            var x = new HtmlString("x");
            var y = new HtmlString("y");
            var z = new HtmlString("z");
            var fc = Field.Configure().Append(x).Append(y).Append(z);

            Assert.That(fc.AppendedHtml, Is.EqualTo(new[] { x, y, z }));
        }

        [Test]
        public void Return_html_for_field_html_if_one_set()
        {
            var x = new HtmlString("");
            var fc = Field.Configure().OverrideFieldHtml(x);

            Assert.That(fc.FieldHtml, Is.EqualTo(x));
        }

        [Test]
        public void Return_null_if_no_format_string_set()
        {
            var fc = Field.Configure();

            Assert.That(fc.FormatString, Is.Null);
        }

        [Test]
        public void Return_format_string_if_set()
        {
            var fc = Field.Configure().WithFormatString("{0}");

            Assert.That(fc.FormatString, Is.EqualTo("{0}"));
        }

        [Test]
        public void Have_label_by_default()
        {
            var fc = Field.Configure();

            Assert.That(fc.HasLabelElement, Is.True);
        }

        [Test]
        public void Set_no_label()
        {
            var fc = Field.Configure().WithoutLabelElement();

            Assert.That(fc.HasLabelElement, Is.False);
        }
    }
}
