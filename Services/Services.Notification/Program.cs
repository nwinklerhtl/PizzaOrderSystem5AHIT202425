using DataContracts.DataTransferObjects;
using DataContracts.Messages.ServiceMessages;
using Infrastructure.Messaging;
using Infrastructure.Messaging.RabbitMq;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using Services.Notification.Hubs;
using Constants = DataContracts.Messages.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("allowWebUi", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseResponseCompression();
app.MapHub<NotificationHub>("/notification-hub");
app.UseCors("allowWebUi");

var scope = app.Services.CreateScope();
await RabbitMqMessagingFactory.CreateReceiverAsync<Notification>(
    Constants.ExchangeName,
    async (cloudMessage, message) =>
    {
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();
        await hubContext.Clients.Group(message.OrderId.ToString()).SendAsync("OrderUpdated", new NotificationDto
        {
            Title = message.Title,
            Message = message.Message,
            CreatedAt = cloudMessage.Time ?? DateTimeOffset.UtcNow,
        });
    });

app.Run();