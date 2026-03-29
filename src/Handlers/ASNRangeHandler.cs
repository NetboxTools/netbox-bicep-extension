namespace Bicep.Extension.Netbox.Handlers;

public class ASNRangeHandler : SlugResourceHandler<ASNRange>
{
    protected override string ApiPath => "/api/ipam/asn-ranges/";
}
