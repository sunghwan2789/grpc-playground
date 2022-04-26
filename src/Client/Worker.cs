using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Client;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly Core.V1.Core.CoreClient _client;

    public Worker(ILogger<Worker> logger, Core.V1.Core.CoreClient client)
    {
        _logger = logger;
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var tickContext = _client.SubscribeTick(new Empty(), cancellationToken: stoppingToken);
        _ = Task.Run(async () =>
        {
            await foreach (var tick in tickContext.ResponseStream.ReadAllAsync())
            {
                _logger.LogInformation("Tick: {@Tick}", tick);
            }
        }, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
