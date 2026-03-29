namespace Bicep.Extension.Netbox.Handlers;

public class ClusterTypeHandler : SlugResourceHandler<ClusterType>
{
    protected override string ApiPath => "/api/virtualization/cluster-types/";
}
