using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DataBase;
using SignalR;


namespace API;
class APIServer
{
	private DBManager dbManager;

	public APIServer()
	{
		dbManager = new DBManager();
	}

	public async Task Run()
	{
		var builder = WebApplication.CreateBuilder();

		// Add services to the container.
		builder.Services.AddSignalR();
		builder.Services.AddCors();

		var app = builder.Build();

		app.UseCors(builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyMethod()
				   .AllowAnyHeader();
		});

		app.UseRouting();
		

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapHub<StatusHub>("/statushub");

			endpoints.MapGet("/fetch", (HttpContext httpContext) =>
			{


				string? currencyName = httpContext.Request.Query["currencyName"];



				string? startDate = httpContext.Request.Query["startDate"];


				string? endDate = httpContext.Request.Query["endDate"];

				httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");



				httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET");



				httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");


				if (currencyName == null || startDate == null || endDate == null)
					return null;
				return dbManager.FetchPrice(currencyName, startDate, endDate);
			});

			endpoints.MapGet("/currencies", (HttpContext httpContext) =>
			{
				return dbManager.FetchCurrencies();
			});

		});

		app.Run();
	}
}
