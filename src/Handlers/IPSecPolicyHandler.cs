namespace Bicep.Extension.Netbox.Handlers;

public class IPSecPolicyHandler : NetboxResourceHandlerBase<IPSecPolicy, NameIdentifiers>
{
    protected override string ApiPath => "/api/vpn/ipsec-policies/";

    protected override string GetLookupQuery(IPSecPolicy properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(IPSecPolicy properties) => new()
    {
        Name = properties.Name
    };
}
