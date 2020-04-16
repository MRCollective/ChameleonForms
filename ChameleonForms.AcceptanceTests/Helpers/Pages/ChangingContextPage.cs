using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ChameleonForms.Example.Controllers;

namespace ChameleonForms.AcceptanceTests.Helpers.Pages
{
    public class ChangingContextPage : ChameleonFormsPage<ParentViewModel>
    {
        public ChangingContextPage(IDocument content) : base(content)
        {
        }

        public BasicViewModel ReadDifferentModel()
        {
            return GetComponent<PageAsDifferentModel, BasicViewModel>().GetFormValues();
        }

        public async Task<ChangingContextPage> PostDifferentModelAsync(BasicViewModel vm)
        {
            GetComponent<PageAsDifferentModel, BasicViewModel>().InputModel(vm);
            return await NavigateToAsync<ChangingContextPage>(Content.QuerySelector("button[type=submit].different-model"));
        }

        public ChildViewModel ReadChildModel()
        {
            return GetComponent<PageAsChildModel, ChildViewModel>().GetFormValues();
        }

        public async Task<ChangingContextPage> PostChildModelAsync(ChildViewModel vm)
        {
            GetComponent<PageAsChildModel, ChildViewModel>().InputModel(vm);
            return await NavigateToAsync<ChangingContextPage>(Content.QuerySelector("button[type=submit].child-model"));
        }

        public ParentViewModel ReadParentModel()
        {
            return GetFormValues();
        }

        public async Task<ChangingContextPage> PostParentModelAsync(ParentViewModel vm)
        {
            InputModel(vm);
            return await NavigateToAsync<ChangingContextPage>(Content.QuerySelector("button[type=submit].parent-model"));
        }

        public class PageAsDifferentModel : ChameleonFormsPage<BasicViewModel>
        {
            public PageAsDifferentModel(IHtmlDocument content) : base(content)
            {
            }
        }

        public class PageAsChildModel : ChameleonFormsPage<ChildViewModel>
        {
            public PageAsChildModel(IHtmlDocument content) : base(content)
            {
            }
        }
    }
}
