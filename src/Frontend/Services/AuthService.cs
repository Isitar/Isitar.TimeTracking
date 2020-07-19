namespace Isitar.TimeTracking.Frontend.Services
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
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

        private Task<Result> RefreshTask = null;

        public AuthService(ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider, HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
        {
            this.localStorage = localStorage;
            this.authenticationStateProvider = authenticationStateProvider;
            this.httpClient = httpClient;
            this.jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<Result> LoginAsync(string username, string password)
        {
            var res = await httpClient.PostAsync("auth/login", new JsonContent(new
            {
                Username = username,
                Password = password
            }));
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

        public async Task LogoutAsync()
        {
            await localStorage.RemoveItemAsync(LocalStorageConstants.JwtTokenKey);
            await localStorage.RemoveItemAsync(LocalStorageConstants.RefreshTokenKey);
            await localStorage.ClearAsync();
            // todo: abstract? interfaces?
            var customAuthenticationState = authenticationStateProvider as CustomAuthenticationStateProvider;
            customAuthenticationState?.LoginChanged();
        }

        public async Task<Result> RefreshAsync()
        {
            if (null == RefreshTask)
            {
                RefreshTask = Task.Run(async () =>
                {
                    var jwtToken = await localStorage.GetItemAsync<string>(LocalStorageConstants.JwtTokenKey);
                    var refreshToken = await localStorage.GetItemAsync<string>(LocalStorageConstants.RefreshTokenKey);
                    var res = await httpClient.PostAsync("auth/refresh",
                        new JsonContent(new {RefreshToken = refreshToken, JwtToken = jwtToken}));
                    if (res.IsSuccessStatusCode)
                    {
                        var resAsString = await res.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<TokenDto>(resAsString, jsonSerializerOptions);
                        await localStorage.SetItemAsync(LocalStorageConstants.JwtTokenKey, result.Token);
                        await localStorage.SetItemAsync(LocalStorageConstants.RefreshTokenKey, result.RefreshToken);
                        return Result.Success();
                    }
                    else
                    {
                        await LogoutAsync();
                        return Result.Failure(new[] {"Error refreshing"});
                    }
                });
            }
            var result = await RefreshTask;
            RefreshTask = null;
            return result;

        }
    }
}