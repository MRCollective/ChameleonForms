using ChameleonForms.Example.Controllers;
using OpenQA.Selenium;

namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages
{
    public class ChangingContextPage : ChameleonFormsPage<ParentViewModel>
    {
        public BasicViewModel ReadDifferentModel()
        {
            return GetComponent<PageAsDifferentModel>().GetFormValues();
        }

        public ChangingContextPage PostDifferentModel(BasicViewModel vm)
        {
            GetComponent<PageAsDifferentModel>().Input(vm);
            return Navigate.To<ChangingContextPage>(By.CssSelector("button[type=submit].different-model"));
        }

        public ChildViewModel ReadChildModel()
        {
            return GetComponent<PageAsChildModel>().GetFormValues();
        }

        public ChangingContextPage PostChildModel(ChildViewModel vm)
        {
            GetComponent<PageAsChildModel>().Input(vm);
            return Navigate.To<ChangingContextPage>(By.CssSelector("button[type=submit].child-model"));
        }

        public ParentViewModel ReadParentModel()
        {
            return GetFormValues();
        }

        public ChangingContextPage PostParentModel(ParentViewModel vm)
        {
            InputModel(vm);
            return Navigate.To<ChangingContextPage>(By.CssSelector("button[type=submit].parent-model"));
        }

        public class PageAsDifferentModel : ChameleonFormsPage<BasicViewModel>
        {
            public void Input(BasicViewModel vm)
            {
                InputModel(vm);
            }
        }
        public class PageAsChildModel : ChameleonFormsPage<ChildViewModel>
        {
            public void Input(ChildViewModel vm)
            {
                InputModel(vm);
            }
        }
    }
}
