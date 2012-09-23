using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

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
    }

    public class ViewModelExample
    {
        [Required]
        public string RequiredStringField { get; set; }

        public string NestedField { get; set; }

        public SomeEnum SomeEnum { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }
    }

    public enum SomeEnum
    {
        Value1,
        [Description("Fiendly name")]
        ValueWithDescription,
        SomeOtherValue
    }
}