namespace Bicep.Extension.Netbox.Handlers;

public class TunnelHandler : NetboxResourceHandlerBase<Tunnel, NameIdentifiers>
{
    protected override string ApiPath => "/api/vpn/tunnels/";

    protected override string GetLookupQuery(Tunnel properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(Tunnel properties) => new()
    {
        Name = properties.Name
    };
}
