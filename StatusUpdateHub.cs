using Microsoft.AspNetCore.SignalR;

namespace Restaurant_Management_System
{
    public class StatusUpdateHub : Hub
    {
        public async Task SendStatusUpdate(int orderId, string status)
        {
            // Broadcast the status update to all connected clients
            await Clients.All.SendAsync("ReceiveStatusUpdate", orderId, status);
        }

    }

}
