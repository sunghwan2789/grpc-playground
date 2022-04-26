using Client;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddGrpcClient<Core.V1.Core.CoreClient>(options =>
        {
            options.Address = new Uri("https://localhost:9999");
        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
