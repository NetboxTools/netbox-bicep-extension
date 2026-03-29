namespace Bicep.Extension.Netbox.Handlers;

public class ManufacturerHandler : SlugResourceHandler<Manufacturer>
{
    protected override string ApiPath => "/api/dcim/manufacturers/";
}
