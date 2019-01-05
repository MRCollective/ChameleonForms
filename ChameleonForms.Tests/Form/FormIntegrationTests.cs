
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.Tests.FieldGenerator;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;
using ChameleonForms.Component;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Autofac;
using Microsoft.AspNetCore.Mvc;

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
            var _autoSubstitute = AutoSubstituteContainer.Create();
            var viewDataDictionary = new ViewDataDictionary<TestFieldViewModel>(_autoSubstitute.Resolve<IModelMetadataProvider>(), new ModelStateDictionary());

            _h = _autoSubstitute.Resolve<HtmlHelper<TestFieldViewModel>>();
            _autoSubstitute.Provide<IHtmlHelper<TestFieldViewModel>>(_h);
            var viewContext = _autoSubstitute.Resolve<ViewContext>(TypedParameter.From<ViewDataDictionary>(viewDataDictionary), TypedParameter.From(_autoSubstitute.Resolve<ActionContext>()));
            viewContext.ClientValidationEnabled = true;
            _h.Contextualize(viewContext);
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
    }
}