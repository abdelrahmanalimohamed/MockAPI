using MockAPI.Domain.Entities;

namespace MockAPI.Application.DTO;
public record CreateProductDto(string name , ProductData data) : ProductDTO(name , data);