namespace Bicep.Extension.Netbox.Handlers;

public class VLANHandler : NetboxResourceHandlerBase<VLAN, VLANIdentifiers>
{
    protected override string ApiPath => "/api/ipam/vlans/";

    protected override string GetLookupQuery(VLAN properties) => $"vid={properties.Vid}";

    protected override VLANIdentifiers GetIdentifiers(VLAN properties) => new()
    {
        Vid = properties.Vid
    };
}
