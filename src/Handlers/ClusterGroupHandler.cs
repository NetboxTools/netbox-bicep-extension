namespace Bicep.Extension.Netbox.Handlers;

public class ClusterGroupHandler : SlugResourceHandler<ClusterGroup>
{
    protected override string ApiPath => "/api/virtualization/cluster-groups/";
}
