var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Endpoints -> Url para acesso
app.MapGet("/", () => "Hello World!");
app.MapPost("/v1/transaction", (Request request, Handler handler) => handler.Handle(request))
    .WithName("Transactions: Create")
    .WithSummary("Cria uma nova transacao")
    .Produces<Response>();


app.MapPut("/", () => "Hello World!");
app.MapDelete("/", () => "Hello World!");

app.Run();

public class Request
{
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int Type { get; set; }
    public decimal Amount { get; set; }
    public long CategoryId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class Response
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

public class Handler()
{
    public Response Handle(Request request)
    {
        return new Response
        {
            Id = 4,
            Title = request.Title,
        };
    }

}