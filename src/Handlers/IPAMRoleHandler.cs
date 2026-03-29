namespace Bicep.Extension.Netbox.Handlers;

public class IPAMRoleHandler : NetboxResourceHandlerBase<IPAMRole, SlugIdentifiers>
{
    protected override string ApiPath => "/api/ipam/roles/";

    protected override string GetLookupQuery(IPAMRole properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(IPAMRole properties) => new()
    {
        Slug = properties.Slug
    };
}
