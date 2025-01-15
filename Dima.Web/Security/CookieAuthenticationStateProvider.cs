using System.Net.Http.Json;
using System.Security.Claims;
using Dima.Core.Models.Identity;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.Web.Security;

public class CookieAuthenticationStateProvider(IHttpClientFactory clientFactory) : AuthenticationStateProvider, ICookieAuthenticationStateProvider
{
    private readonly HttpClient _httpClient = clientFactory.CreateClient(Configuration.HttpClientName);

    private bool IsAuthenticated = false;
    
    public async Task<bool> CheckAuthenticatedAsync()
    {
        await GetAuthenticationStateAsync();
        return IsAuthenticated;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        IsAuthenticated = false;
        var user = new ClaimsPrincipal(new ClaimsIdentity());

        var userInfo = await GetUserAsync();
        if (userInfo == null)
            return new AuthenticationState(user);
        
        var claims = await GetClaimsAsync(userInfo);
        
        var identity = new ClaimsIdentity(claims, nameof(CookieAuthenticationStateProvider));
        user = new ClaimsPrincipal(identity);
        
        IsAuthenticated = true;
        return new AuthenticationState(user);
    }
    

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    
    private async Task<User?> GetUserAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<User>("v1/identity/manage/info");
        }
        catch
        {
            return null;
        }
    }

    private async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email),
            new(ClaimTypes.Email, user.Email)
        };

        claims.AddRange(
            user.Claims.Where(x => x.Key != ClaimTypes.Name && x.Key != ClaimTypes.Email)
                .Select(x => new Claim(x.Key, x.Value))
        );

        RoleClaim[]? roles;
        try
        {
            roles = await _httpClient.GetFromJsonAsync<RoleClaim[]>("v1/identity/roles");
        }
        catch
        {
            return claims;
        }

        foreach (var role in roles ?? [])
        {
            if (!string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value))
                claims.Add(new Claim(role.Type, role.Value, role.ValueType, role.Issuer, role.OriginalIssuer));
        }

        return claims;
    }
}