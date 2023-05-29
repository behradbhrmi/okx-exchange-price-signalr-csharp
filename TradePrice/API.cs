using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DataBase;
using SignalR;
using System.Net;


namespace API;
class APIServer
{
    private DBManager dbManager;

    public APIServer()
    {
        dbManager = new DBManager();
    }

    public void Run()
    {
        var builder = WebApplication.CreateBuilder();

        // Add services to the container.
        builder.Services.AddSignalR();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });
        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseCors();

        app.UseRouting();

        app.MapHub<StatusHub>("/statushub");
        app.MapControllers();

        
        //      app.UseEndpoints(endpoints =>
        //{


        //	//endpoints.MapGet("/fetch", (HttpContext httpContext) =>
        //	//{


        //	//	string? currencyName = httpContext.Request.Query["currencyName"];



        //	//	string? startDate = httpContext.Request.Query["startDate"];


        //	//	string? endDate = httpContext.Request.Query["endDate"];

        //	//	httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");



        //	//	httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET");



        //	//	httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");


        //	//	if (currencyName == null || startDate == null || endDate == null)
        //	//		return null;
        //	//	return dbManager.FetchPrice(currencyName, startDate, endDate);
        //	//});

        //	//endpoints.MapGet("/currencies", (HttpContext httpContext) =>
        //	//{
        //	//	return dbManager.FetchCurrencies();
        //	//});

        //});

        app.Run();
    }
}
