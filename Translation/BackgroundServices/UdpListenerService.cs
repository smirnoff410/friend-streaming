using System.Net.Sockets;
using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace Translation.BackgroundServices;

public class UdpListenerService : IUdpListenerService
{
    private readonly UrlSettings _urlSettings;
    private readonly ILogger<UdpListenerService> _logger;

    public UdpListenerService(
        IOptions<UrlSettings> options,
        ILogger<UdpListenerService> logger)
    {
        _urlSettings = options.Value;
        _logger = logger;
    }
    
    public async Task StartListener()
    {
        var client = new UdpClient(_urlSettings.UdpPort);
        var grpcUrl = _urlSettings.RelayGrpcUrl;
        using var channel = GrpcChannel.ForAddress(grpcUrl);
        _logger.LogInformation($"Connecting gRPC to {grpcUrl}");
        var grpcClient = new Streamer.StreamerClient(channel);
        using var call = grpcClient.GetStreamBytes();
        while (true)
        {
            var data = await client.ReceiveAsync();
            _logger.LogInformation($"Bytes recived: {data.Buffer.Length * sizeof(byte)}");
            await call.RequestStream.WriteAsync(new StreamRequest { Data = ByteString.CopyFrom(data.Buffer) });
        }
    }
}