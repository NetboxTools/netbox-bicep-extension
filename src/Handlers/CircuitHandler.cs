namespace Bicep.Extension.Netbox.Handlers;

public class CircuitHandler : NetboxResourceHandlerBase<Circuit, CidIdentifiers>
{
    protected override string ApiPath => "/api/circuits/circuits/";

    protected override string GetLookupQuery(Circuit properties) => $"cid={properties.Cid}";

    protected override CidIdentifiers GetIdentifiers(Circuit properties) => new()
    {
        Cid = properties.Cid
    };
}
