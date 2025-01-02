using Dima.Core.Models;
using Dima.Core.Responses;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", HandleAsync)
            .WithName("Transaction: Delete")
            .WithSummary("Remove uma transacao")
            .WithDescription("Remove uma transacao")
            .WithOrder(3)
            .Produces<Response<Transaction?>>();
    }

    private static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        [FromQuery]long id)
    {
        var request = new DeleteTransactionRequest
        {
            Id = id,
            UserId = "alyson@gmail.com"
        };
        
        var result = await handler.DeleteAsync(request);
        
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}