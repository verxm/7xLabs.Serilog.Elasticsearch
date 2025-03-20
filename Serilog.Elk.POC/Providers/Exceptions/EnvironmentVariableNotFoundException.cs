using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Serilog.Elk.POC.Providers.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    internal class EnvironmentVariableNotFoundException : Exception
    {
        private const string DEFAULT_ERRRO_MESSAGE_TEMPLATE = "Environment variable {0} not found.";

        public EnvironmentVariableNotFoundException(string key) : base(key) { }

        protected EnvironmentVariableNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static void ThrowIfNullOrEmpty(string key, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new EnvironmentVariableNotFoundException(string.Format(
                    DEFAULT_ERRRO_MESSAGE_TEMPLATE, 
                    key));
            }
        }
    }
}
