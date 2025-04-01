using MediatR;
using MockAPI.Application.Interfaces;
using MockAPI.Application.Responses;

namespace MockAPI.Application.Products.Commands;
internal sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, CreatedProductResponse>
{
	private readonly IProductRepository _productRepository;
	public CreateProductHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}
	public async Task<CreatedProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
	{
		return await _productRepository.AddProductAsync(request.CreateProductDto, cancellationToken);
	}
}