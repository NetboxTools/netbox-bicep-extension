namespace Bicep.Extension.Netbox.Handlers;

public class WirelessLANHandler : NetboxResourceHandlerBase<WirelessLAN, SsidIdentifiers>
{
    protected override string ApiPath => "/api/wireless/wireless-lans/";

    protected override string GetLookupQuery(WirelessLAN properties) => $"ssid={properties.Ssid}";

    protected override SsidIdentifiers GetIdentifiers(WirelessLAN properties) => new()
    {
        Ssid = properties.Ssid
    };
}
