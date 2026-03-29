namespace Bicep.Extension.Netbox.Handlers;

public class IPAMRoleHandler : SlugResourceHandler<IPAMRole>
{
    protected override string ApiPath => "/api/ipam/roles/";
}
