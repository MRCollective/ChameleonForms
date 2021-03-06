﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ChameleonForms.Attributes;
using ChameleonForms.Component.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public ActionResult Form2()
        {
            return View(new ViewModelExample());
        }

        [HttpPost]
        public ActionResult Form2(ViewModelExample vm)
        {
            return View(vm);
        }

        public ActionResult BasicExample()
        {
            return View(new BasicViewModel());
        }

        [HttpPost]
        public ActionResult BasicExample(BasicViewModel vm)
        {
            return View(vm);
        }

        public ActionResult Labels()
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

        public ActionResult NullModelWithList()
        {
            return View();
        }

        public ActionResult NullList()
        {
            return View(new ViewModelExample {List = null});
        }

        public ActionResult Buttons()
        {
            return View();
        }

        public ActionResult ButtonsTH()
        {
            return View();
        }

        public ActionResult Partials()
        {
            return View(new ViewModelExample{TextAreaField = "Initial value"});
        }

        public ActionResult PartialsTH()
        {
            return View(new ViewModelExample { TextAreaField = "Initial value" });
        }

        [HttpPost]
        public ActionResult Partials(ViewModelExample vm)
        {
            return View(vm);
        }

        public ActionResult ChangingContext()
        {
            return View(new ParentViewModel{Child = new ChildViewModel { ChildField = "Initial value from controller"}});
        }

        public ActionResult ChangingContextTH()
        {
            return View(new ParentViewModel { Child = new ChildViewModel { ChildField = "Initial value from controller" } });
        }

        [HttpPost]
        public ActionResult PostDifferentModel(BasicViewModel vm, bool tagHelpers)
        {
            ViewBag.Action = "PostDifferentModel";
            ViewBag.ModelType = vm.GetType().Name;
            ViewBag.BoundModelData = JsonSerializer.Serialize(vm, new JsonSerializerOptions{WriteIndented = true});
            ViewBag.BoundModel = vm;
            return View(tagHelpers ? "ChangingContextTH" : "ChangingContext");
        }

        [HttpPost]
        public ActionResult PostChildViewModel(ChildViewModel vm, bool tagHelpers)
        {
            ViewBag.Action = "PostChildViewModel";
            ViewBag.ModelType = vm.GetType().Name;
            ViewBag.BoundModelData = JsonSerializer.Serialize(vm, new JsonSerializerOptions { WriteIndented = true });
            ViewBag.BoundModel = vm;
            return View(tagHelpers ? "ChangingContextTH" : "ChangingContext");
        }

        [HttpPost]
        public ActionResult PostParentViewModel(ParentViewModel vm, bool tagHelpers)
        {
            ViewBag.Action = "PostParentViewModel";
            ViewBag.ModelType = vm.GetType().Name;
            ViewBag.BoundModelData = JsonSerializer.Serialize(vm, new JsonSerializerOptions { WriteIndented = true });
            ViewBag.BoundModel = vm;
            return View(tagHelpers ? "ChangingContextTH" : "ChangingContext");
        }
    }

    public class ModelBindingViewModel
    {
        private bool _aslist;

        public ModelBindingViewModel()
        {
            List = new List<ListItem> { new ListItem { Id = 1, Name = "A" }, new ListItem { Id = 2, Name = "B" } };
            ChoicesAsTuples = new[] { Tuple.Create(1, "A"), Tuple.Create(2, "B") };
        }

        public void AsList()
        {
            _aslist = true;
        }

        public IFieldConfiguration ModifyConfig(IFieldConfiguration config)
        {
            if (_aslist)
                config.AsRadioList();
            return config;
        }

        [Required]
        public string RequiredString { get; set; }

        public int RequiredInt { get; set; }
        public int? OptionalInt { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NullableDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeWithG { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime? NullableDateTimeWithG { get; set; }

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

        public FlagsEnum RequiredFlagsEnum { get; set; }
        [Required]
        public FlagsEnum? RequiredNullableFlagsEnum { get; set; }
        public FlagsEnum? OptionalFlagsEnum { get; set; }

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
        /*todo: Support this
         [ExistsIn("List", "Id", "Name")]
        public IEnumerable<int> OptionalListIds { get; set; }*/
        [ExistsIn("List", "Id", "Name")]
        public IEnumerable<int?> OptionalNullableListIds { get; set; }


        // These are tested in addition to the other list tests as there
        // was a bug manifesting when using an array of a templated type.
        [ReadOnly(true)]
        public Tuple<Int32, String>[] ChoicesAsTuples { get; set; }

        [Required]
        [ExistsIn("ChoicesAsTuples", "Item1", "Item2")]
        public Int32? Choice { get; set; }

        [DataType(DataType.MultilineText)]
        public string Textarea { get; set; }
    }

    public class BasicViewModel
    {
        [Required]
        public string RequiredString { get; set; }

        public SomeEnum SomeEnum { get; set; }

        public bool SomeCheckbox { get; set; }
    }

    public class ParentViewModel
    {
        public string Field { get; set; }
        public ChildViewModel Child { get; set; }
    }

    public class ViewModelExampleBase
    {
        public ViewModelExampleBase()
        {
            BaseChild = new ViewModelExampleBaseChild();
        }

        public string BaseField { get; set; }
        public string BaseField2 { get; set; }

        public ViewModelExampleBaseChild BaseChild { get; set; }
    }

    public class ViewModelExampleBaseChild
    {
        public string BaseChildField { get; set; }
        public string BaseChildField2 { get; set; }
    }

    public class ViewModelExample : ViewModelExampleBase
    {
        public ViewModelExample()
        {
            // This could be set using a model binder if it's populated from a database or similar
            List = new List<ListItem> {new ListItem{Id = 1, Name = "A"}, new ListItem{Id = 2, Name = "B"}};
            DecimalWithFormatStringAttribute = 1.2300m;
            Decimal = 1.2300m;
            Child = new ChildViewModel();
        }

        [Required]
        public string RequiredStringField { get; set; }

        public string NestedField { get; set; }

        [Required]
        public FlagsEnum? FlagsEnums { get; set; }

        public SomeEnum SomeEnum { get; set; }

        public List<SomeEnum> SomeEnums { get; set; }
        public List<SomeEnum> SomeEnumsList { get; set; }
                
        [DataType(DataType.MultilineText)]
        public string TextAreaField { get; set; }

        public bool SomeCheckbox { get; set; }

        [ReadOnly(true)]
        public List<ListItem> List { get; set; }
        [ExistsIn("List", "Id", "Name")]
        public int ListId { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal DecimalWithFormatStringAttribute { get; set; }

        public decimal Decimal { get; set; }

        public int Int { get; set; }

        public int? NullableInt { get; set; }

        public bool Boolean { get; set; }

        [Required]
        public IFormFile FileUpload { get; set; }

        public ChildViewModel Child { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime DateField { get; set; }

        public Uri UrlAsUri { get; set; }

        [DataType(DataType.Url)]
        public string UrlAsString { get; set; }
    }

    public class ListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Flags]
    public enum FlagsEnum
    {
        One = 1,
        Two = 2,
        Three = 4,
        Four = 8,
        Five = 16
    }

    public enum SomeEnum
    {
        Value1,
        [Description("Friendly name")]
        ValueWithDescription,
        SomeOtherValue
    }

    public class ChildViewModel
    {
        [Required]
        public string ChildField { get; set; }
        public SomeEnum SomeEnum { get; set; }
    }
}