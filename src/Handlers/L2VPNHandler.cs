namespace Bicep.Extension.Netbox.Handlers;

public class L2VPNHandler : SlugResourceHandler<L2VPN>
{
    protected override string ApiPath => "/api/vpn/l2vpns/";
}
