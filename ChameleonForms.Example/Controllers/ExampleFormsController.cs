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

        public ActionResult BasicExample()
        {
            return View(new BasicViewModel());
        }

        [HttpPost]
        public ActionResult Form1Submit(ViewModelExample vm)
        {
            return View(vm.FileUpload.ContentLength);
        }
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