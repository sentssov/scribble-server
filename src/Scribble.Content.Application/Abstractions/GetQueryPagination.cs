namespace Scribble.Content.Application.Abstractions;

public class GetQueryPagination
{
    public const int DefaultPageIndex = 0;
    public const int DefaultPageSize = 20;

    public GetQueryPagination() { }

    public GetQueryPagination(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public int PageIndex { get; set; } = DefaultPageIndex;
    public int PageSize { get; set; } = DefaultPageSize;
}