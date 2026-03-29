namespace Bicep.Extension.Netbox.Handlers;

public class ClusterTypeHandler : NetboxResourceHandlerBase<ClusterType, SlugIdentifiers>
{
    protected override string ApiPath => "/api/virtualization/cluster-types/";

    protected override string GetLookupQuery(ClusterType properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(ClusterType properties) => new()
    {
        Slug = properties.Slug
    };
}
