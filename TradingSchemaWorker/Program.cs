using TradingSchemaSp;
using Webapplikasjoner_oblig;
using Webapplikasjoner_oblig.DAL;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<TradingSchemaWorker>();
        services.AddScoped<ITradingRepository, TradingRepository>();
    })
    .Build();

await host.RunAsync();
