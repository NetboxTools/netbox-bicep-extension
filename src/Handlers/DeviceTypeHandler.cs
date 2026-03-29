namespace Bicep.Extension.Netbox.Handlers;

public class DeviceTypeHandler : NetboxResourceHandlerBase<DeviceType, SlugIdentifiers>
{
    protected override string ApiPath => "/api/dcim/device-types/";

    protected override string GetLookupQuery(DeviceType properties) => $"slug={properties.Slug}";

    protected override SlugIdentifiers GetIdentifiers(DeviceType properties) => new()
    {
        Slug = properties.Slug
    };
}
