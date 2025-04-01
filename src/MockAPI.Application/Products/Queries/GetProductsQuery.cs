using MediatR;
using MockAPI.Application.Common;
using MockAPI.Domain.Entities;

namespace MockAPI.Application.Products.Queries;
public sealed record GetProductsQuery(string? Search , int Page , int PageSize) 
	: IRequest<PaginatedResult<Product>>
{
}