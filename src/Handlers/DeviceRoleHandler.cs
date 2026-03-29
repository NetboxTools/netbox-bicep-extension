namespace Bicep.Extension.Netbox.Handlers;

public class DeviceRoleHandler : NetboxResourceHandlerBase<DeviceRole, SlugIdentifiers>
{
    protected override string ApiPath => "/api/dcim/device-roles/";

    protected override string GetLookupQuery(DeviceRole properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(DeviceRole properties) => new()
    {
        Slug = properties.Slug
    };
}
