using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(connectionString);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName);
});

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/v1/categories", async (CreateCategoryRequest request,
            ICategoryHandler handler) => await handler.CreateAsync(request))
    .WithName("Categories: Create")
    .WithSummary("Cria uma nova categoria")
    .Produces<Response<Category>>();

app.MapPut("/v1/categories/{id}", async (long id,
        UpdateCategoryRequest request,
        ICategoryHandler handler) =>
    {
        request.Id = id;
        return await handler.UpdateAsync(request);
    })
    .WithName("Categories: Update")
    .WithSummary("Edita uma categoria")
    .Produces<Response<Category>>();

app.MapDelete("/v1/categories/{id}",
        async (long id,
            ICategoryHandler handler) =>
        {
            var request = new DeleteCategoryRequest { Id = id, UserId = "alyson@gmail.com"};
            return await handler.DeleteAsync(request);
        })
    .WithName("Categories: Delete")
    .WithSummary("Deleta uma categoria")
    .Produces<Response<Category>>();

app.MapGet("/v1/categories/{id}",
        async (long id,
            ICategoryHandler handler) =>
        {
            var request = new GetCategoryByIdRequest { Id = id, UserId = "alyson@gmail.com"};
            return await handler.GetByIdAsync(request);
        })
    .WithName("Categories: Get by Id")
    .WithSummary("Mostra uma categoria")
    .Produces<Response<Category>>();

app.MapGet("/v1/categories/",
        async (ICategoryHandler handler) =>
        {
            var request = new GetAllCategoriesRequest{UserId = "alyson@gmail.com"};
            return await handler.GetAllAsync(request);
        })
    .WithName("Categories: Get all")
    .WithSummary("Mostra todas as categorias")
    .Produces<Response<Category>>();

app.Run();