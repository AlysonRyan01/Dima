using System.Security.Claims;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", HandleAsync)
            .WithName("Transaction: Create")
            .WithSummary("Cria uma nova transacao")
            .WithDescription("Cria uma nova transacao")
            .WithOrder(1)
            .Produces<Response<Transaction?>>();
    }
    
    private static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        ClaimsPrincipal user,
        CreateTransactionRequest request)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        
        var result = await handler.CreateAsync(request);

        return result.IsSuccess
            ? TypedResults.Created($"/{result.Data?.Id}", result)
            : Results.BadRequest();
    }
}