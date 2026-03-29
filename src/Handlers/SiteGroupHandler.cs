namespace Bicep.Extension.Netbox.Handlers;

public class SiteGroupHandler : SlugResourceHandler<SiteGroup>
{
    protected override string ApiPath => "/api/dcim/site-groups/";
}
