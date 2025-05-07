using FastEndpoints;
using MediatR;
using MockAPI.Application.Products.Commands;
using MockAPI.Application.Responses;

namespace MockAPI.Presentation.Endpoints;

public class DeleteProductEndpoint : Endpoint<DeleteProductRequest , DeletedProductResponse>
{
	private readonly IMediator _mediator;
	public DeleteProductEndpoint(IMediator mediator)
	{
		_mediator = mediator;
	}
	public override void Configure()
	{
		Delete("/api/product/delete/{id}");
		AllowAnonymous();
	}
	public override async Task HandleAsync(DeleteProductRequest req, CancellationToken ct)
	{
		var deletedProduct = await _mediator.Send(new DeleteProductCommand(req.Id));
		await SendOkAsync(deletedProduct);
	}
}
public class DeleteProductRequest
{
	[BindFrom("id")]
	public string Id { get; init; }
}