using Serilog.Elk.POC.Accessors.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Api.Tests.Accessors
{
    [ExcludeFromCodeCoverage]
    internal class TenantHeaderAccessorTest : ITenantHeaderAccessor
    {
        IHttpContextAccessor _contextAccessor;

        public TenantHeaderAccessorTest() : this(new HttpContextAccessor())
        {

        }

        private TenantHeaderAccessorTest(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetValue(string key)
        {
            var value = _contextAccessor?
                .HttpContext?
                .Request?
                .Headers[key]
                .ToString() ?? string.Empty;

            return value;
        }
    }
}
