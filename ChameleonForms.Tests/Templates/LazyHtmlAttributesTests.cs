using System;
using System.Web;
using System.Web.Mvc;
using ChameleonForms.Templates;
using NUnit.Framework;

namespace ChameleonForms.Tests.Templates
{
    class LazyHtmlAttributesShould
    {
        [Test]
        public void Throw_exception_when_passed_null_html_generator()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new LazyHtmlAttributes(null));

            Assert.That(e.ParamName, Is.EqualTo("htmlGenerator"));
        }

        [Test]
        public void Use_the_given_generator_when_returning_html_string()
        {
            var h = new LazyHtmlAttributes(_ => new HtmlString("asdf"));
            h.AddClass("lol");

            Assert.That(h.ToHtmlString(), Is.EqualTo("asdf"));
        }

        [Test]
        public void Lazily_evaluate_the_html_generator()
        {
            var t = new TagBuilder("p");
            var h = new LazyHtmlAttributes(hh => { t.MergeAttributes(hh.Attributes); return new HtmlString(t.ToString(TagRenderMode.Normal));});
            h.AddClass("lol");
            t.InnerHtml = "hi";

            Assert.That(h.ToHtmlString(), Is.EqualTo("<p class=\"lol\">hi</p>"));
        }
    }
}
