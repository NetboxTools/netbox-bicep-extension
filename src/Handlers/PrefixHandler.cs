namespace Bicep.Extension.Netbox.Handlers;

public class PrefixHandler : NetboxResourceHandlerBase<Prefix, PrefixIdentifiers>
{
    protected override string ApiPath => "/api/ipam/prefixes/";

    protected override string GetLookupQuery(Prefix properties) => $"prefix={properties.Prefix}";

    protected override PrefixIdentifiers GetIdentifiers(Prefix properties) => new()
    {
        Prefix = properties.Prefix
    };
}
