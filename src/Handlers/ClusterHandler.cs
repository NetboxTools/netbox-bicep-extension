namespace Bicep.Extension.Netbox.Handlers;

public class ClusterHandler : NetboxResourceHandlerBase<Cluster, NameIdentifiers>
{
    protected override string ApiPath => "/api/virtualization/clusters/";

    protected override string GetLookupQuery(Cluster properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(Cluster properties) => new()
    {
        Name = properties.Name
    };
}
