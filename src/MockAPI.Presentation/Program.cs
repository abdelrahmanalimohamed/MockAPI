using FastEndpoints;
using MockAPI.Application.DependencyInjection;
using MockAPI.Application.Exceptions;
using MockAPI.Infrastructure.DependencyInjection;


namespace MockAPI.Presentation;
public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.Services.AddEndpointsApiExplorer();

		builder.Services
			.AddApplication()
			.AddInfrastructure(builder.Configuration);

		builder.Services.AddFastEndpoints();

		var app = builder.Build();

		app.UseHttpsRedirection();
		app.UseFastEndpoints();
		app.UseMiddleware<ExceptionHandling>();
		app.Run();
	}
}