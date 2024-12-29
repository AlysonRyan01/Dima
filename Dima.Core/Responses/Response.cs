using System.Text.Json.Serialization;

namespace Dima.Core.Responses;

public class Response <T>
{
    public Response()
    {
        _code = Configuration.DefaultStatusCode;
    }
    
    [JsonConstructor]
    public Response(T? data, int code = Configuration.DefaultStatusCode, string? message = null)
    {
        Data = data;
        Message = message;
        _code = code;
    }

    private readonly int _code;
    
    public T? Data { get; set; }
    public string? Message { get; set; }
    
    [JsonIgnore]
    public bool IsSuccess 
        => _code >= 200 && _code <= 299;
}