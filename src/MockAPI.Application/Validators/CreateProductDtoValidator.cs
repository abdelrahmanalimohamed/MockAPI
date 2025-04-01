using FluentValidation;
using MockAPI.Application.DTO;

namespace MockAPI.Application.Validators;
public sealed class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
	public CreateProductDtoValidator()
	{
		RuleFor(x => x.id)
			   .NotEmpty().WithMessage("Product ID is required.");

		RuleFor(x => x.name)
			.NotEmpty().WithMessage("Product name is required.");

		RuleFor(x => x.Data)
		 .NotEmpty().WithMessage("Product data is required.")
		 .When(x => x.Data != null);
	}
}