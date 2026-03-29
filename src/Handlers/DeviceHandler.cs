namespace Bicep.Extension.Netbox.Handlers;

public class DeviceHandler : NetboxResourceHandlerBase<Device, DeviceIdentifiers>
{
    protected override string ApiPath => "/api/dcim/devices/";

    protected override string GetLookupQuery(Device properties) => $"name={properties.Name}";

    protected override DeviceIdentifiers GetIdentifiers(Device properties) => new()
    {
        Name = properties.Name
    };
}
