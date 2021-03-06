using System.Net;
using System.Net.Sockets;
using Google.Protobuf;
using Grpc.Net.Client;

namespace Translation.BackgroundServices;

public class UdpListenerService : IUdpListenerService
{
    private readonly ILogger<UdpListenerService> _logger;

    public UdpListenerService(ILogger<UdpListenerService> logger)
    {
        _logger = logger;
    }
    
    public async Task StartListener()
    {
        var client = new UdpClient(4100);
        var grpcUrl = "http://relay:50051";
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