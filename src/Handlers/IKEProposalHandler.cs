namespace Bicep.Extension.Netbox.Handlers;

public class IKEProposalHandler : NetboxResourceHandlerBase<IKEProposal, NameIdentifiers>
{
    protected override string ApiPath => "/api/vpn/ike-proposals/";

    protected override string GetLookupQuery(IKEProposal properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(IKEProposal properties) => new()
    {
        Name = properties.Name
    };
}
