using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Pkg.Logs
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
                .MinimumLevel.Is(LogEventLevel.Debug);
            //.MinimumLevel.Override("Microsoft", EnvironmentWrapper.MicrosoftEventLevel.ToLogEventLevel())
            //.MinimumLevel.Override("Microsoft.AspNetCore", EnvironmentWrapper.MicrosoftAspNetEventLevel.ToLogEventLevel());
        }

        private static LoggerConfiguration AddDefaultEnrichers(this LoggerConfiguration configuration)
        {
            return configuration
                .Enrich.FromLogContext();
            //.Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers())
            //.Enrich.WithProperty("D1ServiceName", EnvironmentWrapper.ServiceName)
            //.Enrich.WithProperty("D1MicroServiceName", EnvironmentWrapper.MicroServiceName)
            //.Enrich.WithProperty("D1Environment", EnvironmentWrapper.CurrentDescription)
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
            return configuration
                .WriteTo
                .Console();
        }
    }
}
