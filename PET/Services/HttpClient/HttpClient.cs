using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace PET.Services.HttpClient
{
    public class HttpClient : IHttpClient
    {
        private string _dom => "https://localhost:7026";
        private HttpResponseHeaders _headers;
        //public string JwtToken { get; set; }
        //public string JwtToken { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjE4MzkwNDQ4LTljYmYtNGMwOC1hYWExLWNlYTk1MTBhYzhlYyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlBlcnNvbiIsImV4cCI6MTkwOTU4MTExNiwiaXNzIjoiUGV0U2VydmVyIiwiYXVkIjoiUGV0Q2xpZW50In0.WQToB1Tm0txtXZL4rr98fGuPgQdfryhhfY3skAEGKuk";

        public string JwtToken { get; set; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6ImFiMDVkNTAxLTk4ZWYtNDM0ZS04NWYyLTAzZjA0NWU4OWFlMiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlBlcnNvbiIsImV4cCI6MTkwOTUxNzc5NCwiaXNzIjoiUGV0U2VydmVyIiwiYXVkIjoiUGV0Q2xpZW50In0.zxwH1D5ln-G3NRuVd3z5t7wazKO5WqC2YolmdPkyPbg";
        private readonly IHttpClientFactory httpClientFactory;
        public HttpClient(IHttpClientFactory httpClientFactory) => this.httpClientFactory = httpClientFactory;

        private string GetURL(string controller, string action = null, params string[] data)
        {
            string url = default;
            url += "/" + controller;
            url += "/" + action;

            foreach (var item in data)
                url += "/" + item;

            return _dom + url;
        }

        public async Task<OutT> GetAsync<OutT>(string controller = null, string action = null, params string[] data)
        {
            var httpClient = httpClientFactory.CreateClient();
            if(!string.IsNullOrEmpty(JwtToken))
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + JwtToken);
            
            using var response = await httpClient.GetAsync(GetURL(controller, action ,data));
            _headers = response.Headers;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadFromJsonAsync<OutT>(options) : default;
        }

        public async Task<OutT> PostGetJsonAsync<InT, OutT>(InT data, string controller = null, string action = null)
        {
            var httpClient = httpClientFactory.CreateClient();
            if (!string.IsNullOrEmpty(JwtToken))
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + JwtToken);

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                PropertyNameCaseInsensitive = true

                // Allows deserialize  DECIMAL  AND DOUBLE
                //NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
           
            using var response = await httpClient.PostAsJsonAsync(GetURL(controller, action), data, options);
            _headers = response.Headers;
           
            return response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadFromJsonAsync<OutT>(options) : default;
        }

        public string GetHeaderValue(in string headerName)
        {
            return _headers.TryGetValues(headerName, out IEnumerable<string> values) ? values.FirstOrDefault() : default;
        }
    }
}
