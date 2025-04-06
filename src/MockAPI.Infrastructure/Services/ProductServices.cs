using Microsoft.Extensions.Options;
using MockAPI.Application.Common;
using MockAPI.Application.DTO;
using MockAPI.Application.Exceptions;
using MockAPI.Application.Interfaces;
using MockAPI.Application.Responses;
using MockAPI.Domain.Entities;
using MockAPI.Infrastructure.Settings;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MockAPI.Infrastructure.Services;
public class ProductServices : IProductRepository
{
	private readonly HttpClient _httpClient;
	private readonly string _baseUrl;
	public ProductServices
		(HttpClient httpClient, 
		IOptions<ExternalApiSettings> options)
	{
		_httpClient = httpClient;
		_baseUrl = options.Value.ProductApiBaseUrl;
	}
	public async Task<PaginatedResult<Product>> GetProductsAsync(
		string? search,
		int page,
		int pageSize ,
		CancellationToken cancellationToken)
	{

		var response = await GetHttpResponseAsync(cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			return GetEmptyPaginatedResult();
		}

		var products = await DeserializeProductsAsync(response, cancellationToken);
		products = FilterProductsBySearch(products, search);

		var paginatedItems = GetPaginatedItems(products, page, pageSize);
		var totalCount = products.Count;

		return new PaginatedResult<Product>
		{
			Items = paginatedItems,
			TotalCount = totalCount
		};
	}
	public async Task<CreatedProductResponse> AddProductAsync(CreateProductDto createProductDto , CancellationToken cancellationToken)
	{
		var jsonContent = JsonSerializer.Serialize(createProductDto);
		var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

		var response = await _httpClient.PostAsync(_baseUrl, content , cancellationToken);
		var responseData = await response.Content.ReadAsStringAsync(cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseData, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			throw new ApiException(response.StatusCode, errorResponse.Error);
		}

		var createdProductResponse = JsonSerializer.Deserialize<CreatedProductResponse>(responseData, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		if (createdProductResponse is null)
		{
			throw new ApiException(HttpStatusCode.InternalServerError, "The API response was null or invalid.");
		}

		return createdProductResponse;
	}
	public async Task<DeletedProductResponse> DeleteProductAsync(string id , CancellationToken cancellationToken)
	{
		var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}", cancellationToken);
		var responseData = await response.Content.ReadAsStringAsync(cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseData, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			throw new ApiException(response.StatusCode, errorResponse?.Error);
		}

		var deletedProductResponse = JsonSerializer.Deserialize<DeletedProductResponse>(responseData, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		if (deletedProductResponse == null)
		{
			throw new ApiException(HttpStatusCode.InternalServerError, "The API response was null or invalid.");
		}

		return deletedProductResponse;
	}
	public async Task<UpdatedProductResponse> UpdateProductAsync(string Id , UpdateProductDto updateProductDto, CancellationToken cancellationToken)
	{
		var jsonContent = JsonSerializer.Serialize(updateProductDto);
		var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

		var response = await _httpClient.PutAsync($"{_baseUrl}/{Id}", content, cancellationToken);
		var responseData = await response.Content.ReadAsStringAsync(cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseData, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

			throw new ApiException(response.StatusCode, errorResponse.Error);
		}

		var updatedProductResponse = JsonSerializer.Deserialize<UpdatedProductResponse>(responseData, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		});

		if (updatedProductResponse is null)
		{
			throw new ApiException(HttpStatusCode.InternalServerError, "The API response was null or invalid.");
		}

		return updatedProductResponse;
	}
	private async Task<HttpResponseMessage> GetHttpResponseAsync(CancellationToken cancellationToken)
	{
		return await _httpClient.GetAsync(_baseUrl, cancellationToken);
	}
	private PaginatedResult<Product> GetEmptyPaginatedResult()
	{
		return new PaginatedResult<Product> { Items = Enumerable.Empty<Product>(), TotalCount = 0 };
	}
	private async Task<List<Product>> DeserializeProductsAsync(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		var content = await response.Content.ReadAsStringAsync(cancellationToken);
		return JsonSerializer.Deserialize<List<Product>>(content, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		}) ?? new List<Product>();
	}
	private List<Product> FilterProductsBySearch(List<Product> products, string? search)
	{
		if (string.IsNullOrEmpty(search)) return products;
		return products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
	}
	private List<Product> GetPaginatedItems(List<Product> products, int page, int pageSize)
	{
		return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
	}
}