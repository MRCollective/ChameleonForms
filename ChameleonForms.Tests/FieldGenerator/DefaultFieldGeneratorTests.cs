using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;

using ApprovalTests.Reporters;
using Autofac;
using ChameleonForms.Attributes;
using ChameleonForms.Component;
using ChameleonForms.Component.Config;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Templates.Default;
using ChameleonForms.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using NUnit.Framework;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace ChameleonForms.Tests.FieldGenerator
{
    public enum TestEnum
    {
        Simplevalue,
        [Description("Description attr text")]
        ValueWithDescriptionAttribute,
        ValueWithMultpipleWordsAndNoDescriptionAttribute
    }

    [Flags]
    public enum TestFlagsEnum
    {
        Simplevalue = 1,
        [Description("Description attr text")]
        ValueWithDescriptionAttribute = 2,
        ValueWithMultpipleWordsAndNoDescriptionAttribute = 4
    }

    public class TestFieldViewModel
    {
        public TestFieldViewModel()
        {
            Child = new ChildViewModel();
        }

        [Required]
        public string RequiredString { get; set; }

        [Display(Name = "Use this display name")]
        public string StringWithDisplayAttribute { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public Decimal DecimalWithFormatStringAttribute { get; set; }

        public Decimal Decimal { get; set; }

        public DateTime DateTime { get; set; }
        public DateTime? NullableDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeWithFormat { get; set; }

        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NullableDateTimeWithFormat { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeWithGFormat { get; set; }

        public TestEnum RequiredEnum { get; set; }
        [RequiredFlagsEnum]
        public TestFlagsEnum RequiredFlagsEnum { get; set; }

        [Required]
        public TestEnum? RequiredNullableEnum { get; set; }
        [RequiredFlagsEnum]
        public TestFlagsEnum? RequiredNullableFlagsEnum { get; set; }

        [DisplayFormat(NullDisplayText = "Nothing to see here")]
        public TestEnum? OptionalEnumWithNullStringAttribute { get; set; }
        [DisplayFormat(NullDisplayText = "Nothing to see here")]
        public TestFlagsEnum? OptionalFlagsEnumWithNullStringAttribute { get; set; }

        public TestEnum? OptionalEnum { get; set; }
        public TestFlagsEnum? OptionalFlagsEnum { get; set; }

        [Required]
        public IEnumerable<TestEnum> RequiredEnumList { get; set; }

        [Required]
        public IList<TestEnum?> RequiredNullableEnumList { get; set; }
        
        public List<TestEnum> OptionalEnumList { get; set; }

        public ICollection<TestEnum?> OptionalNullableEnumList { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.MultilineText)]
        public string Textarea { get; set; }

        public bool RequiredBoolean { get; set; }

        [Required]
        public bool? RequiredNullableBoolean { get; set; }

        public bool? OptionalBooleanField { get; set; }

        public List<IntListItem> IntList { get; set; }
        public List<StringListItem> StringList { get; set; }

        [ExistsIn("IntList", "Id", "Name")]
        public int? OptionalIntListId { get; set; }

        [Required]
        [ExistsIn("IntList", "Id", "Name")]
        public int? RequiredNullableIntListId { get; set; }

        [ExistsIn("StringList", "Value", "Label")]
        public string OptionalStringListId { get; set; }

        [ExistsIn("IntList", "Id", "Name")]
        public int RequiredIntListId { get; set; }

        [Required]
        [ExistsIn("StringList", "Value", "Label")]
        public string RequiredStringListId { get; set; }

        [ExistsIn("IntList", "Id", "Name")]
        [Required]
        public IEnumerable<int> RequiredIntListIds { get; set; }

        [Required]
        [ExistsIn("IntList", "Id", "Name")]
        public IList<int?> RequiredNullableIntListIds { get; set; }
        
        [ExistsIn("IntList", "Id", "Name")]
        public List<int> OptionalIntListIds { get; set; }

        [ExistsIn("IntList", "Id", "Name")]
        public ICollection<int?> OptionalNullableIntListIds { get; set; }

        [Editable(false)]
        public int ReadonlyInt { get; set; }

        public ChildViewModel Child { get; set; }
    }

    public class IntListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class StringListItem
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }

    public class ChildViewModel
    {
        public TestEnum RequiredChildEnum { get; set; }
        [RequiredFlagsEnum]
        public TestFlagsEnum RequiredChildFlagsEnum { get; set; }
    }

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    abstract class DefaultFieldGeneratorShould
    {
        protected HtmlHelper<TestFieldViewModel> H;
        protected IFieldConfiguration ExampleFieldConfiguration;

        [SetUp]
        public void Setup()
        {
            var autoSubstitute = AutoSubstituteContainer.Create();
            var viewDataDictionary = new ViewDataDictionary<TestFieldViewModel>(autoSubstitute.Resolve<IModelMetadataProvider>(), new ModelStateDictionary());

            H = autoSubstitute.Resolve<HtmlHelper<TestFieldViewModel>>();
            var viewContext = autoSubstitute.Resolve<ViewContext>(TypedParameter.From<ViewDataDictionary>(viewDataDictionary), TypedParameter.From(autoSubstitute.Resolve<ActionContext>()));
            viewContext.ClientValidationEnabled = true;
            H.Contextualize(viewContext);
            ExampleFieldConfiguration = new FieldConfiguration().Attr("data-attr", "value");

            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        }

        protected DefaultFieldGenerator<TestFieldViewModel, T> Arrange<T>(Expression<Func<TestFieldViewModel,T>> property, params Action<TestFieldViewModel>[] vmSetter)
        {
            H.ViewContext.ClientValidationEnabled = true;
            H.ViewContext.ViewData.ModelState.AddModelError(ExpressionHelper.GetExpressionText(property), "asdf");
            //DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredFlagsEnumAttribute), typeof(RequiredAttributeAdapter));
            var vm = new TestFieldViewModel();
            foreach (var action in vmSetter)
            {
                action(vm);
            }
            H.ViewData.Model = vm;

            return new DefaultFieldGenerator<TestFieldViewModel, T>(H, property, new DefaultFormTemplate());
        }

        class DefaultFieldGeneratorTests : DefaultFieldGeneratorShould
        {
            [Test]
            public void Not_throw_exception_getting_model_when_view_model_is_null()
            {
                var generator = Arrange(m => m.Decimal);
                H.ViewData.Model = null;

                generator.GetModel();
            }

            [Test]
            public void Not_throw_exception_getting_value_when_view_model_is_null()
            {
                var generator = Arrange(m => m.Decimal);
                H.ViewData.Model = null;

                generator.GetValue();
            }

            [Test]
            public void Return_property_name()
            {
                var generator = Arrange(m => m.DecimalWithFormatStringAttribute);

                var name = generator.GetFieldId();

                Assert.That(name, Is.EqualTo("DecimalWithFormatStringAttribute"));
            }

            [Test]
            public void Set_field_configuration_if_readonly_attribute_applied()
            {
                var generator = Arrange(m => m.ReadonlyInt);

                var configuration = generator.PrepareFieldConfiguration(ExampleFieldConfiguration, FieldParent.Section);

                Assert.That(configuration.HtmlAttributes["readonly"], Is.EqualTo("readonly"));
            }

            [Test]
            public void GetLabelHtml_should_return_display_attribute_if_WithoutLabelElement_used_and_DisplayAttribute_present()
            {
                var generator = Arrange(x => x.StringWithDisplayAttribute);
                var fieldConfig = new FieldConfiguration();
                fieldConfig.WithoutLabelElement();
                var config = generator.PrepareFieldConfiguration(fieldConfig, FieldParent.Section);
                var actual = generator.GetLabelHtml(config).ToString();
                Assert.That(actual, Is.EqualTo("Use this display name"));
            }
        }
    }
}