using Affirm8.Models;
using System.Text.Json;
using System.Text;

namespace Affirm8.Services
{
    /// <summary>
    /// Service for handling user authentication with the API
    /// </summary>
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl;

        private static string GetApiBaseUrl()
        {
#if ANDROID
            // Android emulator maps host machine's localhost to 10.0.2.2
            return "https://10.0.2.2:7001/api";
#else
            // Windows, iOS, MacCatalyst use localhost
            return "https://localhost:7001/api";
#endif
        }

        private User? _currentUser;
        public User? CurrentUser 
        { 
            get => _currentUser; 
            private set 
            { 
                _currentUser = value;
                CurrentUserChanged?.Invoke(value);
            } 
        }

        public bool IsAuthenticated => CurrentUser?.IsAuthenticated == true;

        public event Action<User?>? CurrentUserChanged;

        public AuthenticationService(IHttpClientFactory httpClientFactory)
        {
#if ANDROID
            _httpClient = httpClientFactory.CreateClient("default");
#else
            _httpClient = httpClientFactory.CreateClient();
#endif
            BaseUrl = GetApiBaseUrl();
            System.Diagnostics.Debug.WriteLine($"AuthenticationService: Using API URL: {BaseUrl}");
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> LoginAsync(string email, string password)
        {
            try
            {
                var loginModel = new LoginModel { Email = email, Password = password };
                var json = JsonSerializer.Serialize(loginModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var accessPass = JsonSerializer.Deserialize<AccessPassModel>(responseJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (accessPass != null)
                    {
                        // Set the token in the user object
                        accessPass.User.Token = accessPass.Token;
                        CurrentUser = accessPass.User;
                        System.Diagnostics.Debug.WriteLine($"LoginAsync: Successfully set CurrentUser = {CurrentUser.Email}, Token length = {accessPass.Token?.Length}");

                        // Set default auth header for future requests
                        _httpClient.DefaultRequestHeaders.Authorization = 
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessPass.Token);

                        return (true, string.Empty);
                    }
                }

                return (false, "Invalid email or password");
            }
            catch (Exception ex)
            {
                return (false, $"Login failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        public async Task<(bool Success, string ErrorMessage)> RegisterAsync(string email, string password, string nickName)
        {
            try
            {
                var registerModel = new RegisterModel 
                { 
                    Email = email, 
                    Password = password, 
                    NickName = nickName 
                };
                var json = JsonSerializer.Serialize(registerModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}/Auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return (true, string.Empty);
                }

                var errorJson = await response.Content.ReadAsStringAsync();
                return (false, $"Registration failed: {errorJson}");
            }
            catch (Exception ex)
            {
                return (false, $"Registration failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        /// <summary>
        /// Get current user ID (returns null if not authenticated)
        /// </summary>
        public Guid? GetCurrentUserId()
        {
            return CurrentUser?.Id;
        }
    }
} 