using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MockAPI.Application.DependencyInjection;
public static class ApplicationServices
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();

		services.AddMediatR(configuration =>
		{
			configuration.RegisterServicesFromAssembly(assembly);
			configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
		});

		services.AddValidatorsFromAssembly(assembly);
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

		return services;
	}
}