using System.Collections.Generic;
using ApprovalTests.Html;
using ChameleonForms.Component.Config;
using NUnit.Framework;

namespace ChameleonForms.Tests.FieldGenerator.DefaultFieldGenerator
{
    class ListTests : DefaultFieldGeneratorShould
    {

        [Test]
        public void Use_correct_html_for_optional_int_list_id()
        {
            var list = new List<IntListItem> { new IntListItem { Id = 1, Name = "A" }, new IntListItem { Id = 2, Name = "B" } };
            var g = Arrange(m => m.OptionalIntListId, m => m.OptionalIntListId = null, m => m.IntList = list);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_nullable_int_list_id()
        {
            var list = new List<IntListItem> { new IntListItem { Id = 1, Name = "A" }, new IntListItem { Id = 2, Name = "B" } };
            var g = Arrange(m => m.RequiredNullableIntListId, m => m.RequiredNullableIntListId = null, m => m.IntList = list);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_string_list_id_as_list()
        {
            var list = new List<StringListItem> { new StringListItem { Value = "1", Label = "A" }, new StringListItem { Value = "2", Label = "B" } };
            var g = Arrange(m => m.OptionalStringListId, m => m.OptionalStringListId = "", m => m.StringList = list);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_null_required_string_list_id_as_list()
        {
            var list = new List<StringListItem> { new StringListItem { Value = "1", Label = "A" }, new StringListItem { Value = "2", Label = "B" } };
            var g = Arrange(m => m.RequiredStringListId, m => m.RequiredStringListId = null, m => m.StringList = list);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_int_list_id_with_none_string_override()
        {
            var list = new List<IntListItem> { new IntListItem { Id = 1, Name = "A" }, new IntListItem { Id = 2, Name = "B" } };
            var g = Arrange(m => m.OptionalIntListId, m => m.OptionalIntListId = null, m => m.IntList = list);

            var result = g.GetFieldHtml(new FieldConfiguration().WithNoneAs("-- Select Item"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_optional_string_list_id_as_list_with_none_string_override()
        {
            var list = new List<StringListItem> { new StringListItem { Value = "1", Label = "A" }, new StringListItem { Value = "2", Label = "B" } };
            var g = Arrange(m => m.OptionalStringListId, m => m.OptionalStringListId = "2", m => m.StringList = list);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList().WithNoneAs("No Value"));

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_int_list_id()
        {
            var list = new List<IntListItem> { new IntListItem { Id = 1, Name = "A" }, new IntListItem { Id = 2, Name = "B" } };
            var g = Arrange(m => m.RequiredIntListId, m => m.RequiredIntListId = 2, m => m.IntList = list);

            var result = g.GetFieldHtml(null);

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }

        [Test]
        public void Use_correct_html_for_required_string_list_id_as_list()
        {
            var list = new List<StringListItem> { new StringListItem { Value = "1", Label = "A" }, new StringListItem { Value = "2", Label = "B" } };
            var g = Arrange(m => m.RequiredStringListId, m => m.RequiredStringListId = "2", m => m.StringList = list);

            var result = g.GetFieldHtml(new FieldConfiguration().AsList());

            HtmlApprovals.VerifyHtml(result.ToHtmlString());
        }
    }
}
