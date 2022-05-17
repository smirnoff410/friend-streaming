using System.Net;
using System.Net.Sockets;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Stream;

namespace Translation.Services;

public class StreamerService : Streamer.StreamerBase
{
    private readonly StreamData _streamData;
    private readonly ILogger<StreamerService> _logger;

    public StreamerService(StreamData streamData, ILogger<StreamerService> logger)
    {
        _streamData = streamData;
        _logger = logger;
    }

    public override async Task<Empty> GetStreamBytes(IAsyncStreamReader<StreamRequest> request, ServerCallContext context)
    {
        await foreach (var data in request.ReadAllAsync())
        {
            _logger.LogInformation($"Get data from client: {data.Data}");
        }

        return new Empty();
    }

    public override Task<Empty> SayHello(CounterRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Get byte array from client. Length: {request.Data.Length}");
        
        var udpClient = new UdpClient();
        var bytes = request.Data.ToByteArray();
        udpClient.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Parse("192.168.0.75"), 4100));

        _streamData.Data = bytes;

        return Task.FromResult(new Empty());
    }
}