using MediatR;
using MockAPI.Application.Interfaces;

namespace MockAPI.Application.Products.Commands;
public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand, string>
{
	private readonly IProductRepository _productRepository;
	public DeleteProductHandler(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}
	public async Task<string> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
	{
		return await _productRepository.DeleteProductAsync(request.id, cancellationToken);
	}
}
