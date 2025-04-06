using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace MockAPI.Application.Exceptions;
public sealed class ExceptionHandling
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandling> _logger;
	public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
	{
		_next = next;
		_logger = logger;
	}
	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
			await HandleExceptionAsync(context, ex);
		}
	}
	private Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";

		HttpStatusCode status;
		object response;

		if (exception is ValidationExceptions validationException)
		{
			status = HttpStatusCode.BadRequest;
			response = new { error = "Validation error", errors = validationException.Errors };
			_logger.LogWarning("Validation error occurred: {Errors}", validationException.Errors);
		}

		else if (exception is ApiException apiException)
		{
			status = apiException.StatusCode;
			response = new { error = exception.Message };
			_logger.LogError(apiException, "Handled exception with status code {StatusCode}: {Message}", status, exception.Message);
		}

		else
		{
			status = HttpStatusCode.InternalServerError;
			response = new { error = exception.Message };
			_logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
		}

		context.Response.StatusCode = (int)status;
		var jsonResponse = JsonSerializer.Serialize(response);
		return context.Response.WriteAsync(jsonResponse);
	}
}