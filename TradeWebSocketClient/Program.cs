using System.Text;
using System.Net.WebSockets;


internal class Program
{
    private const string API_URL = "wss://streamer.cryptocompare.com/v2?api_key=e88a272ec4b86493dfa7d161e3c06090fe66ad22035e10e4b2b82c9a3ea68dd1";

    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var cancellationToken = new CancellationTokenSource();
        System.Net.WebSockets.ClientWebSocket websocket = new
            System.Net.WebSockets.ClientWebSocket();

        websocket.ConnectAsync(new Uri(API_URL), cancellationToken.Token).Wait();

        Console.WriteLine("Connected");

        byte[] buffer = new byte[1024];



        while (true) 
        {

            if (websocket.State == System.Net.WebSockets.WebSocketState.Open)
            {
                var result = await websocket.ReceiveAsync(buffer, cancellationToken.Token);
                Console.WriteLine("Data received");
                var resultText = System.Text.Encoding.UTF8.GetString(buffer);
                await Console.Out.WriteLineAsync(resultText);
            }

            if (cancellationToken.IsCancellationRequested) break;


        }
    }
}