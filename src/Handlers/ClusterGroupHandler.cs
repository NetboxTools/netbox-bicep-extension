namespace Bicep.Extension.Netbox.Handlers;

public class ClusterGroupHandler : NetboxResourceHandlerBase<ClusterGroup, SlugIdentifiers>
{
    protected override string ApiPath => "/api/virtualization/cluster-groups/";

    protected override string GetLookupQuery(ClusterGroup properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(ClusterGroup properties) => new()
    {
        Slug = properties.Slug
    };
}
