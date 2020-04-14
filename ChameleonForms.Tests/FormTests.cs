using ChameleonForms.Enums;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Templates;
using ChameleonForms.Templates.Default;
using ChameleonForms.Templates.TwitterBootstrap3;
using ChameleonForms.Tests.Helpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using NUnit.Framework;
using DefaultFormTemplate = ChameleonForms.Templates.Default.DefaultFormTemplate;

namespace ChameleonForms.Tests
{
    [TestFixture]
    class FormShould
    {
        [Test]
        public void Store_html_helper()
        {
            var f = CreateForm();

            Assert.That(f.HtmlHelper, Is.EqualTo(_h));
        }

        [Test]
        public void Store_template()
        {
            var f = CreateForm();

            Assert.That(f.Template, Is.EqualTo(_t));
        }

        [Test]
        public void Write_start_of_form_on_construction()
        {
            CreateForm();

            _h.ViewContext.Writer.Received().Write(_beginHtml);
            _h.ViewContext.Writer.DidNotReceive().Write(_endHtml);
        }

        [Test]
        public void Write_end_of_form_on_disposal()
        {
            var f = CreateForm();

            f.Dispose();

            _h.ViewContext.Writer.Received().Write(_endHtml);
        }

        [Test]
        public void Construct_form_via_extension_method()
        {
            var t = new DefaultFormTemplate();

            var f2 = _h.BeginChameleonForm(Action, Method, new HtmlAttributes(), Enctype);

            Assert.That(f2, Is.Not.Null);
            _h.ViewContext.Writer.Received().Write(Arg.Is<IHtmlContent>(h => h.ToHtmlString() == t.BeginForm(Action, Method, _htmlAttributes, Enctype).ToHtmlString()));
        }

        [Test]
        public void Construct_form_via_extension_method_to_sub_property()
        {
            var f2 = _h.BeginChameleonFormFor(m => m.Child, Action, Method, new HtmlAttributes(), Enctype);

            Assert.That(f2.HtmlHelper, Is.AssignableTo<HtmlHelper<TestChildViewModel>>());
            Assert.That(f2.HtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, Is.Empty);
        }

        [Test]
        public void Construct_form_via_extension_method_to_sub_property_and_use_correct_model()
        {
            _h.ViewData.Model = new TestFieldViewModel {Child = new TestChildViewModel()};
            var f2 = _h.BeginChameleonFormFor(m => m.Child, Action, Method, new HtmlAttributes(), Enctype);

            Assert.That(f2.HtmlHelper.ViewData.Model, Is.SameAs(_h.ViewData.Model.Child));
        }

        [Test]
        public void Construct_form_via_extension_method_to_new_model_and_use_correct_model()
        {
            var newModel = new RandomViewModel();
            var f2 = _h.BeginChameleonFormFor(newModel, Action, Method, new HtmlAttributes(), Enctype);

            Assert.That(f2.HtmlHelper.ViewData.Model, Is.SameAs(newModel));
            Assert.That(f2.HtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, Is.Empty);
        }

        [Test]
        public void Construct_form_via_extension_method_to_new_model_type_with_null_model()
        {
            var f2 = _h.BeginChameleonFormFor(default(RandomViewModel), Action, Method, new HtmlAttributes(), Enctype);

            Assert.That(f2.HtmlHelper, Is.AssignableTo<HtmlHelper<RandomViewModel>>());
            Assert.That(f2.HtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, Is.Empty);
        }

        [Test]
        public void Construct_form_via_extension_method_using_default_template()
        {
            var f = _h.BeginChameleonForm(Action, Method, new HtmlAttributes(), Enctype);

            Assert.That(f.Template, Is.TypeOf<DefaultFormTemplate>());
        }

        [Test]
        public void Construct_form_via_extension_method_using_default_template_defined_by_user()
        {
            FormTemplate.Default = new TwitterBootstrapFormTemplate();

            var f = _h.BeginChameleonForm(Action, Method, new HtmlAttributes(), Enctype);

            Assert.That(f.Template, Is.TypeOf<TwitterBootstrapFormTemplate>());
        }

        [Test]
        public void Give_a_field_generator()
        {
            var f = CreateForm();

            var g = f.GetFieldGenerator(m => m.SomeProperty);

            Assert.That(g, Is.TypeOf<DefaultFieldGenerator<TestFieldViewModel, string>>());
        }

        private HtmlHelper<TestFieldViewModel> _h;
        private IFormTemplate _t;

        private readonly IHtmlContent _beginHtml = new HtmlString("");
        private readonly IHtmlContent _endHtml = new HtmlString("");

        private const string Action = "/";
        private const FormMethod Method = FormMethod.Post;
        private const EncType Enctype = EncType.Multipart;
        private readonly HtmlAttributes _htmlAttributes = new HtmlAttributes();

        [SetUp]
        public void Setup()
        {
            var context = new MvcTestContext();
            var viewContext = context.GetViewTestContext<TestFieldViewModel>();

            _h = viewContext.HtmlHelper;

            _t = Substitute.For<IFormTemplate>();
            _t.BeginForm(Action, Method, _htmlAttributes, Enctype).Returns(_beginHtml);
            _t.EndForm().Returns(_endHtml);
        }

        private Form<TestFieldViewModel> CreateForm()
        {
            return new Form<TestFieldViewModel>(_h, _t, Action, Method, _htmlAttributes, Enctype);
        }

        [TearDown]
        public void Teardown()
        {
            FormTemplate.Default = new DefaultFormTemplate();
        }

        public class TestFieldViewModel
        {
            public string SomeProperty { get; set; }
            public TestChildViewModel Child { get; set; }
        }

        public class TestChildViewModel
        {
            public string AnotherProperty { get; set; }
        }

        public class RandomViewModel
        {
            public string SomeOtherProperty { get; set; }
        }
    }
}
