
namespace PET.Services.HttpClient
{
    public interface IHttpClient
    {
        public string JwtToken { get; set; }
        public Task<OutT> GetAsync<OutT>(string controller = null, string action = null, params string[] data);
        public Task<OutT> PostGetJsonAsync<InT, OutT>(InT data, string controller = null, string action = null);
        public string GetHeaderValue(in string headerName);
    }
}
