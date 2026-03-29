namespace Bicep.Extension.Netbox.Handlers;

public class ASNRangeHandler : NetboxResourceHandlerBase<ASNRange, SlugIdentifiers>
{
    protected override string ApiPath => "/api/ipam/asn-ranges/";

    protected override string GetLookupQuery(ASNRange properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(ASNRange properties) => new()
    {
        Slug = properties.Slug
    };
}
