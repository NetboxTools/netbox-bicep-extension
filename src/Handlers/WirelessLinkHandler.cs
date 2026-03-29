namespace Bicep.Extension.Netbox.Handlers;

public class WirelessLinkHandler : NetboxResourceHandlerBase<WirelessLink, WirelessLinkIdentifiers>
{
    protected override string ApiPath => "/api/wireless/wireless-links/";

    protected override string GetLookupQuery(WirelessLink properties) => $"interface_a_id={properties.InterfaceA}";

    protected override WirelessLinkIdentifiers GetIdentifiers(WirelessLink properties) => new()
    {
        InterfaceA = properties.InterfaceA
    };
}
