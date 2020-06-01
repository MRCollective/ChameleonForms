using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApprovalTests.Html;
using ApprovalTests.Reporters;
using ChameleonForms.AcceptanceTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ChameleonForms.AcceptanceTests.IntegrationTests
{
    /// <summary>
    /// Loading partial views is very difficult to test by unit testing.
    /// </summary>
    [UseReporter(typeof(DiffReporter))]
    public class PartialForTests : IClassFixture<WebApplicationFactory<Example.Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Example.Startup>
            _factory;

        public PartialForTests(WebApplicationFactory<Example.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Should_render_correctly_when_used_via_form_or_section_and_when_used_for_top_level_property_or_sub_property()
        {
            var renderedSource = await GetRenderedSourceAsync("/ExampleForms/Partials");
            HtmlApprovals.VerifyHtml($"Partials.cshtml\r\n\r\n{GetViewContents("Partials")}\r\n=====\r\n\r\n_ParentPartial.cshtml\r\n\r\n{GetViewContents("_ParentPartial")}\r\n=====\r\n\r\n_ChildPartial.cshtml\r\n\r\n{GetViewContents("_ChildPartial")}\r\n=====\r\n\r\n_BaseParentPartial.cshtml\r\n\r\n{GetViewContents("_BaseParentPartial")}\r\n=====\r\n\r\n_BaseChildPartial.cshtml\r\n\r\n{GetViewContents("_BaseChildPartial")}\r\n=====\r\n\r\nRendered Source\r\n\r\n{renderedSource}");
        }

        [Fact]
        public async Task Should_render_correctly_when_used_via_form_or_section_and_when_used_for_top_level_property_or_sub_property_via_tag_helpers()
        {
            var renderedSource = await GetRenderedSourceAsync("/ExampleForms/PartialsTH");
            HtmlApprovals.VerifyHtml($"PartialsTH.cshtml\r\n\r\n{GetViewContents("PartialsTH")}\r\n=====\r\n\r\n_ParentPartialTH.cshtml\r\n\r\n{GetViewContents("_ParentPartialTH")}\r\n=====\r\n\r\n_ChildPartialTH.cshtml\r\n\r\n{GetViewContents("_ChildPartialTH")}\r\n=====\r\n\r\n_BaseParentPartialTH.cshtml\r\n\r\n{GetViewContents("_BaseParentPartialTH")}\r\n=====\r\n\r\n_BaseChildPartialTH.cshtml\r\n\r\n{GetViewContents("_BaseChildPartialTH")}\r\n=====\r\n\r\nRendered Source\r\n\r\n{renderedSource}");
        }

        private async Task<string> GetRenderedSourceAsync(string url)
        {
            var defaultPage = await _client.GetAsync(url);
            var content = await HtmlHelpers.GetDocumentAsync(_client, defaultPage);

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
