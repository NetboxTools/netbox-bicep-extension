namespace Bicep.Extension.Netbox.Handlers;

public class LocationHandler : NetboxResourceHandlerBase<Location, SlugIdentifiers>
{
    protected override string ApiPath => "/api/dcim/locations/";

    protected override string GetLookupQuery(Location properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(Location properties) => new()
    {
        Slug = properties.Slug
    };
}
