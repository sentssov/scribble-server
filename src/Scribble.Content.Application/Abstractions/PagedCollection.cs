using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Scribble.Content.Models;

namespace Scribble.Content.Application.Abstractions;

public class PagedCollection<T> : IPagedCollection<T>
{
    public PagedCollection(ICollection<T> items, int pageIndex, int pageSize, long totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (long)Math.Ceiling(TotalCount / (double)PageSize);
    }
    
    public ICollection<T> Items { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public long TotalPages { get; }
    public long TotalCount { get; }
    public bool HasNextPage => PageIndex * PageSize < TotalCount;
    public bool HasPreviousPage => PageSize > 1;
    public ICollection<Link> Links { get; set; } = new Collection<Link>();

    public static async Task<IPagedCollection<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize, 
        CancellationToken token)
    {
        var totalCount = await query.LongCountAsync(token)
            .ConfigureAwait(false);

        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize).ToListAsync(token)
            .ConfigureAwait(false);

        return new PagedCollection<T>(items, pageIndex, pageSize, totalCount);
    }
}