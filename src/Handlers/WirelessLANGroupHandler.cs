namespace Bicep.Extension.Netbox.Handlers;

public class WirelessLANGroupHandler : SlugResourceHandler<WirelessLANGroup>
{
    protected override string ApiPath => "/api/wireless/wireless-lan-groups/";
}
