using FastEndpoints;
using MediatR;
using MockAPI.Application.Common;
using MockAPI.Application.Products.Queries;
using MockAPI.Domain.Entities;
namespace MockAPI.Presentation.Endpoints;

public class GetAllProductsEndpoint : Endpoint<GetProductsRequest, GetProductsResponse>
{
	private readonly IMediator _mediator;
	public GetAllProductsEndpoint(IMediator mediator)
	{
		_mediator = mediator;
	}
	public override void Configure()	
	{
		Get("/api/products/getAll");
		AllowAnonymous();
	}
	public override async Task HandleAsync(GetProductsRequest req , CancellationToken ct)
	{
		var products = await _mediator.Send(new GetProductsQuery(req.search, req.page, req.pageSize) , ct);
		if (products is null)
		{
			await SendNotFoundAsync(ct);
		}
		else
		{
			await SendOkAsync(new GetProductsResponse { PaginatedResult = products });
		}
	}
}

public class GetProductsRequest
{
	public string? search { get; init; }
	public int page { get; init; } = 1;
	public int pageSize { get; init; } = 10;
}
public class GetProductsResponse
{
	public PaginatedResult<Product>? PaginatedResult { get; set; }
}
