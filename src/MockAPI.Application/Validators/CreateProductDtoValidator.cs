using FluentValidation;
using MockAPI.Application.DTO;

namespace MockAPI.Application.Validators;
public sealed class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
	public CreateProductDtoValidator()
	{
		RuleFor(x => x.name).ValidateProductName();
	}
}