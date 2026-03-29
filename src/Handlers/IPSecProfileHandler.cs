namespace Bicep.Extension.Netbox.Handlers;

public class IPSecProfileHandler : NetboxResourceHandlerBase<IPSecProfile, NameIdentifiers>
{
    protected override string ApiPath => "/api/vpn/ipsec-profiles/";

    protected override string GetLookupQuery(IPSecProfile properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(IPSecProfile properties) => new()
    {
        Name = properties.Name
    };
}
