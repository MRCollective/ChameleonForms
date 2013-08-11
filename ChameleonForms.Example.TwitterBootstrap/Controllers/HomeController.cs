using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Attributes;

namespace ChameleonForms.Example.TwitterBootstrap.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new ViewModelExample());
        }
    }

    public class ViewModelExample
    {
        public ViewModelExample()
        {
            // This could be set using a model binder if it's populated from a database or similar
            List = new List<ListItem> { new ListItem { Id = 1, Name = "A" }, new ListItem { Id = 2, Name = "B" } };
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