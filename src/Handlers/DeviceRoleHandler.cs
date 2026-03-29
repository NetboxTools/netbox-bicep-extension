namespace Bicep.Extension.Netbox.Handlers;

public class DeviceRoleHandler : SlugResourceHandler<DeviceRole>
{
    protected override string ApiPath => "/api/dcim/device-roles/";
}
