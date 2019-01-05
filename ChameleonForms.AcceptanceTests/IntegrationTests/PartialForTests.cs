using ApprovalTests.Html;
using ApprovalTests.Reporters;
using Microsoft.AspNetCore.Mvc.Testing;
using RazorPagesProject.Tests.Helpers;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace ChameleonForms.AcceptanceTests
{
    /// <summary>
    /// Loading partial views is very difficult to test by unit testing.
    /// </summary>
    [UseReporter(typeof(DiffReporter))]
    public class PartialForTests : IClassFixture<WebApplicationFactory<ChameleonForms.Example.Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<ChameleonForms.Example.Startup>
            _factory;

        public PartialForTests(WebApplicationFactory<ChameleonForms.Example.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public void Should_render_correctly_when_used_via_form_or_section_and_when_used_for_top_level_property_or_sub_property()
        {
            var renderedSource = GetRenederedSourceAsync("/ExampleForms/Partials").Result;
            HtmlApprovals.VerifyHtml(string.Format("Partials.cshtml\r\n\r\n{0}\r\n=====\r\n\r\n_ParentPartial.cshtml\r\n\r\n{1}\r\n=====\r\n\r\n_ChildPartial.cshtml\r\n\r\n{2}\r\n=====\r\n\r\n_BaseParentPartial.cshtml\r\n\r\n{3}\r\n=====\r\n\r\n_BaseChildPartial.cshtml\r\n\r\n{4}\r\n=====\r\n\r\nRendered Source\r\n\r\n{5}",
                GetViewContents("Partials"),
                GetViewContents("_ParentPartial"),
                GetViewContents("_ChildPartial"),
                GetViewContents("_BaseParentPartial"),
                GetViewContents("_BaseChildPartial"),
                renderedSource));
        }

        private async Task<string> GetRenederedSourceAsync(string url)
        {
            var defaultPage = await _client.GetAsync(url);
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            var renderedSource = content.Body.InnerHtml;
            var getFormContent = new Regex(@".*?(<form(.|\n|\r)+?<\/form>).*", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            return getFormContent.Match(renderedSource).Groups[1].Value;
        }

        private string GetViewContents(string viewPath)
        {
            return File.ReadAllText(string.Format(ViewPath, viewPath));
        }

        private static readonly string ViewPath = Path.Combine(Path.GetDirectoryName(typeof(PartialForTests).Assembly.CodeBase.Replace("file:///", "")), "..", "..", "..", "..", "ChameleonForms.Example", "Views", "ExampleForms", "{0}.cshtml");
    }
}
