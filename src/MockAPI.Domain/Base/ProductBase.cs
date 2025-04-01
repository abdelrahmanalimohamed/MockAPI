using MockAPI.Domain.Entities;

namespace MockAPI.Domain.Base;
public abstract class ProductBase
{
	public string Id { get; set; }
	public string Name { get; set; }
	public ProductData? Data { get; set; }
}