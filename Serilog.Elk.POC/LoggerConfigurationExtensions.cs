using Serilog;
using Serilog.Elk.POC.Extensions;
using Serilog.Elk.POC.Providers;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using System;

namespace Serilog.Elk.POC
{
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// Add default configuration of MinimumLevel, Enrichers and GrafanaLoki labels.
        /// </summary>
        public static LoggerConfiguration AddDefaultConfiguration(this LoggerConfiguration configuration)
        {
            return configuration
                .SetMinimumLevel()
                .AddDefaultEnrichers()
                .ConfigureElasticsearchkiSinks();
        }

        /// <summary>        
        /// Add default configuration of MinimumLevel, Enrichers, GrafanaLoki labels with TenantId.        
        /// </summary>
        /// <param name="tenantAccessor">Instance of an accessor responsible to get TenantId value of a context.</param>
        //public static LoggerConfiguration AddDefaultConfigurationWithTenant(this LoggerConfiguration configuration, ITenantHeaderAccessor tenantAccessor)
        public static LoggerConfiguration AddDefaultConfigurationWithTenant(this LoggerConfiguration configuration)
        {
            //if (tenantAccessor == null)
            //{
            //    throw new ArgumentNullException(nameof(tenantAccessor));
            //}

            // var tenantEnricher = new TenantLogEnricher(tenantAccessor);

            return configuration
                .AddDefaultConfiguration();
            //.AddCustomEnrichers(tenantEnricher);
        }

        private static LoggerConfiguration SetMinimumLevel(this LoggerConfiguration configuration)
        {
            return configuration
                .MinimumLevel.Is(
                    EnvironmentVariableProvider.SerilogEventLevel.ToLogEventLevel())
                .MinimumLevel.Override(
                    "Microsoft", 
                    EnvironmentVariableProvider.MicrosoftEventLevel.ToLogEventLevel())
                .MinimumLevel.Override(
                    "Microsoft.AspNetCore", 
                    EnvironmentVariableProvider.MicrosoftAspNetEventLevel.ToLogEventLevel());
        }

        private static LoggerConfiguration AddDefaultEnrichers(this LoggerConfiguration configuration)
        {
            return configuration
                .Enrich.FromLogContext()
                //.Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers()) // https://rehansaeed.com/logging-with-serilog-exceptions/
                .Enrich.WithProperty("ZSerivceName", EnvironmentVariableProvider.ServiceName)
                .Enrich.WithProperty("ZMicroServiceName", EnvironmentVariableProvider.MicroServiceName)
                .Enrich.WithProperty("ZEnvironment", EnvironmentVariableProvider.EnvironmentName);
                //.AddTraceIdEnricher();
        }

        //private static LoggerConfiguration AddTraceIdEnricher(this LoggerConfiguration configuration)
        //{
        //    return configuration.AddCustomEnrichers(
        //        new TraceIdEnricher(),
        //        new EnvironmentEnricher());
        //}

        //private static LoggerConfiguration AddCustomEnrichers(this LoggerConfiguration configuration, params ILogEventEnricher[] enrichers)
        //{
        //    return configuration
        //        .Enrich.With(enrichers);
        //}

        private static LoggerConfiguration ConfigureElasticsearchkiSinks(this LoggerConfiguration configuration)
        {
            var indexName = EnvironmentVariableProvider
                .MicroServiceName
                .ToLower();

            return configuration
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(EnvironmentVariableProvider.ElkUri))
                //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elklogs.internalstaging:9200"))
                {
                    // TODO: Puxar discussão de qual index usar: ServiceName, MicroServiceName, indicador de ambiente?
                    // Seria interessante ter todos os logs em um lugar só caso eu queira pesquisar por TraceId ou algo assim
                    // Vi que o no index-pattern tem como criar um com apenas *
                    // ATENÇÃO: IndexDecider não pode ter letras maiúsculas - por algum motivo não chega no ELK
                    IndexDecider = (_, offset) => $"{indexName}.{offset.Date.Year}.{offset.Date.Month}.{offset.Date.Day}",
                    IndexFormat = $"{indexName}." + ".{0:yyyy.MM.dd}",
                    ConnectionTimeout = TimeSpan.FromSeconds(10),
                    // TODO: FailureCallback tem a Exception que ocorreu ao tentar enviar o log na versão 10 do pacote Serilog.Sinks.Elasticsearch mas ela está depreciada https://www.nuget.org/packages/Serilog.Sinks.Elasticsearch/10.0.0#releasenotes-body-tab
                    // TODO: Fazer testes com o pacote Elastic.Serilog.Sinks para ver se a compatível com a versão atual do ELK que usamos.
                    // Elastic.Serilog.Sinks é o pacote que substitui Serilog.Sinks.Elasticsearch: https://github.com/serilog-contrib/serilog-sinks-elasticsearch?tab=readme-ov-file
                    // MR com a adição do erro de envio do evento: https://github.com/serilog-contrib/serilog-sinks-elasticsearch/pull/449
                    FailureCallback = HandleFailureCallback,
                    EmitEventFailure = EmitEventFailureHandling.RaiseCallback,
                    CustomFormatter = new ElasticsearchJsonFormatter(), // Atenção na formatação sem essa propriedade
                    MinimumLogEventLevel = LogEventLevel.Information,
                    AutoRegisterTemplate = true,
                });
        }

        private static void HandleFailureCallback(LogEvent logEvent)
        {
            Console.WriteLine($"Sending log to Elasticsearch failed. LogEvent.MessageTemplate: {logEvent?.MessageTemplate}");
        }
    }
}
