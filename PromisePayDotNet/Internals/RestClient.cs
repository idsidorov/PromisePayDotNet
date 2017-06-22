using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PromisePayDotNet.Internals
{

    internal class RestClient : IRestClient
    {
        private HttpClient _client;
        public RestClient()
        {
            _client = new HttpClient();
        }
        public Uri BaseUrl { get => _client.BaseAddress; set => _client.BaseAddress = value; }
        public IAuthenticator Authenticator { get; set; }

        public async Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            var rel = new Uri(request.url, UriKind.Relative);
            var req = new HttpRequestMessage
            {
                Method = request.Method,
                RequestUri = rel,
            };
			req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			if (request.Body != null)
            {
                req.Content = new StringContent(JsonConvert.SerializeObject(request.Body), Encoding.UTF8, "application/json");
            }
            else
            {
                req.Headers.Add("Content-Type", "application/json");
            }
            Authenticator?.Add(req);

            var result = await _client.SendAsync(req);

            return new RestResponse
            {
                Content = await result.Content.ReadAsStringAsync(),
                ResponseUri = new Uri(BaseUrl, rel),
                StatusCode = result.StatusCode,
                StatusDescription = result.ReasonPhrase
            };
        }
    }
}
