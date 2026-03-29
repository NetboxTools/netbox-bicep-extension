namespace Bicep.Extension.Netbox.Handlers;

public class RIRHandler : NetboxResourceHandlerBase<RIR, SlugIdentifiers>
{
    protected override string ApiPath => "/api/ipam/rirs/";

    protected override string GetLookupQuery(RIR properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(RIR properties) => new()
    {
        Slug = properties.Slug
    };
}
