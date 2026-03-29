namespace Bicep.Extension.Netbox.Handlers;

public class RackRoleHandler : NetboxResourceHandlerBase<RackRole, SlugIdentifiers>
{
    protected override string ApiPath => "/api/dcim/rack-roles/";

    protected override string GetLookupQuery(RackRole properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(RackRole properties) => new()
    {
        Slug = properties.Slug
    };
}
