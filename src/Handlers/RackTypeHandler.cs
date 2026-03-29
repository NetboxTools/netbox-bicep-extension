namespace Bicep.Extension.Netbox.Handlers;

public class RackTypeHandler : NetboxResourceHandlerBase<RackType, SlugIdentifiers>
{
    protected override string ApiPath => "/api/dcim/rack-types/";

    protected override string GetLookupQuery(RackType properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(RackType properties) => new()
    {
        Slug = properties.Slug
    };
}
