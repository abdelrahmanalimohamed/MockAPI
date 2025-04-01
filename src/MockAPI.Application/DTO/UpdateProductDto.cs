using MockAPI.Domain.Entities;

namespace MockAPI.Application.DTO;
public record UpdateProductDto(string id, string name, ProductData Data) : ProductDTO(id, name, Data)
{
}