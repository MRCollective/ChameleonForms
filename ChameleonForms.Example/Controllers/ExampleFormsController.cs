using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Attributes;

namespace ChameleonForms.Example.Controllers
{
    public class ExampleFormsController : Controller
    {
        public ActionResult Form1()
        {
            return View(new ViewModelExample());
        }

        [HttpPost]
        public ActionResult Form1Submit(ViewModelExample vm)
        {
            return View(vm.FileUpload.ContentLength);
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
    }

    public class ModelBindingViewModel
    {
        public ModelBindingViewModel()
        {
            List = new List<ListItem> { new ListItem { Id = 1, Name = "A" }, new ListItem { Id = 2, Name = "B" } };
        }

        [Required]
        public string RequiredString { get; set; }

        public int RequiredInt { get; set; }
        public int? OptionalInt { get; set; }

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
    }

    public class ListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public enum SomeEnum
    {
        Value1,
        [Description("Fiendly name")]
        ValueWithDescription,
        SomeOtherValue
    }
}