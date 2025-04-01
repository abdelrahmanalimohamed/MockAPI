using MockAPI.Domain.Entities;

namespace MockAPI.Application.DTO;
public abstract record ProductDTO(string id, string name, ProductData Data)
{
}