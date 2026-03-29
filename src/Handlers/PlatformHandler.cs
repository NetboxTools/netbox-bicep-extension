namespace Bicep.Extension.Netbox.Handlers;

public class PlatformHandler : SlugResourceHandler<Platform>
{
    protected override string ApiPath => "/api/dcim/platforms/";
}
