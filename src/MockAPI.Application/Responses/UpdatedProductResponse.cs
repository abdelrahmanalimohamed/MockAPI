using MockAPI.Domain.Base;

namespace MockAPI.Application.Responses;
public class UpdatedProductResponse : ProductBase
{
	public DateTimeOffset UpdatedAt { get; set; }
}