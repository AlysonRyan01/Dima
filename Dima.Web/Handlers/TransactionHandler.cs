using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class TransactionHandler(IHttpClientFactory clientFactory) : ITransactionHandler
{
    private readonly HttpClient _httpClient = clientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("v1/transactions", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
            ?? new Response<Transaction?>(null, 400, "Erro ao cadastrar a transacao");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var result = await _httpClient.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Erro ao atualizar a transacao");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var result = await _httpClient.DeleteAsync($"v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
               ?? new Response<Transaction?>(null, 400, "Erro ao remover a transacao");
    }

    public async Task<PagedResponse<List<Transaction>?>> GetAllAsync(GetAllTransactionRequest request)
    {
        return await _httpClient.GetFromJsonAsync<PagedResponse<List<Transaction>?>>($"v1/transactions")
               ?? new PagedResponse<List<Transaction>?>(null, 400, "Falha ao retornar as transacoes");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        return await _httpClient.GetFromJsonAsync<Response<Transaction?>>($"v1/categories/{request.Id}")
               ?? new Response<Transaction?>(null, 400, "Falha ao retornar a transacao");
    }
}