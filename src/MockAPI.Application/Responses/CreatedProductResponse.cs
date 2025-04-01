using MockAPI.Domain.Base;

namespace MockAPI.Application.Responses;
public sealed class CreatedProductResponse : ProductBase
{
	public DateTimeOffset CreatedAt { get; set; }
}