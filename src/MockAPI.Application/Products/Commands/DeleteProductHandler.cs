using MediatR;
using MockAPI.Application.Interfaces;
using MockAPI.Application.Responses;

namespace MockAPI.Application.Products.Commands;
internal sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeletedProductResponse>
{
	private readonly IProductRepository _productRepository;
	public DeleteProductHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}
	public async Task<DeletedProductResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
	{
		return await _productRepository.DeleteProductAsync(request.id, cancellationToken);
	}
}