using Newtonsoft.Json;
using Websocket.Client;
using DBManager;


namespace WebSocket;
class WSServer
{
	private string streaming_API_Key = "ws2eprRnpTVbchG64lvw";

	public async Task ReceivePrice()
	{
		try
		{
			var exitEvent = new ManualResetEvent(false);
			var url = new Uri("wss://marketdata.tradermade.com/feedadv");

			//var statusHub = 
			//statusHub.Clients.All.SendAsync("StatusUpdate", "Data fetched successfully.");

			using (var client = new WebsocketClient(url))
			{
				client.ReconnectTimeout = TimeSpan.FromSeconds(30);

				//should be async
				client.ReconnectionHappened.Subscribe(info =>
				{
					Console.WriteLine("Reconnection happened, type: " + info.Type);

					//await hubContext.Clients.All.SendAsync("UpdateStatus", "connected");
				});

				client.DisconnectionHappened.Subscribe(async info =>
				{
					Console.WriteLine("Disconnection happened, type: " + info.Type);
					if (info.Type != DisconnectionType.ByUser)
					{
						//await hubContext.Clients.All.SendAsync("UpdateStatus", "disconnected");
					}
				});


				client.MessageReceived.Subscribe(msg =>
				{
					var dataBase = new DataBaseIO();

					if (msg.ToString().ToLower() == "connected")
					{
						string data = "{\"userKey\":\"" + streaming_API_Key + "\", \"symbol\":\"EURUSD,XAUUSD,COPPER\"}";
						client.Send(data);
					}
					else
					{
						var result = JsonConvert.DeserializeObject<Quote>(msg.Text);
						if (result != null)
							dataBase.WriteDB(result);
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