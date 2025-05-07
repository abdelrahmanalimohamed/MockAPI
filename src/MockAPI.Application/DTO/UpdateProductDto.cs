using MockAPI.Domain.Entities;

namespace MockAPI.Application.DTO;
public record UpdateProductDto(string name, ProductData data) : ProductDTO(name, data);