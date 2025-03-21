using Serilog.Core;
using Serilog.Elk.POC.Accessors.Interfaces;
using Serilog.Events;
using System;

namespace Serilog.Elk.POC.Enrichers
{
    internal class LogEnricher : ILogEventEnricher
    {
        private readonly string _logPropertyName;
        private readonly string _headerKey;
        private readonly IHeaderAccessor _headerAccessor;

        public LogEnricher(
            string logPropertyName, 
            string headerKey, 
            IHeaderAccessor headerAccessor)
        {
            Validate(logPropertyName, headerKey, headerAccessor);

            _logPropertyName = logPropertyName;
            _headerKey = headerKey;
            _headerAccessor = headerAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var value = _headerAccessor.GetValue(_headerKey);

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var property = new LogEventProperty(_logPropertyName, new ScalarValue(value));
            logEvent.AddOrUpdateProperty(property);
        }

        // Refactor here
        private static void Validate(string logPropertyName, string headerKey, IHeaderAccessor headerAccessor)
        {
            if (string.IsNullOrEmpty(logPropertyName))
            {
                throw new ArgumentException($"{nameof(logPropertyName)} can't be null or empty.", nameof(logPropertyName));
            }

            if (string.IsNullOrEmpty(headerKey))
            {
                throw new ArgumentException($"{nameof(headerKey)} can't be null or empty.", nameof(headerKey));
            }

            if (headerAccessor == null)
            {
                throw new ArgumentNullException(nameof(headerAccessor), $"{nameof(headerAccessor)} can't be null.");
            }
        }
    }
}
