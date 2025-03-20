using Serilog.Events;
using System;

namespace Serilog.Elk.POC.Extensions
{
    internal static class LogEventLevelExtensions
    {
        public static LogEventLevel ToLogEventLevel(this string text)
        {
            try
            {
                return (LogEventLevel)Enum.Parse(typeof(LogEventLevel), text);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Not supported level '{text}' value", nameof(text), ex);
            }
        }
    }
}
