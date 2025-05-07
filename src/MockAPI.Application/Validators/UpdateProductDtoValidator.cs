using FluentValidation;
using MockAPI.Application.DTO;

namespace MockAPI.Application.Validators;
public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
	public UpdateProductDtoValidator()
    {
        RuleFor(x => x.name).ValidateProductName();
    }
}