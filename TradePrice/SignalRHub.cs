using Microsoft.AspNetCore.SignalR;


namespace SignalR
{
	public class StatusHub : Hub
	{
		public async Task UpdateStatus(string status)
		{
			await Clients.All.SendAsync("StatusUpdate", status);
		}
	}
}
