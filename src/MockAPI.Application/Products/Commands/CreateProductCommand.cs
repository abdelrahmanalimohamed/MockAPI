using MediatR;
using MockAPI.Application.DTO;
using MockAPI.Application.Responses;

namespace MockAPI.Application.Products.Commands;
public sealed record CreateProductCommand(CreateProductDto CreateProductDto) 
	: IRequest<CreatedProductResponse>
{
}