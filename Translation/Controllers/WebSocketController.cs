using System.Net.WebSockets;
using AForge.Video.DirectShow;
using Microsoft.AspNetCore.Mvc;

namespace Translation.Controllers;

public class WebSocketController : ControllerBase
{
    private readonly ILogger<WebSocketController> _logger;
    private readonly StreamData _streamData;

    public WebSocketController(ILogger<WebSocketController> logger, StreamData streamData)
    {
        _logger = logger;
        _streamData = streamData;
    }
    
    [HttpGet("")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    [HttpPost("data")]
    public async Task Post(object data)
    {
        
        _logger.LogInformation($"Post log {data.GetType()}");

    }

    [HttpGet("data")]
    public async Task GetPost(object data)
    {
        _logger.LogInformation("Get log");
        
    }
    
    private async Task Echo(WebSocket webSocket)
    {
        var buffer = new byte[4096 * 4];
        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            var buffer1 = new byte[4096 * 4];
            _streamData.Data.CopyTo(buffer1, 0);
            await webSocket.SendAsync(
                new ArraySegment<byte>(_streamData.Data, 0, _streamData.Data.Length),
                WebSocketMessageType.Binary,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }
}