using Dima.Api.Endpoints;

namespace Dima.Api.Common;

public static class AppExtension
{
    public static void AddSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
    
    public static void AddDocumentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapSwagger().RequireAuthorization();
    }
    
    public static void AddMapEndpoints(this WebApplication app)
    {
        app.MapEndpoints();
    }
}