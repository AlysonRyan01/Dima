using System.Text.Json.Serialization;

namespace Dima.Core.Responses;

public class PagedResponse<T> : Response<T>
{
    [JsonConstructor]
    public PagedResponse(T data, int totalCount, int pageSize = Configuration.DefaultPageSize, int currentPage = 1) : base(data)
    {
        Data = data;
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
    }

    public PagedResponse(T data, int code = Configuration.DefaultStatusCode, string? message = null) : base(data, code, message)
    {
        
    }
    
    public int CurrentPage { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int PageSize { get; set; } = Configuration.DefaultPageSize;
    public int TotalCount { get; set; }
}