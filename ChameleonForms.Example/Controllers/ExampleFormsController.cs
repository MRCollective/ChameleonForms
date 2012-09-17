using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ChameleonForms.Example.Controllers
{
    public class ExampleFormsController : Controller
    {
        public ActionResult Form1()
        {
            return View(new ViewModelExample());
        }
    }

    public class ViewModelExample
    {
        [Required]
        public string RequiredStringField { get; set; }

        public string NestedField { get; set; }

        public SomeEnum SomeEnum { get; set; }
    }

    public enum SomeEnum
    {
        Value1,
        [Description("Fiendly name")]
        ValueWithDescription,
        SomeOtherValue
    }
}