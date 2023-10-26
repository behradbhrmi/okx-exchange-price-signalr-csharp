using csTradePiceWebSocket.API;
using csTradePiceWebSocket.WebSocket;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Runtime.CompilerServices;


public class Quote
{
	public string symbol { get; set; }
	public long  ts { get; set; }
	public double bid { get; set; }
	public double ask { get; set; }
	public double mid { get; set; }
}

class Program
{ 
	static public async Task Main(string[] args)
	{
		APIServer api = new APIServer();
        WSServer webSocket = new WSServer();

		
		var ap =  Task.Run(() => api.Run());

		//var wb = Task.Run(() => webSocket.ReceivePrice());


		//var connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/statushub").Build();
		//await connection.StartAsync();
		//var sig = Task.Run(() =>
		//{
		//	int num = 0;
		//	while (true)
		//	{
		//		num++;
		//		if (num % 2 == 0) connection.InvokeAsync("UpdateStatus", "HEllo");
		//		else connection.InvokeAsync("UpdateStatus", "Bye Bye");
		//		Thread.Sleep(2000);
		//	}
		//});


		await Task.WhenAll(ap);
    }
}
