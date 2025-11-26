using Client.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri = "https://localhost:7212/api/Auth";

        public AuthService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUri);
            _httpClient.DefaultRequestHeaders.Add("Accept", "Application/json");
        }

        public async Task<bool> IsTokenValidAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            return string.IsNullOrEmpty(token);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                //отправка запроса
                var json = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/Login", content);
                var responceContent = response.Content.ReadAsStringAsync().Result;

                //обработка ответа
                if(response.IsSuccessStatusCode)
                {

                    var authResponse = JsonConvert.DeserializeObject<AuthResponse>(responceContent);
                    if(authResponse !=null && authResponse.Success)
                    {
                        await SecureStorage.SetAsync("auth_token", authResponse.Token);
                        await SecureStorage.SetAsync("refresh_token", authResponse.RefreshToken);
                    }

                    return authResponse ?? new AuthResponse
                    {
                        Success = false,
                        ErrorMessage = "Ошибка десериализации"
                    };
                }
                return new AuthResponse
                {
                    Success = false,
                    ErrorMessage = "ошибка " + response.StatusCode.ToString()
                };

            }
            catch(Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    ErrorMessage = "ошибка подключения " + ex.Message
                };
            }
        }

        public async Task<string> RefreshTokenAsync()
        {
            var refresh_token = await SecureStorage.GetAsync("refresh_token");
            if (string.IsNullOrEmpty(refresh_token))
                return string.Empty;

            try
            {
                var response = await _httpClient.PostAsync($"/Refresh?RefreshToken={refresh_token}", null);

                var reponceContent =  response.Content.ReadAsStringAsync().Result;
                var authResponse = JsonConvert.DeserializeObject<AuthResponse>(reponceContent);
                if (response.IsSuccessStatusCode)
                {
                    if (authResponse != null && authResponse.Success)
                    {
                        await SecureStorage.SetAsync("auth_token", authResponse.Token);
                        return authResponse.Token;
                    }
                    return string.Empty;
                }
                return string.Empty;
            }
            catch(Exception ex)
            {
                return string.Empty;
            }
        }

        public async Task<AuthResponse> RegisterAsync(CreateUserRequest createUser)
        {
            try
            {
                var json = JsonConvert.SerializeObject(createUser);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/Register", content);
                var responseContent = response.Content.ReadAsStringAsync().Result;

                if(response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<AuthResponse>(responseContent)
                        ??
                        new AuthResponse
                        {
                            Success = false,
                            ErrorMessage = "Ошибка десериализации"
                        };
                }

                return new AuthResponse
                {
                    Success = false,
                    ErrorMessage = "Ошибка " + response.StatusCode
                };
            }
            catch(Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    ErrorMessage = "Ошибка подключения " + ex.Message
                };
            }
        }
    }
}
