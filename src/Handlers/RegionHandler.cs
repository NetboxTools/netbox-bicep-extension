namespace Bicep.Extension.Netbox.Handlers;

public class RegionHandler : SlugResourceHandler<Region>
{
    protected override string ApiPath => "/api/dcim/regions/";
}
