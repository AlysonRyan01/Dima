using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.Web.Security;

public class CookieAuthenticationStateProvider(IHttpClientFactory clientFactory) : AuthenticationStateProvider, ICookieAuthenticationStateProvider
{
    private readonly HttpClient _httpClient = clientFactory.CreateClient(Configuration.HttpClientName);
    
    public Task<bool> CheckAuthenticatedAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }

    public void NotifyAuthenticationStateChanged()
    {
        throw new NotImplementedException();
    }
}