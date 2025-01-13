using System.Net.Http.Json;
using System.Text;
using Dima.Core.Handlers;
using Dima.Core.Requests.Identity;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class IdentityHandler(IHttpClientFactory httpClientFactory) : IIdentityHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<string>> LoginAsync(LoginRequest request)
    {
       var result = await _httpClient.PostAsJsonAsync("v1/Identity/login?useCookies=true", request);
       return result.IsSuccessStatusCode 
           ? new Response<string>("Login realizado com sucesso!", 200, "Login realizado com sucesso!") 
           : new Response<string>("null", 400, "Usuario ou senha incorretos");
    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("v1/Identity/register", request);
        return result.IsSuccessStatusCode 
            ? new Response<string>("Cadastro realizado com sucesso!", 200, "Cadastro realizado com sucesso!") 
            : new Response<string>("null", 400, "Não foi possivel criar o registro");
    }

    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        await _httpClient.PostAsync("v1/Identity/logout", emptyContent);
    }
}