using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MockAPI.Application.Interfaces;
using MockAPI.Infrastructure.Services;
using MockAPI.Infrastructure.Settings;

namespace MockAPI.Infrastructure.DependencyInjection;
public static class InfrastructureServices
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration Configuration)
	{
		services.Configure<ExternalApiSettings>(Configuration.GetSection("ExternalApis"));

		services.AddHttpClient<IProductRepository, ProductServices>((sp, client) =>
		{
			var settings = sp.GetRequiredService<IOptions<ExternalApiSettings>>().Value;
			client.BaseAddress = new Uri(settings.ProductApiBaseUrl);
		});

		return services;
	}
}