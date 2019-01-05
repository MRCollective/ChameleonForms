using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChameleonForms.Example.Controllers
{
    public class ComparisonController : Controller
    {
        public ActionResult ChameleonForms()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChameleonForms(SignupViewModel vm)
        {
            return View(vm);
        }

        public ActionResult EditorTemplates()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditorTemplates(SignupViewModel vm)
        {
            return View(vm);
        }

        public ActionResult HtmlHelpers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HtmlHelpers(SignupViewModel vm)
        {
            return View(vm);
        }

        public class SignupViewModel
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            [DisplayFormat(DataFormatString = "d/M/yyyy", ApplyFormatInEditMode = true)]
            public DateTime DateOfBirth { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            public string EmailAddress { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Required]
            public MembershipType MembershipType { get; set; }

            [DataType(DataType.Url)]
            public string Homepage { get; set; }
            [DataType(DataType.MultilineText)]
            public string Bio { get; set; }

            [Required]
            public bool TermsAndConditions { get; set; }
        }

        public enum MembershipType
        {
            Standard,
            Bonze,
            Silver,
            Gold,
            Platinum
        }
    }
}
