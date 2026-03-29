namespace Bicep.Extension.Netbox.Handlers;

public class IPAddressHandler : NetboxResourceHandlerBase<IPAddress, IPAddressIdentifiers>
{
    protected override string ApiPath => "/api/ipam/ip-addresses/";

    protected override string GetLookupQuery(IPAddress properties) => $"address={properties.Address}";

    protected override IPAddressIdentifiers GetIdentifiers(IPAddress properties) => new()
    {
        Address = properties.Address
    };
}
