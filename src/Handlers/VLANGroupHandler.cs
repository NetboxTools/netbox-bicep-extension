namespace Bicep.Extension.Netbox.Handlers;

public class VLANGroupHandler : SlugResourceHandler<VLANGroup>
{
    protected override string ApiPath => "/api/ipam/vlan-groups/";
}
