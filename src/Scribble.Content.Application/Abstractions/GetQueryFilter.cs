namespace Scribble.Content.Application.Abstractions;

public class GetQueryFilter
{
    public string? SearchTerm { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; } = SortOrderConstants.Ascending;
}

public static class SortOrderConstants
{
    public const string Descending = "desc";
    public const string Ascending = "asc";
}