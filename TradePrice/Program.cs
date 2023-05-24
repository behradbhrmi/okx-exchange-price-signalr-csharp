using API;
using WebSocket;
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
		api.Run();
        await Console.Out.WriteLineAsync("Hello");
        //WSServer webSocket = new WSServer();
        //webSocket.ReceivePrice();
    }
}
