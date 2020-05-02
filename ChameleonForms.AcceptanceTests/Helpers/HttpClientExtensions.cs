using System;
using System.Net.Http;
using System.Threading.Tasks;
using ChameleonForms.AcceptanceTests.Helpers.Pages;

namespace ChameleonForms.AcceptanceTests.Helpers
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetPageAsync<T>(this HttpClient client, string url) where T : IChameleonFormsPage
        {
            var html = await client.GetAsync(url);
            var content = await HtmlHelpers.GetDocumentAsync(client, html);
            return (T) Activator.CreateInstance(typeof(T), content);
        }
    }
}
