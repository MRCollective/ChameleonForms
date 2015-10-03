using ChameleonForms.Component;
using ChameleonForms.Component.Partial;
using ChameleonForms.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChameleonForms.Tests.Component
{
    public class TestPartialForChildViewModel
    {
        public string SomeProperty { get; set; }
    }

    public class TestPartialForViewModel
    {
        public TestPartialForChildViewModel ChildView { get; set; }
    }

    [TestFixture]
    public class PartialForTests
    {
        private readonly IHtmlString _beginHtml = new HtmlString("");
        private readonly IHtmlString _endHtml = new HtmlString("");
        private readonly IHtmlString _nestedBeginHtml = new HtmlString("");
        private readonly IHtmlString _nestedEndHtml = new HtmlString("");
        private IForm<TestPartialForViewModel> _f;
        private readonly IHtmlString _heading = new HtmlString("title");

        [SetUp]
        public void Setup()
        {
            _f = Substitute.For<IForm<TestPartialForViewModel>>();
            var autoSubstitute = AutoSubstituteContainer.Create();
            var helper = autoSubstitute.Resolve<HtmlHelper<TestPartialForViewModel>>();
            helper.ViewData.Model = new TestPartialForViewModel();
            var requestContext = autoSubstitute.Resolve<RequestContext>();
            requestContext.RouteData.Values.Add("controller", "PartialForFakeController");
            _f.HtmlHelper.Returns(helper);
            _f.Template.BeginSection(Arg.Is<IHtmlString>(h => h.ToHtmlString() == _heading.ToHtmlString()), Arg.Any<IHtmlString>(), Arg.Any<HtmlAttributes>()).Returns(_beginHtml);
            _f.Template.EndSection().Returns(_endHtml);
            _f.Template.BeginNestedSection(Arg.Is<IHtmlString>(h => h.ToHtmlString() == _heading.ToHtmlString()), Arg.Any<IHtmlString>(), Arg.Any<HtmlAttributes>()).Returns(_nestedBeginHtml);
            _f.Template.EndNestedSection().Returns(_nestedEndHtml);
        }

        private ISection<TestPartialForChildViewModel> Arrange(bool isNested)
        {
            var section = new Section<TestPartialForViewModel>(_f, _heading, isNested);
            return new PartialProxySection<TestPartialForViewModel, TestPartialForChildViewModel>(section, x => x.ChildView);
        }

        [Test]
        [Ignore]
        public void Test1()
        {
            var ss = _f.PartialFor(x => x.ChildView, "PartialChildView").ToHtmlString();
        }
    }
}