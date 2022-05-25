using Microsoft.AspNetCore.Mvc;
using Translation;
using Translation.BackgroundServices;
using Translation.Hubs;
using Translation.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddSingleton<StreamData>();
builder.Services.AddHostedService<TranslationBackgroundService>();
builder.Services.AddScoped<IUdpListenerService, UdpListenerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<CounterService>();
//app.MapGrpcService<StreamerService>();
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);

app.MapHub<StreamHub>("/streamHub");
app.MapHub<ChatHub>("/chatHub");

app.MapControllers();
app.Run();