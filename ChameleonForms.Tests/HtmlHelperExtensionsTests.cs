
using Autofac;
using AutofacContrib.NSubstitute;
using ChameleonForms.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NUnit.Framework;

namespace ChameleonForms.Tests
{
    public class HtmlHelperExtensionsShould
    {
        [Test]
        public void Create_html_helper_against_same_writer_with_different_type()
        {
            var newHtmlHelper = _h.For<AnotherViewModel>();
            Assert.That(newHtmlHelper.ViewContext.Writer, Is.SameAs(_h.ViewContext.Writer));
        }

        [Test]
        public void Create_html_helper_against_same_request_context_with_different_type()
        {
            var newHtmlHelper = _h.For<AnotherViewModel>();
            Assert.That(newHtmlHelper.ViewContext.RouteData, Is.SameAs(_h.ViewContext.RouteData));
            Assert.That(newHtmlHelper.ViewContext.HttpContext, Is.SameAs(_h.ViewContext.HttpContext));
        }
        
        [Test]
        public void Create_html_helper_with_empty_prefix_with_different_type()
        {
            var newHtmlHelper = _h.For<AnotherViewModel>();
            Assert.That(newHtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, Is.Empty);
        }

        [Test]
        public void Create_html_helper_with_prefix_with_different_type()
        {
            var newHtmlHelper = _h.For<AnotherViewModel>(htmlFieldPrefix: "Prefix");
            Assert.That(newHtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, Is.EqualTo("Prefix"));
        }

        [Test]
        public void Create_html_helper_with_prefix_with_different_type_but_extending_original_prefix()
        {
            ((HtmlHelper) _h).ViewData.TemplateInfo.HtmlFieldPrefix = "OriginalPrefix";
            var newHtmlHelper = _h.For<AnotherViewModel>(htmlFieldPrefix: "Prefix");
            Assert.That(newHtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, Is.EqualTo("OriginalPrefix.Prefix"));
        }

        [Test]
        public void Create_html_helper_with_copy_of_view_data_with_different_type()
        {
            const string key = "key";
            const string value = "value";
            ((HtmlHelper)_h).ViewData[key] = value; // HtmlHelper<TModel>.ViewData is different to HtmlHelper.ViewData (lol)
            var newHtmlHelper = _h.For<AnotherViewModel>();
            Assert.That(newHtmlHelper.ViewData, Is.Not.SameAs(((HtmlHelper)_h).ViewData), "View data is different dictionary");
            Assert.That(newHtmlHelper.ViewData[key], Is.EqualTo(value), "View data contents are the same");
        }

        [Test]
        public void Create_html_helper_with_new_model_of_different_type()
        {
            var newModel = new AnotherViewModel();
            var newHtmlHelper = _h.For(newModel);
            Assert.That(newHtmlHelper.ViewData.Model, Is.SameAs(newModel));
        }

        [Test]
        public void Create_html_helper_with_sub_model_bound_to_parent()
        {
            _h.ViewData.Model = new TestViewModel
            {
                Child = new TestChildViewModel()
            };
            var newHtmlHelper = _h.For(m => m.Child, bindFieldsToParent: true);
            Assert.That(newHtmlHelper.ViewData.Model, Is.SameAs(_h.ViewData.Model.Child), "Model is correct");
            Assert.That(newHtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, Is.EqualTo("Child"), "Prefix is correct");
        }

        [Test]
        public void Create_html_helper_with_correct_prefix_for_sub_model_bound_to_parent_with_prefix()
        {
            _h.ViewData.Model = new TestViewModel
            {
                Child = new TestChildViewModel()
            };
            _h.ViewData.TemplateInfo.HtmlFieldPrefix = "OriginalPrefix";
            var newHtmlHelper = _h.For(m => m.Child, bindFieldsToParent: true);
            Assert.That(newHtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, Is.EqualTo("OriginalPrefix.Child"));
        }

        [Test]
        public void Create_html_helper_for_sub_model_when_parent_null()
        {
            _h.ViewData.Model = null;
            var newHtmlHelper = _h.For(m => m.Child, bindFieldsToParent: true);
            Assert.That(newHtmlHelper.ViewData.Model, Is.Null);
        }

        [Test]
        public void Create_html_helper_with_new_model_for_sub_model_not_bound_to_parent()
        {
            _h.ViewData.Model = new TestViewModel
            {
                Child = new TestChildViewModel()
            };
            var newHtmlHelper = _h.For(m => m.Child, bindFieldsToParent: false);
            Assert.That(newHtmlHelper.ViewData.Model, Is.SameAs(_h.ViewData.Model.Child), "Model is correct");
            Assert.That(newHtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, Is.Empty, "Prefix is correct");
        }


        private AutoSubstitute _autoSubstitute;
        private HtmlHelper<TestViewModel> _h;

        [SetUp]
        public void Setup()
        {
            _autoSubstitute = AutoSubstituteContainer.Create();
            var viewDataDictionary = new ViewDataDictionary<TestViewModel>(_autoSubstitute.Resolve<IModelMetadataProvider>(), new ModelStateDictionary());

            _h = _autoSubstitute.Resolve<HtmlHelper<TestViewModel>>();
            _autoSubstitute.Provide<IHtmlHelper<TestViewModel>>(_h);
            var viewContext = _autoSubstitute.Resolve<ViewContext>(TypedParameter.From<ViewDataDictionary>(viewDataDictionary), TypedParameter.From(_autoSubstitute.Resolve<ActionContext>()));
            viewContext.ClientValidationEnabled = true;
            _h.Contextualize(viewContext);
        }

        public class TestViewModel
        {
            public string SomeProperty { get; set; }
            public TestChildViewModel Child { get; set; }
        }

        public class TestChildViewModel
        {
            public string SomeOtherProperty { get; set; }
        }

        public class AnotherViewModel
        {
            public string AnotherProperty { get; set; }
        }
    }
}
