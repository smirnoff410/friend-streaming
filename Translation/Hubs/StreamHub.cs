using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;

namespace Translation.Hubs;

public class StreamHub : Hub
{
    private readonly StreamData _streamData;

    public StreamHub(StreamData streamData)
    {
        _streamData = streamData;
    }
    
    public async Task Send()
    {
        while (true)
        {
            await Clients.All.SendAsync("ReceiveMessage", _streamData.Data);
        }
    }
    
    public async IAsyncEnumerable<int> Streaming(CancellationToken cancellationToken)
    {
        while (true)
        {
            yield return _streamData.Data.Length;
            await Task.Delay(1000, cancellationToken);
        }
    }
    
    public async IAsyncEnumerable<int> Counter(
        int count,
        int delay,
        [EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        for (var i = 0; i < count; i++)
        {
            // Check the cancellation token regularly so that the server will stop
            // producing items if the client disconnects.
            cancellationToken.ThrowIfCancellationRequested();

            yield return i;

            // Use the cancellationToken in other APIs that accept cancellation
            // tokens so the cancellation can flow down to them.
            await Task.Delay(delay, cancellationToken);
        }
    }
}