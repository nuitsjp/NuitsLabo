using GenericHostConsoleAppStudy;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging((context, builder) =>
    {
        builder.ClearProviders();
    })
    .Build();

await host.RunAsync();
