namespace Scribble.Content.Application.Abstractions;

public interface IPagedCollection<T>
{
    ICollection<T> Items { get; }
    int PageIndex { get; }
    int PageSize { get; }
    long TotalPages { get; }
    long TotalCount { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
    public ICollection<Link> Links { get; set; }
}