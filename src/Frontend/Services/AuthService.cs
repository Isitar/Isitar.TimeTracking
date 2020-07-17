namespace Isitar.TimeTracking.Frontend.Services
{
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Application.Common.Entities;
    using Blazored.LocalStorage;
    using Common;
    using Data;
    using global::Common.Resources;
    using Infrastructure.Identity.Services.TokenService;
    using Microsoft.AspNetCore.Components.Authorization;

    public class AuthService : IAuthService
    {
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions;


        public AuthService(ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
        {
            this.localStorage = localStorage;
            this.authenticationStateProvider = authenticationStateProvider;
            this.httpClient = httpClient;
            this.jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<Result> LoginAsync(string username, string password)
        {
            var res = await httpClient.PostAsync("login", new StringContent(JsonSerializer.Serialize(new
            {
                Username = username,
                Password = password
            }), Encoding.UTF8, "application/json"));
            if (res.IsSuccessStatusCode)
            {
                var resAsString = await res.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TokenDto>(resAsString, jsonSerializerOptions);
                await localStorage.SetItemAsync(LocalStorageConstants.JwtTokenKey, result.Token);
                await localStorage.SetItemAsync(LocalStorageConstants.RefreshTokenKey, result.RefreshToken);
                // todo: abstract? interfaces?
                var customAuthenticationState = authenticationStateProvider as CustomAuthenticationStateProvider;
                customAuthenticationState?.LoginChanged();
                return Result.Success();
            }
            else
            {
                var errors = await res.Content.ReadAsStringAsync();
                try
                {
                    var parsedErrors = JsonSerializer.Deserialize<string[]>(errors, jsonSerializerOptions);
                    return Result.Failure(parsedErrors);
                }
                catch
                {
                    return Result.Failure(new[] {Translation.UnspecifiedError});
                }
            }
        }

        public async Task LogoutAsync()
        {
            await localStorage.RemoveItemAsync(LocalStorageConstants.JwtTokenKey);
            await localStorage.RemoveItemAsync(LocalStorageConstants.RefreshTokenKey);
            await localStorage.ClearAsync();
            // todo: abstract? interfaces?
            var customAuthenticationState = authenticationStateProvider as CustomAuthenticationStateProvider;
            customAuthenticationState?.LoginChanged();
        }
    }
}