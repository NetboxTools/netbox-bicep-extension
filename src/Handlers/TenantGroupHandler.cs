namespace Bicep.Extension.Netbox.Handlers;

public class TenantGroupHandler : SlugResourceHandler<TenantGroup>
{
    protected override string ApiPath => "/api/tenancy/tenant-groups/";
}
