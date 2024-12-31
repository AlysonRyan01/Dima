using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest createCategoryRequest)
    {
        try
        {
            var category = new Category
            {
                UserId = createCategoryRequest.UserId,
                Title = createCategoryRequest.Title,
                Description = createCategoryRequest.Description
            };
            
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            
            return new Response<Category?>(category, 201, "Categoria criada com sucesso!");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Falha ao criar uma categoria.");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest updateCategoryRequest)
    {
        try
        {
            var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == updateCategoryRequest.Id && x.UserId == updateCategoryRequest.UserId);
            
            if (category == null)
                return new Response<Category?>(null, 404, "Categoria nao encontrada");
            
            category.Title = updateCategoryRequest.Title;
            category.Description = updateCategoryRequest.Description;
            
            context.Categories.Update(category);
            await context.SaveChangesAsync();
            
            return new Response<Category?>(category, 201, "Categoria atualizada com sucesso!");
        }
        
        catch
        {
            return new Response<Category?>(null, 500, "Falha ao atualizar a categoria.");
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest deleteCategoryRequest)
    {
        try
        {
            var category = await context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == deleteCategoryRequest.Id && x.UserId == deleteCategoryRequest.UserId);
            
            if (category == null)
                return new Response<Category?>(null, 404, "Categoria nao encontrada");
            
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            
            return new Response<Category?>(category, 201, "Categoria removida com sucesso!");
        }
        catch
        {
            return new Response<Category?>(null, 500, "Falha ao remover a categoria!");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest getCategoryByIdRequest)
    {
        try
        {
            var category = await context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == getCategoryByIdRequest.Id && x.UserId == getCategoryByIdRequest.UserId);
            
            if (category == null)
                return new Response<Category?>(null, 404, "Categoria nao encontrada");
            
            return new Response<Category?>(category);
        }
        catch
        {
            return new Response<Category?>(null, 500, "Falha ao carregar a categoria!");

        }
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest getAllCategoriesRequest)
    {
        try
        {
            var query = context
                .Categories
                .AsNoTracking()
                .Where(x => x.UserId == getAllCategoriesRequest.UserId)
                .OrderBy(x => x.Title);
            
            var categories = await query.Skip((getAllCategoriesRequest.PageNumber - 1) * getAllCategoriesRequest.PageSize)
                .Take(getAllCategoriesRequest.PageSize)
                .ToListAsync();
            
            var count = await query.CountAsync();
            
            return new PagedResponse<List<Category>>(categories, count, getAllCategoriesRequest.PageNumber, getAllCategoriesRequest.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Category>>(null, 500, "Falha ao obter as categorias!");
        }
    }
}