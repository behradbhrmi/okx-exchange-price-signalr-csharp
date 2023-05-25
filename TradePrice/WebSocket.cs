using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using Websocket.Client;
using Newtonsoft.Json;
using DataBase;


namespace WebSocket;
class WSServer
{
	private string streaming_API_Key = "ws2eprRnpTVbchG64lvw";
	private HubConnection connection;
	private DataBaseIO db;

	public WSServer()
	{
		connection = new HubConnectionBuilder().WithUrl("http://localhost:8080/statushub").Build();
		db = new DataBaseIO();
	}

	public async Task ReceivePrice()
	{
		await connection.StartAsync();
		try
		{
			var exitEvent = new ManualResetEvent(false);
			var url = new Uri("wss://marketdata.tradermade.com/feedadv");

			using (var client = new WebsocketClient(url))
			{
				client.ReconnectTimeout = TimeSpan.FromSeconds(30);


				client.ReconnectionHappened.Subscribe(async info =>
				{
					await connection.InvokeAsync("UpdateStatus", "connected");
					Console.WriteLine("Reconnection happened, type: " + info.Type);
				});


				client.DisconnectionHappened.Subscribe(async info =>
				{
					await connection.InvokeAsync("UpdateStatus", "disconnected");
					Console.WriteLine("Disconnection happened, type: " + info.Type);
				});


				client.MessageReceived.Subscribe(async msg =>
				{

					if (msg.ToString().ToLower() == "connected")
					{
						await connection.InvokeAsync("UpdateStatus", "connected");
						string data = "{\"userKey\":\"" + streaming_API_Key + "\", \"symbol\":\"EURUSD,XAUUSD,COPPER\"}";
						client.Send(data);
					}
					else
					{
						var result = JsonConvert.DeserializeObject<Quote>(msg.Text);
						if (result != null)
							db.WriteDB(result);
					}
				});
				client.Start();
				exitEvent.WaitOne();
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("ERROR: " + ex.ToString());
		}
	}
}