using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;

namespace Catalog.Infrastructure.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CatalogDbContext>(configure =>
        {
            configure.UseSqlServer(builder.Configuration.GetConnectionString(CatalogDbContext.DefaultConnectionStringName));
        });

        builder.Services.AddMassTransit(configure =>
        {
            var brokerConfig = builder.Configuration.GetSection(BrokerOptions.SectionName)
                                                    .Get<BrokerOptions>();
            if (brokerConfig is null)
            {
                throw new ArgumentNullException(nameof(BrokerOptions));
            }

            configure.AddConsumers(Assembly.GetExecutingAssembly());
            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(brokerConfig.Host, hostConfigure =>
                {
                    hostConfigure.Username(brokerConfig.Username);
                    hostConfigure.Password(brokerConfig.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddOptions<CatalogOptions>()
                        .BindConfiguration(nameof(CatalogOptions));

    }

    public static void LoggerConfigure(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((hostBuilderContext, configureLogger) =>
        {
            configureLogger.ReadFrom.Configuration(hostBuilderContext.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("service_name", "Catalog");

            configureLogger.WriteTo.Async(sinkConfig =>
            {
                sinkConfig.Console(Serilog.Events.LogEventLevel.Error, theme: AnsiConsoleTheme.Code,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} <{SourceContext}>] {Message:lj}");

                sinkConfig.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = false,
                    ConnectionTimeout = TimeSpan.FromSeconds(5),
                    InlineFields = true,
                    MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information,
                    IndexDecider = (logEvent,dateTimeOffset)=>$"catalog-{dateTimeOffset:yyyy-MM-dd}-{logEvent.Level}",
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.RaiseCallback,
                    FailureCallback =(logEvent,ex)=>Console.WriteLine($"{logEvent},Ex:{0}",ex.Message)
                });
            });
        });
    }
}