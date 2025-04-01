using MockAPI.Application.DependencyInjection;
using MockAPI.Application.Exceptions;
using MockAPI.Infrastructure.DependencyInjection;

namespace MockAPI.Presentation;
public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllers();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		builder.Services
			  .AddApplication()
			  .AddInfrastructure(builder.Configuration);

		var app = builder.Build();
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();
		app.MapControllers();
		app.UseMiddleware<ExceptionHandling>();
		app.Run();
	}
}