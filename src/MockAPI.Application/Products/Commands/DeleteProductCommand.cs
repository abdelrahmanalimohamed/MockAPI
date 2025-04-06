using MediatR;
using MockAPI.Application.Responses;
namespace MockAPI.Application.Products.Commands;
public sealed record DeleteProductCommand(string id) 
	: IRequest<DeletedProductResponse>
{
}