using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Attributes;
using ChameleonForms.Component.Config;

namespace ChameleonForms.Example.Controllers
{
    public class ExampleFormsController : Controller
    {
        public ActionResult Form1()
        {
            return View(new ViewModelExample());
        }

        [HttpPost]
        public ActionResult Form1(ViewModelExample vm)
        {
            return View(vm);
        }

        public ActionResult BasicExample()
        {
            return View(new BasicViewModel());
        }

        public ActionResult ModelBindingExample()
        {
            return View(new ModelBindingViewModel());
        }

        [HttpPost]
        public ActionResult ModelBindingExample(ModelBindingViewModel vm)
        {
            return View(vm);
        }

        public ActionResult ModelBindingExample2()
        {
            var vm = new ModelBindingViewModel();
            vm.AsList();
            return View("ModelBindingExample", vm);
        }

        [HttpPost]
        public ActionResult ModelBindingExample2(ModelBindingViewModel vm)
        {
            vm.AsList();
            return View("ModelBindingExample", vm);
        }

        public ActionResult NullModel()
        {
            return View();
        }
    }

    public class ModelBindingViewModel
    {
        private bool _aslist;

        public ModelBindingViewModel()
        {
            List = new List<ListItem> { new ListItem { Id = 1, Name = "A" }, new ListItem { Id = 2, Name = "B" } };
        }

        public void AsList()
        {
            _aslist = true;
        }

        public IFieldConfiguration ModifyConfig(IFieldConfiguration config)
        {
            if (_aslist)
                config.AsList();
            return config;
        }

        [Required]
        public string RequiredString { get; set; }

        public int RequiredInt { get; set; }
        public int? OptionalInt { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NullableDateTime { get; set; }

        public bool RequiredBool { get; set; }
        [Required]
        public bool? RequiredNullableBool { get; set; }
        public bool? OptionalBool { get; set; }
        public bool? OptionalBool2 { get; set; }
        public bool? OptionalBool3 { get; set; }

        public SomeEnum RequiredEnum { get; set; }
        [Required]
        public SomeEnum? RequiredNullableEnum { get; set; }
        public SomeEnum? OptionalEnum { get; set; }

        [Required]
        public IEnumerable<SomeEnum> RequiredEnums { get; set; }
        [Required]
        public IEnumerable<SomeEnum?> RequiredNullableEnums { get; set; }
        public IEnumerable<SomeEnum> OptionalEnums { get; set; }
        public IEnumerable<SomeEnum?> OptionalNullableEnums { get; set; }

        [ReadOnly(true)]
        public List<ListItem> List { get; set; }

        [ExistsIn("List", "Id", "Name")]
        public int RequiredListId { get; set; }
        [Required]
        [ExistsIn("List", "Id", "Name")]
        public int? RequiredNullableListId { get; set; }
        [ExistsIn("List", "Id", "Name")]
        public int? OptionalListId { get; set; }

        [ExistsIn("List", "Id", "Name")]
        [Required]
        public IEnumerable<int> RequiredListIds { get; set; }
        [Required]
        [ExistsIn("List", "Id", "Name")]
        public IEnumerable<int?> RequiredNullableListIds { get; set; }
        [ExistsIn("List", "Id", "Name")]
        public IEnumerable<int> OptionalListIds { get; set; }
        [ExistsIn("List", "Id", "Name")]
        public IEnumerable<int?> OptionalNullableListIds { get; set; }
    }

    public class BasicViewModel
    {
        [Required]
        public string RequiredString { get; set; }

        public SomeEnum SomeEnum { get; set; }

        public bool SomeCheckbox { get; set; }
    }

    public class ViewModelExample
    {
        public ViewModelExample()
        {
            // This could be set using a model binder if it's populated from a database or similar
            List = new List<ListItem> {new ListItem{Id = 1, Name = "A"}, new ListItem{Id = 2, Name = "B"}};
            DecimalWithFormatStringAttribute = 1.2300m;
            Decimal = 1.2300m;
        }

        [Required]
        public string RequiredStringField { get; set; }

        public string NestedField { get; set; }

        public SomeEnum SomeEnum { get; set; }

        public List<SomeEnum> SomeEnums { get; set; }
        public List<SomeEnum> SomeEnumsList { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string TextAreaField { get; set; }

        public bool SomeCheckbox { get; set; }

        public List<ListItem> List { get; set; }
        [ExistsIn("List", "Id", "Name")]
        public int ListId { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal DecimalWithFormatStringAttribute { get; set; }

        public decimal Decimal { get; set; }

        public int? NullableInt { get; set; }

        public bool Boolean { get; set; }
    }

    public class ListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public enum SomeEnum
    {
        Value1,
        [Description("Friendly name")]
        ValueWithDescription,
        SomeOtherValue
    }
}