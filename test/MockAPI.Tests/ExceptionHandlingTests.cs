using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MockAPI.Application.Exceptions;
using Moq;
using System.Net;
using System.Text.Json;

namespace MockAPI.Tests;
public class ExceptionHandlingTests
{
	private readonly Mock<ILogger<ExceptionHandling>> _mockLogger;
	private readonly Mock<RequestDelegate> _mockNext;
	private readonly ExceptionHandling _middleware;

	public ExceptionHandlingTests()
	{
		_mockLogger = new Mock<ILogger<ExceptionHandling>>();
		_mockNext = new Mock<RequestDelegate>();
		_middleware = new ExceptionHandling(_mockNext.Object, _mockLogger.Object);
	}

	[Fact]
	public async Task Invoke_WhenNoException_ShouldCallNextDelegate()
	{
		// Arrange
		var context = new DefaultHttpContext();

		// Act
		await _middleware.Invoke(context);

		// Assert
		_mockNext.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
	}

	[Fact]
	public async Task Invoke_WhenExceptionThrown_ShouldReturnInternalServerError()
	{
		// Arrange
		var exception = new Exception("Something went wrong");
		_mockNext.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(exception);

		var context = new DefaultHttpContext();
		context.Response.Body = new MemoryStream();

		// Act
		await _middleware.Invoke(context);

		// Assert
		Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);

		context.Response.Body.Seek(0, SeekOrigin.Begin);
		var responseString = await new StreamReader(context.Response.Body).ReadToEndAsync();
		var response = JsonSerializer.Deserialize<Dictionary<string, string>>(responseString);

		Assert.NotNull(response);
		Assert.Equal(exception.Message, response["error"]);

		_mockLogger.Verify(
		l => l.Log(
			LogLevel.Error,
			It.IsAny<EventId>(),
			It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unhandled exception")),
			It.Is<Exception>(ex => ex.Message == "Something went wrong"),
			It.IsAny<Func<It.IsAnyType, Exception, string>>()),
		Times.AtLeastOnce);
	}

	[Fact]
	public async Task Invoke_WhenValidationExceptionThrown_ShouldReturnBadRequest()
	{
		// Arrange
		var errors = new Dictionary<string, string[]> { { "Field1", new[] { "Error1" } } };
		var validationException = new ValidationExceptions(errors);
		_mockNext.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(validationException);

		var context = new DefaultHttpContext();
		context.Response.Body = new MemoryStream();

		// Act
		await _middleware.Invoke(context);

		// Assert
		Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);

		context.Response.Body.Seek(0, SeekOrigin.Begin);
		var responseString = await new StreamReader(context.Response.Body).ReadToEndAsync();
		var response = JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);

		Assert.NotNull(response);
		Assert.Equal("Validation error", response["error"].ToString());
		Assert.NotNull(response["errors"]);

		_mockLogger.Verify(
				l => l.Log(
					LogLevel.Warning,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Validation error occurred")), 
					It.IsAny<Exception>(), 
					It.IsAny<Func<It.IsAnyType, Exception, string>>() 
				),
				Times.Once
			);
	}


	[Fact]
	public async Task Invoke_WhenApiExceptionThrown_ShouldReturnApiExceptionError()
	{
		// Arrange
		var apiException = new ApiException(HttpStatusCode.NotFound, "Not Found");
		_mockNext.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(apiException);

		var context = new DefaultHttpContext();
		context.Response.Body = new MemoryStream();

		// Act
		await _middleware.Invoke(context);

		// Assert
		Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);

		context.Response.Body.Seek(0, SeekOrigin.Begin);
		var responseString = await new StreamReader(context.Response.Body).ReadToEndAsync();
		var response = JsonSerializer.Deserialize<Dictionary<string, string>>(responseString);

		Assert.NotNull(response);
		Assert.Equal("Not Found", response["error"]);

		_mockLogger.Verify(
		l => l.Log(
			LogLevel.Error,
			It.IsAny<EventId>(),
			It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Handled exception with status code")),
			It.Is<Exception>(ex => ex is ApiException && ex.Message == "Not Found"),
			It.IsAny<Func<It.IsAnyType, Exception, string>>()
		),
		Times.Once);
	}
}