using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Component;
using ChameleonForms.Tests.FieldGenerator;
using ChameleonForms.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace ChameleonForms.Tests
{
    [TestFixture]
    class FormContextShould
    {
        [SetUp]
        public void Setup()
        {
            var autoSubstitute = AutoSubstituteContainer.Create();
            _h = autoSubstitute.Resolve<HtmlHelper<TestFieldViewModel>>();
        }

        private HtmlHelper<TestFieldViewModel> _h;
        [Test]
        public void Output_no_markup_for_an_empty_editor()
        {
            using (var f = _h.BeginChameleonFormContext())
            {
            }

            _h.ViewContext.Writer.DidNotReceive().Write(Arg.Any<IHtmlString>());
        }

        [Test]
        public void Output_some_markup_for_an_editor_with_a_section_and_field()
        {
            using (var f = _h.BeginChameleonFormContext())
            {
                using (var s = f.BeginSection("Section"))
                {
                    s.FieldFor(x => x.Decimal);
                }
            }

            _h.ViewContext.Writer.Received().Write(Arg.Any<IHtmlString>());
        }
    }
}
