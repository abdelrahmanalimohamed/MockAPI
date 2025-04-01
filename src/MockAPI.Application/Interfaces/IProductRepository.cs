using MockAPI.Application.Common;
using MockAPI.Application.DTO;
using MockAPI.Application.Responses;
using MockAPI.Domain.Entities;

namespace MockAPI.Application.Interfaces;
public interface IProductRepository
{
	Task<PaginatedResult<Product>> GetProductsAsync(string? search, int page, int pageSize , CancellationToken cancellationToken);
	Task<CreatedProductResponse> AddProductAsync(CreateProductDto createProductDto , CancellationToken cancellationToken);
	Task<string?> DeleteProductAsync(string id , CancellationToken cancellationToken);
	Task<UpdatedProductResponse> UpdateProductAsync(string Id, UpdateProductDto updateProductDto, CancellationToken cancellationToken);
}