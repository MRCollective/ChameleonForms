using System;
using System.IO;
using System.Text.RegularExpressions;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ChameleonForms.AcceptanceTests
{
    /// <summary>
    /// Loading partial views is very difficult to test by unit testing.
    /// </summary>
    [UseReporter(typeof(DiffReporter))]
    public class PartialForTests
    {
        [Test]
        public void Should_render_correctly_when_used_via_form_or_section_and_when_used_for_top_level_property_or_sub_property()
        {
            var renderedSource = GetRenederedSource("/ExampleForms/Partials");
            HtmlApprovals.VerifyHtml(string.Format("Partials.cshtml\r\n\r\n{0}\r\n=====\r\n\r\n_ParentPartial.cshtml\r\n\r\n{1}\r\n=====\r\n\r\n_ChildPartial.cshtml\r\n\r\n{2}\r\n=====\r\n\r\n_BaseParentPartial.cshtml\r\n\r\n{3}\r\n=====\r\n\r\n_BaseChildPartial.cshtml\r\n\r\n{4}\r\n=====\r\n\r\nRendered Source\r\n\r\n{5}",
                GetViewContents("Partials"),
                GetViewContents("_ParentPartial"),
                GetViewContents("_ChildPartial"),
                GetViewContents("_BaseParentPartial"),
                GetViewContents("_BaseChildPartial"),
                renderedSource));
        }

        private string GetRenederedSource(string url)
        {
            Host.Instance.Application.Browser.Navigate().GoToUrl(string.Format("http://localhost:12345{0}", url));
            new WebDriverWait(Host.Instance.Application.Browser, TimeSpan.FromSeconds(5))
                .Until(b => b.FindElement(By.Id("Int")));
            var renderedSource = Host.Instance.Application.Browser.PageSource;
            var getFormContent = new Regex(@".*?(<form(.|\n|\r)+?<\/form>).*", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            return getFormContent.Match(renderedSource).Groups[1].Value;
        }

        private string GetViewContents(string viewPath)
        {
            return File.ReadAllText(string.Format(ViewPath, viewPath));
        }

        private static readonly string ViewPath = Path.Combine(Path.GetDirectoryName(typeof(PartialForTests).Assembly.CodeBase.Replace("file:///", "")), "..", "..", "..", "ChameleonForms.Example", "Views", "ExampleForms", "{0}.cshtml");
    }
}
