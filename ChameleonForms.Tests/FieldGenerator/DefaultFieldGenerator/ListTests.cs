using System.Collections.Generic;
using ApprovalTests.Html;
using ChameleonForms.Component.Config;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class ListTests : DefaultFieldGeneratorShould
    {
        private readonly List<IntListItem> IntList = new List<IntListItem>
        {
            new IntListItem {Id = 1, Name = "A"},
            new IntListItem {Id = 2, Name = "B"}
        };

        private readonly List<StringListItem> StringList = new List<StringListItem>
        {
            new StringListItem { Value = "1", Label = "A" },
            new StringListItem { Value = "2", Label = "B" }
        };

        private FieldGenerators.DefaultFieldGenerator<TestFieldViewModel, int?> ArrangeOptionalIntList(int? value)
        {
            return Arrange(m => m.OptionalIntListId, m => m.OptionalIntListId = value, m => m.IntList = IntList);
        }

        private FieldGenerators.DefaultFieldGenerator<TestFieldViewModel, int?> ArrangeRequiredNullableIntList(int? value)
        {
            return Arrange(m => m.RequiredNullableIntListId, m => m.RequiredNullableIntListId = value, m => m.IntList = IntList);
        }

        private FieldGenerators.DefaultFieldGenerator<TestFieldViewModel, int> ArrangeRequiredIntList(int value)
        {
            return Arrange(m => m.RequiredIntListId, m => m.RequiredIntListId = value, m => m.IntList = IntList);
        }

        private FieldGenerators.DefaultFieldGenerator<TestFieldViewModel, string> ArrangeOptionalStringList(string value)
        {
            return Arrange(m => m.OptionalStringListId, m => m.OptionalStringListId = value, m => m.StringList = StringList);
        }

        private FieldGenerators.DefaultFieldGenerator<TestFieldViewModel, string> ArrangeRequiredStringList(string value)
        {
            return Arrange(m => m.RequiredStringListId, m => m.RequiredStringListId = value, m => m.StringList = StringList);
        }

        [Test]
        public void Use_correct_html_for_optional_int_list_id()
        {
            var g = ArrangeOptionalIntList(null);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_nullable_int_list_id()
        {
            var g = ArrangeRequiredNullableIntList(null);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_string_list_id_as_list()
        {
            var g = ArrangeOptionalStringList("");

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_null_required_string_list_id_as_list()
        {
            var g = ArrangeRequiredStringList(null);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_int_list_id_with_none_string_override()
        {
            var g = ArrangeOptionalIntList(null);

            var result = g.GetFieldHtml(new FieldConfiguration().WithNoneAs("-- Select Item"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_string_list_id_as_list_with_none_string_override()
        {
            var g = ArrangeOptionalStringList("2");

            var result = g.GetFieldHtml(new FieldConfiguration().AsList().WithNoneAs("No Value"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_int_list_id()
        {
            var g = ArrangeRequiredIntList(2);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_string_list_id_as_list()
        {
            var g = ArrangeRequiredStringList("2");

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
