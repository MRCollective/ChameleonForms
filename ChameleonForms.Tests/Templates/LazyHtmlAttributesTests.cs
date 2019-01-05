using System;
using System.Web;

using ChameleonForms.Templates;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var t = new TagBuilder("p")
            {
                TagRenderMode = TagRenderMode.Normal
            };
            var h = new LazyHtmlAttributes(hh => 
            {
                t.MergeAttributes(hh.Attributes);
                return new HtmlString(t.ToHtmlString());
            });
            h.AddClass("lol");
            t.InnerHtml.Append("hi");

            Assert.That(h.ToHtmlString(), Is.EqualTo("<p class=\"lol\">hi</p>"));
        }
    }
}
