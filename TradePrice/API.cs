using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DBManager;


namespace API;
class APIServer
{
	public async Task Run()
	{
		var builder = WebApplication.CreateBuilder();

		// Add services to the container.
		//builder.Services.AddSignalR();
		builder.Services.AddCors();

		builder.Services.AddSignalR();

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
			//endpoints.MapHub<StatusHub>("/statushub");

			endpoints.MapGet("/fetch", (HttpContext httpContext) =>
			{
				string currencyName = httpContext.Request.Query["currencyName"];
				DateTime startDate = Convert.ToDateTime(httpContext.Request.Query["startDate"]);
				DateTime endDate = Convert.ToDateTime(httpContext.Request.Query["endDate"]);

				if (currencyName != null && startDate != null)
				{
					var db = new DataBaseIO();
					return JsonConvert.SerializeObject(db.FetchPrice(currencyName, startDate, endDate));
					
				}
				return null;
			});


			endpoints.MapGet("/currencies", (HttpContext httpContext) =>
			{
				var db = new DataBaseIO();
				return JsonConvert.SerializeObject(db.FetchCurrencies());
			});
		});

		app.Run();
	}
}
