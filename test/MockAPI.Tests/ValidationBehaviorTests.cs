using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MockAPI.Application.Behaviors;
using MockAPI.Application.DTO;
using MockAPI.Application.Exceptions;
using MockAPI.Application.Products.Commands;
using MockAPI.Application.Responses;
using MockAPI.Domain.Entities;
using Moq;

namespace MockAPI.Tests;
public class ValidationBehaviorTests
{
	private readonly Mock<IValidator<CreateProductCommand>> _mockValidator;
	private readonly Mock<RequestHandlerDelegate<CreatedProductResponse>> _mockNext;
	private readonly IServiceProvider _serviceProvider;
	public ValidationBehaviorTests()
	{
		_mockValidator = new Mock<IValidator<CreateProductCommand>>();
		_mockNext = new Mock<RequestHandlerDelegate<CreatedProductResponse>>();

		var serviceCollection = new ServiceCollection();
		_serviceProvider = serviceCollection.BuildServiceProvider();
	}

	[Fact]
	public async Task Handle_WithValidRequest_ShouldCallNext()
	{
		// Arrange
		var validators = new List<IValidator<CreateProductCommand>> { _mockValidator.Object };
		var behavior = new ValidationBehavior<CreateProductCommand, CreatedProductResponse>(validators, _serviceProvider);
		var request = new CreateProductCommand(new CreateProductDto("TestProduct", new ProductData()));

		_mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ValidationResult());

		_mockNext.Setup(n => n()).ReturnsAsync(new CreatedProductResponse());

		// Act
		var response = await behavior.Handle(request, _mockNext.Object, CancellationToken.None);

		// Assert
		_mockNext.Verify(n => n(), Times.Once);
	}

	[Fact]
	public async Task Handle_WithInvalidRequest_ShouldThrowValidationException()
	{
		// Arrange
		var validators = new List<IValidator<CreateProductCommand>> { _mockValidator.Object };
		var behavior = new ValidationBehavior<CreateProductCommand, CreatedProductResponse>(validators, _serviceProvider);
		var request = new CreateProductCommand(new CreateProductDto("TestProduct", new ProductData()));

		var failures = new List<ValidationFailure>
		{
			new ValidationFailure("Property", "Error message")
		};

		_mockValidator.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
			.ReturnsAsync(new ValidationResult(failures));

		// Act & Assert
		await Assert.ThrowsAsync<ValidationExceptions>(() => behavior.Handle(request, _mockNext.Object, CancellationToken.None));
	}
}