using MediatR;
using MockAPI.Application.Common;
using MockAPI.Application.Interfaces;
using MockAPI.Domain.Entities;

namespace MockAPI.Application.Products.Queries;
internal sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, PaginatedResult<Product>>
{
	private readonly IProductRepository _productRepository;
	public GetProductsHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}
	public async Task<PaginatedResult<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
	{
		return await _productRepository.GetProductsAsync(request.Search, request.Page, request.PageSize , cancellationToken);
	}
}