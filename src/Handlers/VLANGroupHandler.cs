namespace Bicep.Extension.Netbox.Handlers;

public class VLANGroupHandler : NetboxResourceHandlerBase<VLANGroup, SlugIdentifiers>
{
    protected override string ApiPath => "/api/ipam/vlan-groups/";

    protected override string GetLookupQuery(VLANGroup properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(VLANGroup properties) => new()
    {
        Slug = properties.Slug
    };
}
