using System.Security.Claims;

namespace Dima.Api.Endpoints.Identity;

public class GetRolesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGroup("v1/identity")
            .WithTags("Identity")
            .MapGet("/roles", Handle)
            .RequireAuthorization();
    }

    private static Task<IResult> Handle(ClaimsPrincipal user)
    {
        if (user.Identity == null || !user.Identity.IsAuthenticated)
            return Task.FromResult<IResult>(Results.Unauthorized());
        
        var identity = (ClaimsIdentity)user.Identity;
        var roles = identity
            .FindAll(identity.RoleClaimType)
            .Select(c => new
            {
                c.Issuer,
                c.OriginalIssuer,
                c.Type,
                c.Value,
                c.ValueType
            });
        
        return Task.FromResult<IResult>(TypedResults.Json(roles));
    }
}