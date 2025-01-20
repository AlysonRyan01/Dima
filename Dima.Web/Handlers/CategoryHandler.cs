using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Web.Handlers;

public class CategoryHandler(IHttpClientFactory clientFactory) : ICategoryHandler
{
    private readonly HttpClient _httpClient = clientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest createCategoryRequest)
    {
        var result = await _httpClient.PostAsJsonAsync("v1/categories", createCategoryRequest);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
            ?? new Response<Category?>(null, 400, "Falha ao criar a categoria");
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest updateCategoryRequest)
    {
        var result = await _httpClient.PutAsJsonAsync($"v1/categories/{updateCategoryRequest.Id}", updateCategoryRequest);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao atualizar a categoria");
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest deleteCategoryRequest)
    {
        var result = await _httpClient.DeleteAsync($"v1/categories/{deleteCategoryRequest.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
               ?? new Response<Category?>(null, 400, "Falha ao excluir a categoria");
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest getCategoryByIdRequest)
    {
        return await _httpClient.GetFromJsonAsync<Response<Category?>>($"v1/categories/{getCategoryByIdRequest.Id}")
            ?? new Response<Category?>(null, 400, "Falha ao retornar a categoria");
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest getAllCategoriesRequest)
    {
        return await _httpClient.GetFromJsonAsync<PagedResponse<List<Category>>>($"v1/categories")
            ?? new PagedResponse<List<Category>>(null, 400, "Falha ao retornar as categorias");
    }
}