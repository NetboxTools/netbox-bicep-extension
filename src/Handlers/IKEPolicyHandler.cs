namespace Bicep.Extension.Netbox.Handlers;

public class IKEPolicyHandler : NetboxResourceHandlerBase<IKEPolicy, NameIdentifiers>
{
    protected override string ApiPath => "/api/vpn/ike-policies/";

    protected override string GetLookupQuery(IKEPolicy properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(IKEPolicy properties) => new()
    {
        Name = properties.Name
    };
}
