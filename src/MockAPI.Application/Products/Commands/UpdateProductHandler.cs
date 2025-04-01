using MediatR;
using MockAPI.Application.Interfaces;
using MockAPI.Application.Responses;

namespace MockAPI.Application.Products.Commands;
internal sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdatedProductResponse>
{
	private readonly IProductRepository _productRepository;
	public UpdateProductHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}
	public async Task<UpdatedProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
	{
		return await _productRepository.UpdateProductAsync(request.Id, request.UpdateProductDto, cancellationToken);
	}
}