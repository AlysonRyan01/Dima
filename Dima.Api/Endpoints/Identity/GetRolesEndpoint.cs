using System.Security.Claims;
using Dima.Core.Models.Identity;

namespace Dima.Api.Endpoints.Identity;

public class GetRolesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("v1/roles", Handle)
            .RequireAuthorization();
    }

    private static Task<IResult> Handle(ClaimsPrincipal user)
    {
        if (user.Identity == null || !user.Identity.IsAuthenticated)
            return Task.FromResult<IResult>(Results.Unauthorized());
        
        var identity = (ClaimsIdentity)user.Identity;
        var roles = identity
            .FindAll(identity.RoleClaimType)
            .Select(x => new RoleClaim
            {
                Issuer = x.Issuer,
                OriginalIssuer = x.OriginalIssuer,
                Type = x.Type,
                Value = x.Value,
                ValueType = x.ValueType
            });
        
        return Task.FromResult<IResult>(TypedResults.Json(roles));
    }
}