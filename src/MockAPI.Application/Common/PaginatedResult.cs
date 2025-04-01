namespace MockAPI.Application.Common;
public class PaginatedResult<T>
{
	public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
	public int TotalCount { get; set; }
}