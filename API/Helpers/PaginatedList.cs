using Microsoft.EntityFrameworkCore;

namespace API.Helpers;

public class PaginatedList<T>
{
    public int Count { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public List<T> Items { get; set; }

    public PaginatedList(List<T> items, int count, int currentPage, int pageSize)
    {
        Items = items;
        Count = count;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = (int) Math.Ceiling(count / (double) pageSize);
    }

    public static async Task<PaginatedList<T>> CreatePaginatedListAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}