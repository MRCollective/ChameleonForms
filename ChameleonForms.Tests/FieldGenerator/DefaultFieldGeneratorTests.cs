using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using ApprovalTests.Reporters;
using ChameleonForms.Attributes;
using ChameleonForms.FieldGenerators;
using ChameleonForms.Tests.Helpers;
using NUnit.Framework;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace ChameleonForms.Tests.FieldGenerator
{
    public enum TestEnum
    {
        Simplevalue,
        [Description("Description attr text")]
        ValueWithDescriptionAttribute,
        ValueWithMultpipleWordsAndNoDescriptionAttribute,
    }

    public class TestFieldViewModel
    {
        [Required]
        public string RequiredString { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public Decimal DecimalWithFormatStringAttribute { get; set; }

        public Decimal Decimal { get; set; }

        public DateTime DateTime { get; set; }
        public DateTime? NullableDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeWithFormat { get; set; }

        [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NullableDateTimeWithFormat { get; set; }

        public TestEnum RequiredEnum { get; set; }

        [Required]
        public TestEnum? RequiredNullableEnum { get; set; }

        [DisplayFormat(NullDisplayText = "Nothing to see here")]
        public TestEnum? OptionalEnumWithNullStringAttribute { get; set; }

        public TestEnum? OptionalEnum { get; set; }

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

        public HttpPostedFileBase FileUpload { get; set; }

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

    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    abstract class DefaultFieldGeneratorShould
    {
        protected HtmlHelper<TestFieldViewModel> _h;

        [SetUp]
        public void Setup()
        {
            var autoSubstitute = AutoSubstituteContainer.Create();
            _h = autoSubstitute.Resolve<HtmlHelper<TestFieldViewModel>>();
        }

        protected DefaultFieldGenerator<TestFieldViewModel, T> Arrange<T>(Expression<Func<TestFieldViewModel,T>> property, params Action<TestFieldViewModel>[] vmSetter)
        {
            _h.ViewContext.UnobtrusiveJavaScriptEnabled = true;
            _h.ViewContext.ClientValidationEnabled = true;
            _h.ViewContext.ViewData.ModelState.AddModelError(ExpressionHelper.GetExpressionText(property), "asdf");
            var vm = new TestFieldViewModel();
            foreach (var action in vmSetter)
            {
                action(vm);
            }
            _h.ViewData.Model = vm;
            _h.ViewData.ModelMetadata.Model = vm;

            return new DefaultFieldGenerator<TestFieldViewModel, T>(_h, property);
        }

        [Test]
        public void Not_throw_exception_getting_model_when_view_model_is_null()
        {
            var generator = Arrange(m => m.Decimal);
            _h.ViewData.Model = null;
            _h.ViewData.ModelMetadata.Model = null;
            
            generator.GetModel();
        }

        [Test]
        public void Not_throw_exception_getting_value_when_view_model_is_null()
        {
            var generator = Arrange(m => m.Decimal);
            _h.ViewData.Model = null;
            _h.ViewData.ModelMetadata.Model = null;

            generator.GetValue();
        }
        
        [Test]
        public void Return_property_name()
        {
            var generator = Arrange(m => m.DecimalWithFormatStringAttribute);

            var name = generator.GetPropertyName();

            Assert.That(name, Is.EqualTo("DecimalWithFormatStringAttribute"));
        }
    }
}