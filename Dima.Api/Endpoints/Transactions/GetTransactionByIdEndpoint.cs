using Dima.Core.Models;
using Dima.Core.Responses;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", HandleAsync)
            .WithName("Transaction: Get by id")
            .WithSummary("Retorna uma transacao")
            .WithDescription("Retorna uma transacao")
            .WithOrder(4)
            .Produces<Response<Transaction?>>();
    }

    public static async Task<IResult> HandleAsync(
        ITransactionHandler handler,
        [FromQuery] long id)
    {
        var request = new GetTransactionByIdRequest
        {
            UserId = "alyson@gmail.com",
            Id = id
        };

    var result = await handler.GetByIdAsync(request);
        
        return result.IsSuccess 
            ? Results.Ok(result) 
            : Results.NotFound(result);
    }
}