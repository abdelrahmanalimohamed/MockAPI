using Microsoft.Extensions.Options;
using MockAPI.Application.DTO;
using MockAPI.Application.Exceptions;
using MockAPI.Application.Responses;
using MockAPI.Domain.Entities;
using MockAPI.Infrastructure.Services;
using MockAPI.Infrastructure.Settings;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MockAPI.Tests;
public class ProductServicesTests
{
	private readonly Mock<HttpMessageHandler> _handlerMock;
	private readonly HttpClient _httpClient;
	private readonly ProductServices _productServices;
	private readonly string _baseUrl = "https://api.restful-api.dev/objects";
	public ProductServicesTests()
	{
		_handlerMock = new Mock<HttpMessageHandler>();
		_httpClient = new HttpClient(_handlerMock.Object);
		var options = Options.Create(new ExternalApiSettings { ProductApiBaseUrl = _baseUrl });
		_productServices = new ProductServices(_httpClient, options);
	}

	[Fact]
	public async Task GetProductsAsync_ReturnsPaginatedResult_WhenApiReturnsSuccess()
	{
		var products = new List<Product>
				{
					new Product { Id = "1", Name = "Product A" },
					new Product { Id = "2", Name = "Product B" }
				};

		var jsonResponse = JsonSerializer.Serialize(products);
		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
		};

		_handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(responseMessage);

		var result = await _productServices.GetProductsAsync(null, 1, 2, CancellationToken.None);

		Assert.NotNull(result);
		Assert.Equal(2, result.TotalCount);
		Assert.Equal(2, result.Items.Count());
	}

	[Fact]
	public async Task AddProductAsync_ReturnsCreatedProductResponse_WhenApiReturnsSuccess()
	{
		var createProductDto = new CreateProductDto("123", "New Product", new ProductData());
		var createdResponse = new CreatedProductResponse { Id = "123", Name = "New Product" };

		var jsonResponse = JsonSerializer.Serialize(createdResponse);
		var responseMessage = new HttpResponseMessage(HttpStatusCode.Created)
		{
			Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
		};

		_handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(responseMessage);

		var result = await _productServices.AddProductAsync(createProductDto, CancellationToken.None);

		Assert.NotNull(result);
		Assert.Equal("123", result.Id);
		Assert.Equal("New Product", result.Name);
	}

	[Fact]
	public async Task AddProductAsync_ThrowsApiException_WhenApiReturnsError()
	{
		var createProductDto = new CreateProductDto("123", "New Product", new ProductData());
		var errorResponse = new { Error = "Bad Request" };
		var jsonResponse = JsonSerializer.Serialize(errorResponse);
		var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
		{
			Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
		};

		_handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(responseMessage);

		var exception = await Assert.ThrowsAsync<ApiException>(() => _productServices.AddProductAsync(createProductDto, CancellationToken.None));

		Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
		Assert.Equal("Bad Request", exception.Message);
	}

	[Fact]
	public async Task DeleteProductAsync_ReturnsMessage_WhenApiReturnsSuccess()
	{
		var successResponse = new { Message = "Product deleted successfully" };
		var jsonResponse = JsonSerializer.Serialize(successResponse);
		var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
		};

		_handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(responseMessage);

		var result = await _productServices.DeleteProductAsync("123", CancellationToken.None);

		Assert.Equal("Product deleted successfully", result);
	}

	[Fact]
	public async Task DeleteProductAsync_ReturnsErrorMessage_WhenApiReturnsError()
	{
		var errorResponse = new { Error = "Product not found" };
		var jsonResponse = JsonSerializer.Serialize(errorResponse);
		var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
		{
			Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
		};

		_handlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(responseMessage);

		var result = await _productServices.DeleteProductAsync("999", CancellationToken.None);

		Assert.Equal("Product not found", result);
	}

	[Fact]
	public async Task UpdateProductAsync_WhenApiReturnsSuccess_ShouldReturnUpdatedProductResponse()
	{
		// Arrange
		var productId = "123";
		var updateProductDto = new UpdateProductDto( productId , "Updated Product" , new ProductData());
		var expectedResponse = new UpdatedProductResponse { Id = "123", Name = "Updated Product" };

		var jsonResponse = JsonSerializer.Serialize(expectedResponse);
		var httpResponse = new HttpResponseMessage
		{
			StatusCode = HttpStatusCode.OK,
			Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
		};

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Put &&
					req.RequestUri.ToString().EndsWith($"/{productId}")
				),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(httpResponse);

		// Act
		var result = await _productServices.UpdateProductAsync(productId, updateProductDto, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(expectedResponse.Id, result.Id);
		Assert.Equal(expectedResponse.Name, result.Name);
	}

	[Fact]
	public async Task UpdateProductAsync_WhenApiReturnsError_ShouldThrowApiException()
	{
		// Arrange
		var productId = "123"; 
		var updateProductDto = new UpdateProductDto(productId, "Updated Product", new ProductData());
		var errorResponse = new ErrorResponse { Error = "Invalid request" };

		var jsonResponse = JsonSerializer.Serialize(errorResponse);
		var httpResponse = new HttpResponseMessage
		{
			StatusCode = HttpStatusCode.BadRequest,
			Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
		};

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Put &&
					req.RequestUri.ToString().EndsWith($"/{productId}")
				),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(httpResponse);

		// Act & Assert
		var exception = await Assert.ThrowsAsync<ApiException>(
			() => _productServices.UpdateProductAsync(productId, updateProductDto, CancellationToken.None));

		Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
		Assert.Equal("Invalid request", exception.Message);
	}

	[Fact]
	public async Task UpdateProductAsync_WhenApiResponseIsNull_ShouldThrowApiException()
	{
		// Arrange
		var productId = "123";
		var updateProductDto = new UpdateProductDto(productId, "Updated Product", new ProductData());

		var httpResponse = new HttpResponseMessage
		{
			StatusCode = HttpStatusCode.OK,
			Content = new StringContent("null", Encoding.UTF8, "application/json") // Simulating null response
		};

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Put &&
					req.RequestUri.ToString().EndsWith($"/{productId}")
				),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(httpResponse);

		// Act & Assert
		var exception = await Assert.ThrowsAsync<ApiException>(
			() => _productServices.UpdateProductAsync(productId, updateProductDto, CancellationToken.None));

		Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
		Assert.Equal("The API response was null or invalid.", exception.Message);
	}
}
