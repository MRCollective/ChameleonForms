using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Templates;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class BuildSubmitButtonShould
    {
        [Test]
        public void Generate_submit_button_with_default_options()
        {
            var h = HtmlCreator.BuildSubmitButton("value");

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }

        [Test]
        public void Generate_submit_button_with_non_default_options()
        {
            var h = HtmlCreator.BuildSubmitButton("thevalue", "reset", "myId", new {onclick = "return false;", @class = "a&^&*FGdf"});

            HtmlApprovals.VerifyHtml(h.ToHtmlString());
        }
    }

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class OutputAttributesShould
    {
        [Test]
        public void Output_nothing_if_null()
        {
            var h = HtmlCreator.BuildAttributes(null);

            Assert.That(h.ToString(), Is.Empty);
        }

        [Test]
        public void Output_nothing_if_empty_object()
        {
            var h = HtmlCreator.BuildAttributes(new {});

            Assert.That(h.ToString(), Is.Empty);
        }

        [Test]
        public void Output_attributes_from_single_object()
        {
            var h = HtmlCreator.BuildAttributes(new { src = "http://someurl/", @class = "asdf asdf", data_attribute = "some&^\"thing" });

            HtmlApprovals.VerifyHtml(h.ToString());
        }

        [Test]
        public void Merge_attributes_from_multiple_objects()
        {
            var h = HtmlCreator.BuildAttributes(new { id = "oldid", @class = "class", data_random = "random" }, new { id = "newid", data_new_attr = "newattr", @class = "class2 class3" });

            HtmlApprovals.VerifyHtml(h.ToString());
        }
    }
}
