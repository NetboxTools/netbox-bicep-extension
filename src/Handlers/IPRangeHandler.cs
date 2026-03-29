namespace Bicep.Extension.Netbox.Handlers;

public class IPRangeHandler : NetboxResourceHandlerBase<IPRange, IPRangeIdentifiers>
{
    protected override string ApiPath => "/api/ipam/ip-ranges/";

    protected override string GetLookupQuery(IPRange properties) => $"start_address={properties.StartAddress}";

    protected override IPRangeIdentifiers GetIdentifiers(IPRange properties) => new()
    {
        StartAddress = properties.StartAddress
    };
}
