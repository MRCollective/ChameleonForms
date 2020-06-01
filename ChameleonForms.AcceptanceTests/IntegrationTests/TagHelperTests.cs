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
    [UseReporter(typeof(DiffReporter))]
    public class TagHelperTests : IClassFixture<WebApplicationFactory<Example.Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Example.Startup>
            _factory;

        public TagHelperTests(WebApplicationFactory<Example.Startup> factory)
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
            var renderedSource = GetRenderedSourceAsync("/Comparison/ChameleonFormsTH").Result;
            HtmlApprovals.VerifyHtml($"ChameleonFormsTH.cshtml\r\n\r\n{GetViewContents("Comparison/ChameleonFormsTH")}\r\n=====\r\n\r\nRendered Source\r\n\r\n{renderedSource}");
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

        private static readonly string ViewPath = Path.Combine(Path.GetDirectoryName(typeof(PartialForTests).Assembly.CodeBase.Replace("file:///", "")), "..", "..", "..", "..", "ChameleonForms.Example", "Views", "{0}.cshtml");
    }
}
