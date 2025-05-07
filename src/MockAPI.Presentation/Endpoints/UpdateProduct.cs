using FastEndpoints;
using MediatR;
using MockAPI.Application.DTO;
using MockAPI.Application.Products.Commands;
using MockAPI.Application.Responses;

namespace MockAPI.Presentation.Endpoints
{
	public class UpdateProduct : Endpoint<UpdateProductRequest, UpdatedProductResponse>
	{
		private readonly IMediator _mediator;
		public UpdateProduct(IMediator mediator)
		{
			_mediator = mediator;
		}
		public override void Configure()
		{
			Put("/api/product/update/{id}");
			AllowAnonymous();
		}
		public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct)
		{
			var updatedProduct = await _mediator.Send(new UpdateProductCommand(req.Id, req.UpdateProductDto) , ct);
			await SendOkAsync(updatedProduct);
		}
	}
	public class UpdateProductRequest
	{
		[BindFrom("id")]
		public string Id { get; init; }
		[FromBody]
		public UpdateProductDto UpdateProductDto { get; init; }
	}
}
