using MediatR;
using Microsoft.AspNetCore.Mvc;
using MockAPI.Application.DTO;
using MockAPI.Application.Products.Commands;
using MockAPI.Application.Products.Queries;

namespace MockAPI.Presentation.Controller;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
	private readonly IMediator _mediator;
	public ProductsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("GetAllProducts")]
	public async Task<IActionResult> GetProducts([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
	{
		var products = await _mediator.Send(new GetProductsQuery(search, page, pageSize));
		return Ok(products);
	}

	[HttpPost("CreateNewProduct")]
	public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
	{
		var createdProduct = await _mediator.Send(new CreateProductCommand(createProductDto));
		return Ok(createdProduct);
	}

	[HttpDelete("DeleteProduct/{Id}")]
	public async Task<IActionResult> DeleteProduct([FromRoute] string Id)
	{
		var deletedProduct = await _mediator.Send(new DeleteProductCommand(Id));
		return Ok(deletedProduct);
	}

	[HttpPut("UpdateProduct/{id}")]
	public async Task<IActionResult> UpdateProduct([FromRoute] string id , [FromBody] UpdateProductDto updateProductDto)
	{
		var updatedProduct = await _mediator.Send(new UpdateProductCommand(id, updateProductDto));
		return Ok(updatedProduct);
	}
}