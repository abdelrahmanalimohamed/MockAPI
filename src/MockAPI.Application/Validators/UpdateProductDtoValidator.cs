using FluentValidation;
using MockAPI.Application.DTO;

namespace MockAPI.Application.Validators;
public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
	public UpdateProductDtoValidator()
	{
		RuleFor(x => x.name)
			.NotEmpty().WithMessage("Product name is required.");

		RuleFor(x => x.Data)
		 .NotEmpty().WithMessage("Product data is required.")
		 .When(x => x.Data != null);
	}
}