namespace Bicep.Extension.Netbox.Handlers;

public class SiteHandler : NetboxResourceHandlerBase<Site, SlugIdentifiers>
{
    protected override string ApiPath => "/api/dcim/sites/";

    protected override string GetLookupQuery(Site properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(Site properties) => new()
    {
        Slug = properties.Slug
    };
}
