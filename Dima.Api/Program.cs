using Dima.Api;
using Dima.Api.Common;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfigurations();
builder.AddSecurity();
builder.AddDataContexts();
builder.AddServices();
builder.AddDocumentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.AddDocumentation();

app.UseCors(ApiConfiguration.CorsPolicyName);

app.AddSecurity();
app.AddMapEndpoints();

app.Run();