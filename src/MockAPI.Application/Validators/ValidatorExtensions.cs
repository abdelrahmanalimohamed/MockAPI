using FluentValidation;

namespace MockAPI.Application.Validators;
public static class ValidatorExtensions
{
	public static IRuleBuilderOptions<T, string> ValidateProductName<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		return ruleBuilder.NotEmpty().WithMessage("Product name is required.");
	}
}
