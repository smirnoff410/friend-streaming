using Count;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Translation.Services;

public class CounterService : Counter.CounterBase
{
    private readonly ILogger<CounterService> _logger;

    public CounterService(ILogger<CounterService> logger)
    {
        _logger = logger;
    }

    public override Task<CounterReply> IncrementCount(Empty request, ServerCallContext context)
    {
        return base.IncrementCount(request, context);
    }

    public override Task<CounterReply> AccumulateCount(IAsyncStreamReader<CounterRequest> requestStream, ServerCallContext context)
    {
        return base.AccumulateCount(requestStream, context);
    }

    public override Task Countdown(Empty request, IServerStreamWriter<CounterReply> responseStream, ServerCallContext context)
    {
        return base.Countdown(request, responseStream, context);
    }
}