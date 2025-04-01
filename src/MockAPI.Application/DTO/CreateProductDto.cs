using MockAPI.Domain.Entities;

namespace MockAPI.Application.DTO;
public record CreateProductDto(string id , string name , ProductData Data) : ProductDTO(id , name , Data);