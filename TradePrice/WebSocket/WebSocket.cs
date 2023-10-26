using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using Websocket.Client;
using Newtonsoft.Json;
using csTradePiceWebSocket.DB;

namespace csTradePiceWebSocket.WebSocket;
class WSServer
{
    private string streaming_API_Key = "wsAJ4KAYHhrtR5lK7S9g";
    private HubConnection connection;
    private DataBaseIO db;

    public WSServer()
    {
        db = new DataBaseIO();
        connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/statushub").Build();
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


                client.ReconnectionHappened.Subscribe(info =>
                {
                    connection.InvokeAsync("UpdateStatus", "reconnected");
                    Console.WriteLine("Reconnection happened, type: " + info.Type);
                });

                client.DisconnectionHappened.Subscribe(info =>
                {
                    connection.InvokeAsync("UpdateStatus", "disconnected");
                    Console.WriteLine("Disconnection happened, type: " + info.Type);
                });


                client.MessageReceived.Subscribe(msg =>
                {

                    connection.InvokeAsync("UpdateStatus", "connected");
                    if (msg.ToString().ToLower() == "connected")
                    {
                        Console.WriteLine("Connection Happpend", msg.Text);
                        string data = "{\"userKey\": \"" + streaming_API_Key + "\", \"symbol\":\"EURUSD,XAUUSD,BTCUSD,BNBUSD\"}";
                        client.Send(data);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<Quote>(msg.Text);
                        if (result != null)
                            db.WriteDB(result);
                    }
                });
                await client.Start();
                exitEvent.WaitOne();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.ToString());
        }
    }
}