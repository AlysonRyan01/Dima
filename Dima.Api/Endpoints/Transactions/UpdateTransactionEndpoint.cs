using System.Security.Claims;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions;

public class UpdateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id}", HandleAsync)
            .WithName("Transaction: Update")
            .WithSummary("Atualiza uma nova transacao")
            .WithDescription("Atualiza uma nova transacao")
            .WithOrder(2)
            .Produces<Response<Transaction?>>();
    }

    private static async Task<IResult> HandleAsync(
        UpdateTransactionRequest request,
        ClaimsPrincipal user,
        ITransactionHandler handler,
        [FromQuery] long id)

    {
        request.Id = id;
        request.UserId = user.Identity?.Name ?? string.Empty;

        var result = await handler.UpdateAsync(request);

        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}