namespace Bicep.Extension.Netbox.Handlers;

public class LocationHandler : SlugResourceHandler<Location>
{
    protected override string ApiPath => "/api/dcim/locations/";
}
