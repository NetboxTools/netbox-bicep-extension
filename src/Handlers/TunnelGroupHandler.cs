namespace Bicep.Extension.Netbox.Handlers;

public class TunnelGroupHandler : SlugResourceHandler<TunnelGroup>
{
    protected override string ApiPath => "/api/vpn/tunnel-groups/";
}
