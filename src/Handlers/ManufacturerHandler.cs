namespace Bicep.Extension.Netbox.Handlers;

public class ManufacturerHandler : NetboxResourceHandlerBase<Manufacturer, SlugIdentifiers>
{
    protected override string ApiPath => "/api/dcim/manufacturers/";

    protected override string GetLookupQuery(Manufacturer properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(Manufacturer properties) => new()
    {
        Slug = properties.Slug
    };
}
