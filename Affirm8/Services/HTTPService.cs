using Affirm8.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Affirm8.Services
{
    public class HTTPService : IHTTPService
    {
        private HttpClient _httpClient;
        private JsonSerializerOptions _serializerOptions;
        private const string _URI = "https://hfv2f0qr-7027.euw.devtunnels.ms/api/";
        private string? _token;

        public HTTPService()
        {
            _httpClient = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<bool> Login(string userEmail, string userPassword)
        {
            Uri uri = new Uri(string.Format(_URI + "Authentication/login", string.Empty));

            var loginObject = new { email = userEmail, password = "string" };
            string json = JsonSerializer.Serialize(loginObject);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var token = JsonSerializer.Deserialize<LoginModel>(responseData, _serializerOptions);
                _token = token?.Token;
                return true;
            }
            return false;
        }

        public async Task<bool> Register(string userEmail, string userPassword, string userNickName)
        {
            Uri uri = new Uri(string.Format(_URI + "Authentication/register", string.Empty));

            var registerObject = new { email = userEmail, password = userPassword, nickName = userNickName };
            string json = JsonSerializer.Serialize(registerObject);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                //string responseData = await response.Content.ReadAsStringAsync();
                //var token = JsonSerializer.Deserialize<LoginModel>(responseData, _serializerOptions);
                //_token = token?.Token;
                return true;
            }
            return false;
            }

        

        public async Task<ObservableCollection<Post>> GetPosts()
            {
            return new ObservableCollection<Post>();
        }
    }
}
