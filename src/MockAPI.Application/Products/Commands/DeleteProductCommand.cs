using MediatR;
namespace MockAPI.Application.Products.Commands;
public sealed record DeleteProductCommand(string id) 
	: IRequest<string>
{
}