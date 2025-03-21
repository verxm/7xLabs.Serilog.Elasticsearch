using Serilog.Elk.POC.Accessors.Interfaces;

namespace Serilog.Elk.POC.Enrichers
{
    internal class TenantLogEnricher : LogEnricher
    {
        private const string LOG_PROPERTY_NAME = "ZTenantId";
        private const string TENANT_ID_HEADER_KEY = "TenantId";

        public TenantLogEnricher(ITenantHeaderAccessor tenantHeaderAccessor) : base(LOG_PROPERTY_NAME, TENANT_ID_HEADER_KEY, tenantHeaderAccessor)
        {

        }
    }
}
