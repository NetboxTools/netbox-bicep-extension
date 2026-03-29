namespace Bicep.Extension.Netbox.Handlers;

public class ASNHandler : NetboxResourceHandlerBase<ASN, ASNIdentifiers>
{
    protected override string ApiPath => "/api/ipam/asns/";

    protected override string GetLookupQuery(ASN properties) => $"asn={properties.Asn}";

    protected override ASNIdentifiers GetIdentifiers(ASN properties) => new()
    {
        Asn = properties.Asn
    };
}
