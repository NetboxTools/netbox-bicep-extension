namespace Bicep.Extension.Netbox.Handlers;

public class IPSecProposalHandler : NetboxResourceHandlerBase<IPSecProposal, NameIdentifiers>
{
    protected override string ApiPath => "/api/vpn/ipsec-proposals/";

    protected override string GetLookupQuery(IPSecProposal properties) => $"name={properties.Name}";

    protected override NameIdentifiers GetIdentifiers(IPSecProposal properties) => new()
    {
        Name = properties.Name
    };
}
