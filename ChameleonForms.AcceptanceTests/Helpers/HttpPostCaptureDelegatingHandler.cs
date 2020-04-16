using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ChameleonForms.AcceptanceTests.Helpers
{
    public class HttpPostCaptureDelegatingHandler : DelegatingHandler
    {
        private readonly List<string> _posts = new List<string>();
        public IEnumerable<string> Posts => _posts;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post)
            {
                var postHeaders = string.Join("\n", request.Content.Headers.Select(h => $"{h.Key}: {string.Join(";", h.Value)}"));
                var postBody = await request.Content.ReadAsStringAsync();

                _posts.Add($"{postHeaders}\n\n{postBody}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
