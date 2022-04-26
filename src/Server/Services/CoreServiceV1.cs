using Core.V1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Server.Services;

public class CoreServiceV1 : Core.V1.Core.CoreBase
{
    public override Task<HeartbeatReply> Heartbeat(HeartbeatRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HeartbeatReply());
    }

    public override async Task SubscribeTick(Empty request, IServerStreamWriter<TickEvent> responseStream, ServerCallContext context)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await timer.WaitForNextTickAsync(context.CancellationToken);

            await responseStream.WriteAsync(new TickEvent
            {
                Now = DateTimeOffset.Now.ToTimestamp(),
            });
        }
    }
}
