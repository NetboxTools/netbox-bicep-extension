namespace Bicep.Extension.Netbox.Handlers;

public class SiteHandler : SlugResourceHandler<Site>
{
    protected override string ApiPath => "/api/dcim/sites/";
}
