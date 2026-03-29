namespace Bicep.Extension.Netbox.Handlers;

public class TenantHandler : SlugResourceHandler<Tenant>
{
    protected override string ApiPath => "/api/tenancy/tenants/";
}
