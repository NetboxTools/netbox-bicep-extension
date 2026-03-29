namespace Bicep.Extension.Netbox.Handlers;

public class TenantHandler : NetboxResourceHandlerBase<Tenant, SlugIdentifiers>
{
    protected override string ApiPath => "/api/tenancy/tenants/";

    protected override string GetLookupQuery(Tenant properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(Tenant properties) => new()
    {
        Slug = properties.Slug
    };
}
