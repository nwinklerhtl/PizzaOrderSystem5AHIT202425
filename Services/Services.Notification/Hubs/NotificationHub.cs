using Microsoft.AspNetCore.SignalR;

namespace Services.Notification.Hubs;

public class NotificationHub : Hub
{
    public async Task RegisterApprovalUpdates(string orderId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, orderId);
    }
}