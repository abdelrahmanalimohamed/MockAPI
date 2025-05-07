using FastEndpoints;
using MediatR;
using MockAPI.Application.DTO;
using MockAPI.Application.Products.Commands;
using MockAPI.Application.Responses;

namespace MockAPI.Presentation.Endpoints;

public class CreateProductEndPoint : Endpoint<CreateProductRequest, CreatedProductResponse>
{
	private readonly IMediator _mediator;
	public CreateProductEndPoint(IMediator mediator)
	{
		_mediator = mediator;
	}
	public override void Configure()
	{
		Post("/api/product/create");
		AllowAnonymous();
	}
	public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
	{
		var createdProduct = await _mediator.Send(new CreateProductCommand(req.CreateProductDto) , ct);
		await SendOkAsync(createdProduct);
	}
}

public class CreateProductRequest
{
	[FromBody]
	public CreateProductDto CreateProductDto { get; set; }
}