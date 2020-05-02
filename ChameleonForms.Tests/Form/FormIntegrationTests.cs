using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Tests.FieldGenerator;
using NUnit.Framework;
using ChameleonForms.Component;
using ChameleonForms.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Html;

namespace ChameleonForms.Tests.Form
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    class IntegratedFormShould
    {
        private HtmlHelper<TestFieldViewModel> _h;

        [SetUp]
        public void Setup()
        {
            var context = new MvcTestContext();
            var viewContext = context.GetViewTestContext<TestFieldViewModel>();
            _h = viewContext.HtmlHelper;
        }

        [Test]
        public void Output_field_html_with_field_configuration_values()
        {
            var form = _h.BeginChameleonForm();

            var html = form.FieldElementFor(m => m.Decimal).AddClass("a-class").ToHtmlString();

            HtmlApprovals.VerifyHtml(html);
        }

        [Test]
        public void Output_label_html_with_field_configuration_values()
        {
            var form = _h.BeginChameleonForm();

            var html = form.LabelFor(m => m.Decimal).Label("LABEL").ToHtmlString();

            HtmlApprovals.VerifyHtml(html);
        }

        [Test]
        public void Output_required_field_using_default_template()
        {
            string html;
            using (var f = _h.BeginChameleonForm())
            {
                using (var s = f.BeginSection("Section"))
                {
                    html = s.FieldFor(m => m.Decimal).AddClass("a-class").Label("LABEL").ToHtmlString();
                }
            }

            HtmlApprovals.VerifyHtml(html);
        }

        [Test]
        public void Output_optional_field_using_default_template()
        {
            string html;
            using (var f = _h.BeginChameleonForm())
            {
                using (var s = f.BeginSection("Section"))
                {
                    html = s.FieldFor(m => m.NullableDateTime).ToHtmlString();
                }
            }

            HtmlApprovals.VerifyHtml(html);
        }

        [Test]
        public void Output_field_with_hint_using_default_template()
        {
            string html;
            using (var f = _h.BeginChameleonForm())
            {
                using (var s = f.BeginSection("Section"))
                {
                    html = s.FieldFor(m => m.NullableDateTime).WithHint("A hint").ToHtmlString();
                }
            }

            HtmlApprovals.VerifyHtml(html);
        }
    }
}