using Serilog.Elk.POC.Providers.Cosntants;
using Serilog.Elk.POC.Providers.Exceptions;
using System;

namespace Serilog.Elk.POC.Providers
{
    internal static class EnvironmentVariableProvider
    {
        internal static string EnvironmentName { get; private set; }

        internal static string ServiceName { get; private set; }
        internal static string MicroServiceName { get; private set; }

        internal static string ElkUri { get; private set; }
        internal static string SerilogEventLevel { get; private set; }
        internal static string MicrosoftEventLevel { get; private set; }
        internal static string MicrosoftAspNetEventLevel { get; private set; }

        static EnvironmentVariableProvider()
        {
            EnvironmentName = GetRequiredVariableValue(VariableKeys.ENVIRONMENT_NAME);

            ServiceName = GetRequiredVariableValue(VariableKeys.SERVICE_NAME);
            MicroServiceName = GetRequiredVariableValue(VariableKeys.MICRO_SERVICE_NAME);

            ElkUri = GetRequiredVariableValue(VariableKeys.ELK_URI);
            SerilogEventLevel = GetRequiredVariableValue(VariableKeys.SERILOG_EVENT_LEVEL);
            MicrosoftEventLevel = GetRequiredVariableValue(VariableKeys.MICROSOFT_EVENT_LEVEL);
            MicrosoftAspNetEventLevel = GetRequiredVariableValue(VariableKeys.MICROSOFT_ASPNET_EVENT_LEVEL);
        }

        private static string GetRequiredVariableValue(string variableKey)
        {
            var variableValue = GetVariableValue(variableKey);

            EnvironmentVariableNotFoundException.ThrowIfNullOrEmpty(variableKey, variableValue);

            return variableValue!;
        }

        private static string? GetVariableValue(string variableKey)
        {
            return Environment.GetEnvironmentVariable(variableKey);
        }
    }
}
